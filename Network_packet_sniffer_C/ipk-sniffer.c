#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <strings.h>
#include <stdint.h>
#include <time.h>
#include <pcap.h>
#include <arpa/inet.h>
#include <netinet/ether.h>
#include <netinet/ip.h>
#include <netinet/ip6.h>
#include <netinet/tcp.h>
#include <netinet/udp.h>
#include <netinet/if_ether.h>
#include <getopt.h>
#include <ctype.h>
#include <signal.h>
#include <stdbool.h>
#include <netinet/icmp6.h>

// Global variables
pcap_t* packet_processor;
int header_length;
struct icmp6_hdr *icmp6hdr;


void printHelp() {
    printf("usage: ./ipk-sniffer [-i interface | --interface interface] {-p port [--tcp|-t] [--udp|-u]} [--arp] [--icmp4] [--icmp6] [--igmp] [--mld] {-n num}\n");
    printf("    -h               help message\n");
    printf("    -i               specify interface\n");
    printf("    -p               set packet port to filter\n");
    printf("    -t               filter TCP\n");
    printf("    -u               filter UDP\n");
    printf("    --arp            filter ARP\n");
    printf("    --icmp           filter ICMPv4, ICMPv6\n");
    printf("    -n               set packet limit (unlimited if not set)\n");
}



// Clears the pcap descriptor and exits the program
void clear() {
    // close the pcap descriptor
    pcap_close(packet_processor);
    exit(EXIT_SUCCESS);
}


pcap_t* open_socket(char* device, char* filter) {
    // error buffer for pcap_functions
    char errbuf[PCAP_ERRBUF_SIZE];
    
    pcap_t *temphandle = NULL;
    uint32_t source_ip;
    uint32_t netmask;
    
    // Filtering
    struct bpf_program bpf;

    // Opening the device for live capture
    if ((temphandle = pcap_open_live(device, BUFSIZ, 1, 1000, errbuf)) == NULL) {
        fprintf(stderr, "pcap_open_live(): %s\n", errbuf);
        return NULL;
    }

    // Gset network device source IP address and netmask.
    if (pcap_lookupnet(device, &source_ip, &netmask, errbuf) < 0) {
        fprintf(stderr, "pcap_lookupnet(): %s\n", errbuf);
        pcap_close(temphandle);
        return NULL;
    }

    // convert the packet filter expression into a packet filter binary.
    if (pcap_compile(temphandle, &bpf, filter, 1, netmask) == -1)  {
        fprintf(stderr, "pcap_compile(): %s\n", pcap_geterr(temphandle));
        pcap_close(temphandle);
        return NULL;
    }

    // bind the packet filter to the libpcap temphandle.    
    if (pcap_setfilter(temphandle, &bpf) < 0) {
        fprintf(stderr, "pcap_setfilter(): %s\n", pcap_geterr(temphandle));
        pcap_freecode(&bpf);
        pcap_close(temphandle);
        return NULL;
    }

    // free the memory used by the compiled filter program
    pcap_freecode(&bpf);

    return temphandle;
}

void printActiveInterfaces() {
    pcap_if_t *interfaces;
    char errbuf[PCAP_ERRBUF_SIZE];

    // Find all active interfaces
    if (pcap_findalldevs(&interfaces, errbuf) == -1) {
        fprintf(stderr, "Error finding devices: %s\n", errbuf);
        exit(EXIT_FAILURE);
    }

    // Print out interface information
    printf("Active interfaces:\n");
    for (pcap_if_t *iface = interfaces; iface != NULL; iface = iface->next) {
        printf("\t%s\n", iface->name);

    }

    // Free the list of interfaces
    pcap_freealldevs(interfaces);
}

void printData(unsigned char *packet, struct pcap_pkthdr * header) {
    int j;
    for (int i = 0; i < header->caplen; i += 16) {
        printf("0x%04x: ", i);
        for (j = 0; j < 16; j++) {
            if (i + j < header->caplen) {
                printf("%02x ", packet[i + j]);
            } else {
                printf("   ");
            }
        }
        printf(" ");
        for (int j = 0; j < 16; j++) {
            if (i + j < header->caplen) {
                if (j==8) {
                    printf(" ");
                }
                if (isprint(packet[i + j])) {
                    printf("%c", packet[i + j]);
                } else {
                    printf(".");
                }
            }
        }
        printf("\n");
    }
}

