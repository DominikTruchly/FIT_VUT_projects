#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>

int getWordLength(char string[])
{
    // Returns word length of given string
    int wordLength = 0;
    for (int i = 0; string[i] != '\0'; i++) {
        wordLength += 1;
    }
    return (wordLength -1);
}
int compareStrings(char a[], char b[])
{
    // Compares 2 strings, returns 0 if they are the same and 1 if not
    int foundDifference = 0;
    int i = 0;
    while (a[i] != '\0' && b[i] != '\0') {
        if (a[i] != b[i]) {
            foundDifference = 1;
            break;
        }
        i++;
    }
    if (foundDifference == 0) {
        return 0;
    }
    else {
        return 1;
    }
}
int stringToInt(char string[])
{
    // Converts argv[] strings to integers, returns -1 if there are any other characters
    int result = 0;
    for (int i = 0; string[i] != '\0'; i++) {
        if (string[i] > '9' || string[i] < '0')
            return -1;
        result = result * 10 + string[i] - '0';
    }
    return result;
}
int numbersInString(char word[])
{
    // Returns 1 if there are numbers in string, else returns 0
    int numbers = 0;
    for (int i = 0; word[i] != '\0'; i++) {
        char letter = word[i];
        if (letter >= '0' && letter <= '9') {
            numbers = 1;
            break;
        }
    }
    if (numbers == 1) {
        return 1;
    }
    else {
        return 0;
    }
}
int specialCharsInString(char word[])
{
    // Returns 1 if there are special characters in string, else returns 0
    int special = 0;
    for (int i = 0; word[i] != '\0'; i++) {
        char letter = word[i];
        if ((letter >= ' ' && letter <= '/') ||
            (letter >= ':' && letter <= '@') ||
            (letter >= '[' && letter <= '`') ||
            (letter >= '{' && letter <= '~')) {
            special = 1;
        }
    }
    if (special == 1) {
        return 1;
    }
    else {
        return 0;
    }
}
int charSequence(char word[], int len)
{
    // Counts same consecutives chars in string, returns 0 if there are more than parameter
    int same = 1;
    for (int i = 0; word[i] != '\0'; i++) {
        if (same >= len) {
            return 0;
        }
        if (word[i] != word[i - 1]) {
            same = 1;
        }
        if (word[i] == word[i + 1]) {
            same += 1;
        }
    }
    return 1;
}
int level1(char password[])
{
    // Detects passwords with at least one small and one capital letter
    int caps = 0;
    int lows = 0;
    for (int i = 0; password[i] != '\0'; i++) {
        char letter = password[i];
        if (letter >= 'A' && letter <= 'Z') {
            caps = 1;
        }
        if (letter >= 'a' && letter <= 'z') {
            lows = 1;
        }
    }
    if (caps == 1 && lows == 1) {
        return 1;
    }
    return 0;
}
int level2(char password[], int parameter)
{
    // Detects passwords that contains at least (parameter) of different characters
    if (parameter > 4) {
        parameter = 4;
    }
    int caps = 0;
    int lows = 0;
    int nums = 0;
    int uniques = 0;
    for (int i = 0; password[i] != '\0'; i++) {
        char letter = password[i];
        if (letter >= 'A' && letter <= 'Z' ) {
            caps = 1;
        }
        if (letter >= 'a' && letter <= 'z') {
            lows = 1;
        }
        if (numbersInString(password)){
            nums = 1;
        }
        if (specialCharsInString(password)){
            uniques = 1;
        }
    }    
    int differentGroups = caps + lows + nums + uniques;
    if (differentGroups >= parameter){
        return level1(password);
    }
    return 0;
}
int level3(char password[], int len)
{
    // Running level2 and detecs char sequences in password
    if(level2(password, len)){
        return charSequence(password, len);
    }
    return 0;
}
int level4(char password[], int len)
{
    // Finding substrings in password
    // Running level3
    if (level3(password, len) == 0){
        return 0;
    } 
    if (getWordLength(password) < len){
        return level3(password, len);
    }

    // Saving (len) letters to teporary string
    bool foundSubstrings = false;
    for (int i = 0; i < (getWordLength(password)+1-len); i++){
        char word[100] = "";
        int order = 0;
        int count = 0;
        for (int j = i; j < len+i; j++){
            word[order] = password[j];
            order += 1;
        }
        count = 0;
        char *temp = word;

        // Saving (len) letters to teporary string one more time
        for (int i = 0; i < (getWordLength(password)+1-len); i++){
            char word2[100] = "";
            int order = 0;
            for (int j = i; j < len+i; j++){
                word2[order] = password[j];
                order += 1;
            }
            char *temp2 = word2;
            // Comparing substrings and breaking the loop if found more than 1
            int x = compareStrings(temp,temp2);
            if (x == 0){
                count += 1;
            }
            if (count > 1){
            foundSubstrings = true;
            }
        }
        if (foundSubstrings == true){
            break;
        }
        
    }
    if (foundSubstrings){
        return 0;
    }
    else{
        return 1;
    }
}
int statistics(int min, float wordsLength, float numberOfWords, int nchars)
{
    // Printing statistics of all passwords
    printf("Statistika:\n");
    printf("Ruznych znaku: %d\n", nchars-1);
    printf("Minimalni delka: %d\n", min);
    if (numberOfWords == 0){
        float averageLength = 0.0;
        printf("Prumerna delka: %f\n", averageLength);
    }
    else{
        float averageLength = wordsLength / numberOfWords;
        printf("Prumerna delka: %.1f\n", averageLength);
    }
    return 0;
}
int main(int argc, char *argv[])
{
    // Handling wrong arguments and errors
    if (argc<3 || argc>4) {
        fprintf(stderr, "Error: Wrong number of arguments");
        return 1;
    }
    int level = stringToInt(argv[1]);
    int parameter = stringToInt(argv[2]);
    if (parameter < 1 || level < 1) {
        fprintf(stderr, "Error: Wrong argument value");
        return 1;
    }
    if (level > 4 || level == -1) {
        fprintf(stderr, "Error: Wrong argument value");
        return 1;
    }

    // Handling --stats argument 
    int showstats = 0;
    char *trystats = "--stats";
    if (argv[3] != NULL) {
        if (getWordLength(argv[3]) == getWordLength(trystats)) {
            if (compareStrings(argv[3], trystats) == 0) {
                showstats = 1;
            }
        }
        else {
            fprintf(stderr, "Error: Wrong argument value");
            return 1;
        }
    }

    // Variables declaration
    char pass[102];
    int wordsLength = 0;
    int numberOfWords = 0;
    int tempMin = 100;
    int longpass = 0;

    // Creating array to store unique chars in passwords
    bool ncharsArray[127];
    for (int i = 0; i < 127; i++){
        ncharsArray[i] = false;
    }


    while (fgets(pass, 103, stdin) != NULL) {
        // Handling statistics of passwords
        wordsLength += getWordLength(pass);
        numberOfWords += 1;
        if (getWordLength(pass) > 100) {
            longpass = 1;
            break;
        }
        for (int i = 0; pass[i] != '\0'; i++) {
            int value = pass[i];
            ncharsArray[value] = true;
        }
        if (getWordLength(pass) < tempMin) {
            tempMin = getWordLength(pass);
        }
        
        // Starting a correct level with parameter
        if (level == 1 && level1(pass)) {
                printf("%s", pass);
        }
        if (level == 2 && level2(pass,parameter)) {
            printf("%s",pass);
        }
        if (level == 3 && level3(pass,parameter)) {
            printf("%s",pass);
        }
        if (level == 4 && level4(pass, parameter))  {
            printf("%s", pass);
            
        }
    }  
    // Handling too long password
    if (longpass == 1) {
        fprintf(stderr, "Error: Too long password");
        return 1;
    }

    // Showing --stats if passed the handler and getting length of nchars
    if (showstats == 1) {
        int nchars = 0;
        for (int h = 0; h < 127; h++){
            if (ncharsArray[h] == true){
                nchars += 1;
            }
        }
        statistics(tempMin, wordsLength, numberOfWords, nchars);
    }    
    return 0;
}