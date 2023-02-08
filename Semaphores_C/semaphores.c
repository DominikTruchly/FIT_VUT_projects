#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/mman.h>
#include <sys/wait.h>
#include <sys/stat.h>
#include <limits.h>
#include <semaphore.h>
#include <fcntl.h>
#include <unistd.h>
#include <ctype.h>

#define DEBUG
#define MMAP(pointer) {(pointer) = mmap(NULL, sizeof(*(pointer)), PROT_READ | PROT_WRITE, MAP_SHARED | MAP_ANONYMOUS, -1, 0);}
#define UNMAP(pointer){munmap((pointer), sizeof((pointer)));}
 
FILE *file;

// Semaphores declaration
sem_t *semaforStart = NULL;
sem_t *semaforQueue = NULL;
sem_t *semaforCreateForO = NULL;
sem_t *semaforCreateForH = NULL;
sem_t *semaforCreatedSignal = NULL;
sem_t *semaforNotEnoughElements = NULL;
sem_t *semaforMoleculeCreated = NULL;
sem_t *semaforNextMolecule = NULL;

// Global variables declaration
int *lineNumber = NULL;
int *noM = NULL;
int *created = NULL;
int *allElementsNeeded = 0;
int *printed = 0;
int *usedO = 0;
int *usedH = 0;


int init(){
    // Make global variables shared for processes
    MMAP(usedO);
    MMAP(usedH);
    MMAP(noM);
    MMAP(created);
    MMAP(allElementsNeeded);
    MMAP(lineNumber);
    MMAP(printed);
    MMAP(semaforStart);
    MMAP(semaforQueue);
    MMAP(semaforCreateForO);
    MMAP(semaforCreateForH);
    MMAP(semaforCreatedSignal);
    MMAP(semaforNotEnoughElements);
    MMAP(semaforMoleculeCreated);
    MMAP(semaforNextMolecule);
    // Initialize semaphores
    sem_init(semaforStart,1,1);
    sem_init(semaforQueue,1,1);
    sem_init(semaforCreateForO,1,1);
    sem_init(semaforCreateForH,1,2);
    sem_init(semaforCreatedSignal,1,0);
    sem_init(semaforNotEnoughElements,1,1);
    sem_init(semaforMoleculeCreated,1,0);
    sem_init(semaforNextMolecule,1,0);
    // Open file
    file = fopen("proj2.out", "w");
    return 0;
}

void cleanup(){
    // Clean all allocated shared variables
    UNMAP(usedO);
    UNMAP(usedH);
    UNMAP(noM);
    UNMAP(created);
    UNMAP(allElementsNeeded);
    UNMAP(lineNumber);
    UNMAP(printed);
    UNMAP(semaforStart);
    UNMAP(semaforQueue);
    UNMAP(semaforCreateForO);
    UNMAP(semaforCreateForH);
    UNMAP(semaforCreatedSignal);
    UNMAP(semaforNotEnoughElements);
    UNMAP(semaforMoleculeCreated);
    UNMAP(semaforNextMolecule);
    // Destroy semaphores
    sem_destroy(semaforStart);
    sem_destroy(semaforQueue);
    sem_destroy(semaforCreateForO);
    sem_destroy(semaforCreateForH);
    sem_destroy(semaforCreatedSignal);
    sem_destroy(semaforNotEnoughElements);
    sem_destroy(semaforMoleculeCreated);
    sem_destroy(semaforNextMolecule);
    // Close file
    if(file != NULL) fclose(file);

}

