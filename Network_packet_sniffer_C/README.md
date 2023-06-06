# IPK Project 2 - ZETA: Network sniffer
## Dominik Truchly xtruch01

## Execution
```
./ipk-sniffer [-i interface | --interface interface] {-p port [--tcp|-t] [--udp|-u]} [--arp] [--icmp4] [--icmp6] [--igmp] [--mld] {-n num}
```

## Basic theory
Packet sniffing can be used for legitimate purposes like network troubleshooting and security monitoring to identify and fix issues, optimize network performance, and ensure compliance with regulations. However, it can also be used for malicious activities like stealing data, intercepting passwords, and accessing confidential information. It is important to use packet sniffing responsibly and with proper authorization to avoid legal and ethical issues.

IPv4 and IPv6 are two versions of the Internet Protocol. IPv4 is older and has a 32-bit address space, while IPv6 is newer and has a 128-bit address space, allowing for more unique addresses. IPv6 has a simpler and more efficient header format, built-in security features, and is designed for modern networks. 

## Code specification
Sniffer ipk-sniffer.c is able to analyse network and filter packets base on given filter and interface. It prints basic information about them: timestamp inRFC 3339 format, source MAC, destination MAC, frame length in bytes, source and destination IP addresses, ports and data.
Arguments can be given in any order, and if no protocol is specified all of them are added to the filter.
The program supports IPv4 Options, IPv6 Extension Headers. 

It reads command line arguments to specify the network interface to capture on, as well as filters for specific types of packets (TCP, UDP, ARP, ICMP, etc.) to capture. The program then opens a pcap socket to capture packets, sets the header length based on the type of network link, and starts packet processing using the pcap_loop() function. When a packet is captured, the process_packet() function is called to handle it.

Open_socket function takes in two arguments, device and filter, and returns a pcap_t pointer.
The function first initializes an error buffer and some variables for network device information and packet filtering. It then opens the specified network device for live capture and looks up its IP address and netmask.
Next, it compiles the packet filter expression and binds it to the pcap_t handle. Finally, it frees the memory used by the compiled filter program and returns the handle.

The function process_packet processes a network packet and prints information about it. It prints the timestamp, source and destination MAC addresses, frame length, and the source and destination IP addresses (and ports if it's a TCP/UDP packet).
It parses the Ethernet header, IP header (IPv4 and IPv6), and ARP header to extract relevant information.
If the packet is an ARP packet, it prints the source and destination IP addresses and returns.
Overall, the function is responsible for parsing and printing information about various network packet headers.

The program can be terminated at any given moment with "Ctrl+C".

## Functionality testing
The program was tested on NIX os. I used *nping* to set filters that I wanted to test and wireshark for payload data. I tested it to ensure that sniffer is capable of accurately detecting and analyzing network traffic for the specified interface and protocol.

### Examples
```
sudo ./ipk-sniffer -i eth0 --tcp -p 80
timestamp: 2023-04-17T16:43:13.828+02:00
src MAC: 00:15:5d:21:32:61
dst MAC: 00:15:5d:21:39:bc
frame length: 54 bytes
src IP: 172.26.223.21
dst IP: 142.251.37.110
src port: 47644
dst port: 80

0x0000: 00 15 5d 21 39 bc 00 15 5d 21 32 61 08 00 45 00  ..]!9... ]!2a..E.
0x0010: 00 28 ca 40 00 00 40 06 70 f6 ac 1a df 15 8e fb  .(.@..@. p.......
0x0020: 25 6e ba 1c 00 50 0f 35 a7 37 00 00 00 00 50 02  %n...P.5 .7....P.
0x0030: 05 c8 f9 a7 00 00                                ......

```
```
sudo ./ipk-sniffer --interface eth0 --arp
timestamp: 2023-04-17T16:44:07.628+02:00
src MAC: 00:15:5d:21:32:61
dst MAC: ff:ff:ff:ff:ff:ff
frame length: 42 bytes
src IP: 0.0.142.251
dst IP: 0.0.0.0

0x0000: ff ff ff ff ff ff 00 15 5d 21 32 61 08 06 00 01  ........ ]!2a....
0x0010: 08 00 06 04 00 01 00 15 5d 21 32 61 ac 1a df 15  ........ ]!2a....
0x0020: 00 00 00 00 00 00 8e fb 25 6e                    ........ %n

```
```
sudo ./ipk-sniffer --interface eth0 --icmp4
timestamp: 2023-04-17T16:46:35.478+02:00
src MAC: 00:15:5d:21:32:61
dst MAC: 00:15:5d:21:39:bc
frame length: 42 bytes
src IP: 172.26.223.21
dst IP: 142.251.37.110
0x0000: 00 15 5d 21 39 bc 00 15 5d 21 32 61 08 00 45 00  ..]!9... ]!2a..E.
0x0010: 00 1c 0f d0 00 00 40 01 2b 78 ac 1a df 15 8e fb  ......@. +x......
0x0020: 25 6e 08 00 32 70 c5 8e 00 01                    %n..2p.. ..

```

## Bibliographic citations and other resources

Vladimir Vesely DemoTcp [online]. Publisher: Brno University of Technology, Faculty of Information Technology, January 30th 2023. [cit. 2023-17-04]. Available at: https://git.fit.vutbr.cz/NESFIT/IPK-Projekty/src/branch/master/Stubs/cpp

https://cs.wikipedia.org/wiki/IPv4
https://www.devdungeon.com/content/using-libpcap-c
https://www.tcpdump.org/pcap.html