void process_packet(u_char *args, struct pcap_pkthdr *header, u_char *packet) {
    char src_ip[256];
    char dst_ip[256];


    char timestamp[64];
    time_t seconds = header->ts.tv_sec;
    struct timeval tv = header->ts;
    struct tm *local = localtime(&seconds);
    strftime(timestamp, sizeof(timestamp), "%Y-%m-%dT%H:%M:%S", local);

    char tz_offset[7];
    int hours = (int)(local->tm_gmtoff / 3600);
    int minutes = (int)((local->tm_gmtoff / 60) % 60);
    snprintf(tz_offset, sizeof(tz_offset), "%+03d:%02d", hours, minutes);

    snprintf(timestamp + strlen(timestamp), sizeof(timestamp) - strlen(timestamp), ".%03d%s", (int)tv.tv_usec / 1000, tz_offset);

    printf("timestamp: %s\n", timestamp);


    // Parse Ethernet header
    const struct ether_header *eth_header;
    eth_header = (struct ether_header *)packet;
    int etherType = ntohs(eth_header->ether_type);
    printf("src MAC: %02x:%02x:%02x:%02x:%02x:%02x\n", eth_header->ether_shost[0], eth_header->ether_shost[1], eth_header->ether_shost[2], eth_header->ether_shost[3], eth_header->ether_shost[4], eth_header->ether_shost[5]);
    printf("dst MAC: %02x:%02x:%02x:%02x:%02x:%02x\n", eth_header->ether_dhost[0], eth_header->ether_dhost[1], eth_header->ether_dhost[2], eth_header->ether_dhost[3], eth_header->ether_dhost[4], eth_header->ether_dhost[5]);
    
    // Print frame length
    printf("frame length: %d bytes\n", header->len);    

    // Parse IP header
    bool ipv4_flag = false;
    struct ip* iphdr;
    struct ip6_hdr* ip6hdr;
    struct arphdr* arphdr;

    int offsetHeader = 0;
    int nextHeader = -1;
    
    u_char * tempPtr = packet + header_length;

    // IPV4 and IPV6 header length is different
    // Parse IPv4 header
    if (etherType == 2048) {
        ipv4_flag = true;
        iphdr = (struct ip *) tempPtr;

        inet_ntop(AF_INET, &(iphdr->ip_src), src_ip, INET_ADDRSTRLEN);
        inet_ntop(AF_INET, &(iphdr->ip_dst), dst_ip, INET_ADDRSTRLEN);

        tempPtr += 4 * iphdr->ip_hl;
     
    } 
    // Parse IPv6 header
    else if (etherType == 34525) {
        ipv4_flag = false;
        ip6hdr = (struct ip6_hdr *) tempPtr;

        
        inet_ntop(AF_INET6, &(ip6hdr->ip6_src), src_ip, INET6_ADDRSTRLEN);
        inet_ntop(AF_INET6, &(ip6hdr->ip6_dst), dst_ip, INET6_ADDRSTRLEN);

        
        nextHeader = ip6hdr->ip6_nxt;
        offsetHeader += 40;
        tempPtr += 40;

        switch (nextHeader) {
            case IPPROTO_ROUTING:;
                struct ip6_rthdr *header =  (struct ip6_rthdr *) tempPtr;
                tempPtr += sizeof(struct ip6_rthdr);
                offsetHeader += sizeof(struct ip6_rthdr);
                nextHeader = header->ip6r_nxt;
                break;

            case IPPROTO_HOPOPTS:;
                struct ip6_hbh *header1 =  (struct ip6_hbh *) tempPtr;
                tempPtr += sizeof(struct ip6_hbh);
                offsetHeader += sizeof(struct ip6_hbh);
                nextHeader = header1->ip6h_nxt;
                break;

            case IPPROTO_FRAGMENT:;
                struct ip6_frag *header2 =  (struct ip6_frag *) tempPtr;
                tempPtr += sizeof(struct ip6_frag);
                offsetHeader += sizeof(struct ip6_frag);
                nextHeader = header2->ip6f_nxt;
                break;

            case IPPROTO_DSTOPTS:;
                struct ip6_dest * header3 =  (struct ip6_dest *) tempPtr;
                packet += sizeof(struct ip6_dest);
                tempPtr += sizeof(struct ip6_dest);
                nextHeader = header3->ip6d_nxt;
                break;

            default:
                break;
        }

    } 
    else if (etherType == 2054) {
        // ARP
        arphdr = (struct arphdr *) tempPtr;
        tempPtr += sizeof(struct arphdr);
        if (ntohs(arphdr->ar_hrd) == ARPHRD_ETHER && ntohs(arphdr->ar_pro) == ETHERTYPE_IP) {
            struct ether_arp* arp = (struct ether_arp*)tempPtr;
            inet_ntop(AF_INET, arp->arp_spa, src_ip, INET_ADDRSTRLEN);
            inet_ntop(AF_INET, arp->arp_tpa, dst_ip, INET_ADDRSTRLEN);
            printf("src IP: %s\n", src_ip);
            printf("dst IP: %s\n\n", dst_ip);
            printData(packet, header);
            printf("\n\n");
            return;
        }
    } 
    else {
        return;
    }

    // Print IP addresses and ports
    struct tcphdr* tcphdr;
    struct udphdr* udphdr;
    switch (ipv4_flag ? iphdr->ip_p : nextHeader) {    
        // TCP
        case IPPROTO_TCP:
            tcphdr = (struct tcphdr *) tempPtr;
            printf("src IP: %s\n", src_ip);
            printf("dst IP: %s\n", dst_ip);
            printf("src port: %d\n", ntohs(tcphdr->th_sport));
            printf("dst port: %d\n\n", ntohs(tcphdr->th_dport));
            printData(packet, header);
            printf("\n\n");
            break;
    
        // UDP
        case IPPROTO_UDP:
            udphdr = (struct udphdr *) tempPtr;
            printf("src IP: %s\n", src_ip);
            printf("dst IP: %s\n", dst_ip);
            printf("src port: %d\n", ntohs(udphdr->uh_sport));
            printf("dst port: %d\n\n", ntohs(udphdr->uh_dport));
            printData(packet, header);
            printf("\n\n");
            break;

        // ICMP
        case IPPROTO_ICMP:
            printf("src IP: %s\n", src_ip);
            printf("dst IP: %s\n", dst_ip);
            printData(packet, header);
            printf("\n\n");
            break;

        // ICMPv6
        case IPPROTO_ICMPV6:
            icmp6hdr = (struct icmp6_hdr *) tempPtr;
            printf("src IP: %s\n", src_ip);
            printf("dst IP: %s\n", dst_ip);
            printData(packet, header);
            printf("\n\n");
            break;

        // IGMP
        case IPPROTO_IGMP:
            printf("src IP: %s\n", src_ip);
            printf("dst IP: %s\n", dst_ip);
            printf("src port: %d\n", 0);
            printf("dst port: %d\n\n", 0);
            printData(packet, header);
            printf("\n\n");
            break;

        default:
            printf("src IP: %s\n", src_ip);
            printf("dst IP: %s\n", dst_ip);
            printData(packet, header);
            break;
    }

}