void process_oxygen(int *NH, int idO, int TI, int TB ){
    // Start of element
    sem_wait(semaforStart);
    fprintf(file, "%d: O %d: started\n", ++(*lineNumber),idO);
    fflush(file);
    sem_post(semaforStart);

    // Wait after start
    usleep(rand() %(TI + 1)*1000);

    // Inclusion to the queue
    sem_wait(semaforQueue);
    fprintf(file,"%d: O %d: going to queue\n", ++(*lineNumber),idO);
    fflush(file);
    sem_post(semaforQueue);

    // Clearing queue if no more molecules will be produced
    sem_wait(semaforCreateForO);
    sem_wait(semaforNotEnoughElements);
    if ((*NH - *usedH) < 2){
        fprintf(file,"%d: O %d: not enough H\n", ++(*lineNumber), idO);
        fflush(file);
        sem_post(semaforCreateForO);
        //sem_post(semaforCreateForH);
        sem_post(semaforNotEnoughElements);
        exit(0);
    }
    sem_post(semaforNotEnoughElements);
    // Creating molecule
    fprintf(file,"%d: O %d: creating molecule %d\n", ++(*lineNumber),idO, *noM+1);
    fflush(file);
    *allElementsNeeded += 1;
    
    // Signal that all elements are in molecule
    if (*allElementsNeeded == 3){
        sem_post(semaforMoleculeCreated);
    }

    // Waiting for all elements to be in molecule 
    sem_wait(semaforMoleculeCreated);
    *allElementsNeeded = 0;

    // Simulation of molecule creation
    usleep(rand() %(TB + 1)*1000);
    
    // Sending signal that molecule is created
    sem_post(semaforCreatedSignal);
    sem_post(semaforCreatedSignal);
    sem_wait(semaforCreatedSignal);
    fprintf(file,"%d: O %d: molecule %d created\n", ++(*lineNumber),idO, *noM+1);
    fflush(file);
    sem_post(semaforCreatedSignal);
    *printed += 1;

    // Checking that every element got the molecule creation signal
    if(*printed == 3){
        sem_post(semaforNextMolecule);
        *printed = 0;
    }

    // Preparing for next molecule to be created
    sem_wait(semaforNextMolecule);
    *created = 1;
    *noM +=1;
    *printed = 0;
    *usedO += 1;
    *usedH += 2;
    sem_post(semaforCreateForH);
    sem_post(semaforCreateForH);
    sem_post(semaforCreateForO);
    

    exit(0);
}
void process_hydrogen(int *NO, int *NH, int idH, int TI){
    // Start of element
    sem_wait(semaforStart);
    fprintf(file,"%d: H %d: started\n", ++(*lineNumber),idH );
    fflush(file);
    sem_post(semaforStart);

    // Wait after start
    usleep(rand() %(TI + 1)*1000);

    // Inclusion to the queue
    sem_wait(semaforQueue);
    fprintf(file,"%d: H %d: going to queue\n", ++(*lineNumber),idH);
    fflush(file);
    sem_post(semaforQueue);

    // Clearing queue if no more molecules will be produced
    sem_wait(semaforCreateForH);
    sem_wait(semaforNotEnoughElements);
    if ((*NO-*usedO) < 1 || (*NH-*usedH) < 2){
        fprintf(file,"%d: H %d: not enough O or H\n", ++(*lineNumber), idH);
        fflush(file);
        sem_post(semaforCreateForH);
        sem_post(semaforCreateForO);
        sem_post(semaforNotEnoughElements);
        exit(0);
    }
    sem_post(semaforNotEnoughElements);
    // Creating molecule
    fprintf(file,"%d: H %d: creating molecule %d\n", ++(*lineNumber),idH, *noM+1);
    fflush(file);
    *allElementsNeeded += 1;

    // Signal that all elements are in molecule
    if (*allElementsNeeded == 3){
        sem_post(semaforMoleculeCreated);
    }

    // Waitinf for molecule creation signal from Oxygen 
    sem_wait(semaforCreatedSignal);
    fprintf(file,"%d: H %d: molecule %d created\n", ++(*lineNumber),idH, *noM+1);
    fflush(file);
    *printed += 1;

    // Checking that every element got the molecule creation signal
    if (*printed == 3){
        sem_post(semaforNextMolecule);
        *printed = 0;
    }

    exit(0);
}

int main(int argc, char *argv[]){
    // Checking entry conditions
    if (argc != 5){
        fprintf(stderr, "Wrong number of arguments\n");
        exit(1);
    }
    int NO = atoi(argv[1]);
    int NH = atoi(argv[2]);
    int TI = atoi(argv[3]);
    int TB = atoi(argv[4]);

    if (init (&NO, &NH) == -1){
        cleanup();
        exit(1);
    }
    if (NO < 0 || NH < 0 || TI < 0 || TB < 0 || TI > 1000 || TB > 1000) {
        fprintf(stderr, "Wrong argument value\n");
        cleanup();
        exit(1);
    }
    
    // Making NO processes of oxygen 
    for (int i = 0; i < NO; i ++){
        pid_t element = fork();
        if (element == 0){
            process_oxygen(&NH, i+1, TI, TB);
        }
        else if(element == -1){
            cleanup();
            fprintf(stderr,"Fork oxygen failed");
            exit(1);
        }
    }
  
    // Making NH processes of hydrogen 
    for (int i = 0; i < NH; i ++){
        pid_t element2 = fork();
        if (element2 == 0){
            process_hydrogen(&NO, &NH,i+1, TI);
        }
        else if(element2 == -1){
            cleanup();
            fprintf(stderr,"Fork hydrogen failed");
            exit(1);
        }
    }
    // Wait for all child processes to end
    while(wait(NULL)>0);
    // Clear allocated memory 
    cleanup();
    exit(0);
}