int main(int argc, char **argv){
    // Initialize variables
    bool TCP = false;
    bool UDP = false;
    bool ARP = false;
    bool ICMP4 = false;
    bool ICMP6 = false;
    bool IGMP = false;
    bool MLD = false;
    bool NDP = false;

    char interface[256] = "";
    char port[16] = "";
    char filter[256] = "";
    
    int filterCounter = 0;
    int limit = 1;

    // Register signal handlers
    signal(SIGINT, clear);
    signal(SIGTERM, clear);
    signal(SIGQUIT, clear);

    // Long options for parsing
    static struct option long_options[] = {
        {"help", no_argument, 0, 'h'},
        {"interface", required_argument, 0, 'i'},
        {"tcp", no_argument, 0, 't'},
        {"udp", no_argument, 0, 'u'},
        {"arp", no_argument, 0, 'a'},
        {"icmp4", no_argument, 0, '4'},
        {"icmp6", no_argument, 0, '6'},
        {"igmp", no_argument, 0, 'g'},
        {"mld", no_argument, 0, 'm'},
        {"ndp", no_argument, 0, 'd'},
        {0, 0, 0, 0}
    };

    bool portSet = false;
    // Parse command line options
    int c;
    while((c = getopt_long(argc, argv, "hi:n:utp:", long_options, NULL)) != -1) { 
        switch(c) { 
             case 'h': 
                printHelp();
                exit(EXIT_SUCCESS);
                break;
            case 'i': 
                if (optarg != NULL){
                    strcat(interface, optarg);
                }
                break;
            case 'p': 
                portSet = true;
                strcat(port, optarg);
                break; 
            case 't': 
                TCP = true;
                break; 
            case 'u': 
                UDP = true;
                break; 
            case 'n': 
                limit = atoi(optarg);
                break; 
            case 'a': 
                ARP = true;
                break; 
            case '4': 
                ICMP4 = true;
                break;
            case '6':
                ICMP6 = true;
                break;
            case 'm':
                MLD = true;
                break;
            case 'g':
                IGMP = true;
                break;
            case 'd':
                NDP = true;
                break;
        } 
    }

    // Check for empty -i or --interface
    if (strcmp(interface, "") == 0) {
        printActiveInterfaces();
        exit(EXIT_FAILURE);
    }


    // Build expression for pcap filter
    if (TCP) {
        if (port != NULL && strlen(port) > 0) {
            strcat(filter, "tcp port ");
            strcat(filter, port);
            strcat(filter, " or ");
        }
        else {
            strcat(filter, "tcp or ");
        }
    }
    if (UDP) {
        if (port != NULL && strlen(port) > 0) {
            strcat(filter, "udp port ");
            strcat(filter, port);
            strcat(filter, " or ");
        }
        else {
            strcat(filter, "udp or ");
        }
    }    
    if (ARP) strcat(filter, "arp or ");
    if (ICMP4) strcat(filter, "icmp or ");
    if (ICMP6) strcat(filter, "icmp6 or ");
    if (IGMP) strcat(filter, "igmp or ");
    if (MLD) strcat(filter, "(icmp6 and ip6[40] == 143) or (icmp and ip[20] == 130) or ");
    if (NDP) strcat(filter, "(icmp6 and ip6[40] == 135) or ");

    if (strlen(filter) == 0) {
        strcat(filter, "tcp or udp or arp or icmp or icmp6 or igmp or (icmp6 and ip6[40] == 143) or (icmp and ip[20] == 130) or (icmp6 and ip6[40] == 135) or ");
    }

    // Remove last OR
    if (strlen(filter) > 3) {
        filter[strlen(filter) - 3] = '\0';
    }

    // open the pcap socket
    packet_processor = open_socket(interface, filter);
    if (packet_processor == NULL) {
        exit(EXIT_FAILURE);
    }

    // set header length
    int link_type; 
    bool link = true;
    if ((link_type = pcap_datalink(packet_processor)) < 0) {
        printf("pcap_datalink(): %s\n", pcap_geterr(packet_processor));
        link = false;
    }
    if (link){
        switch (link_type) {
            case DLT_NULL:
                header_length = 4;
                break;

            case DLT_EN10MB:
                header_length = 14;
                break;

            case DLT_SLIP:
            case DLT_PPP:
                header_length = 24;
                break;

            case DLT_LINUX_SLL:
                header_length = 16;
                break;

            default:
                printf("Unsupported datalink (%d)\n", link_type);
                break;
        }
    }
    if (header_length == 0) {
        exit(EXIT_FAILURE);
    }

    // Start packet processing
    if (pcap_loop(packet_processor, limit, (pcap_handler) process_packet, NULL) < 0) {
        fprintf(stderr, "pcap_loop(): %s\n", pcap_geterr(packet_processor));
        pcap_close(packet_processor);
        exit(EXIT_FAILURE);
    }
    
    clear();

}