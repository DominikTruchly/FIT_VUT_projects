#include "Lexical_analysis.c"
#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <string.h>
#include <ctype.h>
#include <stdbool.h>

bool writeCheck(Lexeme lex){
    if (lex.kind == ID && strcmp(lex.string,"write") == 0){
        return true;
    }
    return false;
}
bool varCheck(Lexeme lex){
    if (lex.kind == VAR){
        return true;
    }
    return false;
}
bool readCheck(Lexeme lex){
    if (strcmp(lex.string, "readi") == 0){
        return true;
    }
    return false;
}

int main(){
    bool inFunction = false;
    //bool labelWrite = false;
    printf(".IFJcode22\n");
    Lexeme lex = {0};
    while (lex.kind != LEOF){
        lex = getLexeme();
        if (lex.kind == LEOF){
            free(tokenString);
            tokenString = NULL;
            break;
        }
        if (inFunction){}
        if (inFunction == false){
            if(writeCheck(lex)){
                lex = getLexeme();
                lex = getLexeme();
                lex.string++;
                lex.string[strlen(lex.string)-1] = '\0';
                printf("PUSHS string@");
                for (int i = 0; i < (int)(strlen(lex.string)); i++){
                    if (lex.string[i] == ' '){
                        printf("\\032");
                    }
                    else if (lex.string[i] == '#'){
                        printf("\\035");
                    }
                    else if (lex.string[i] == '\\'){
                        printf("\\092");
                    }
                    else{
                        printf("%c",lex.string[i]);
                    }
                }
                printf("\n");
                printf("DEFVAR LF@current\n");
                printf("POPS LF@current\n");
                printf("WRITE LF@current\n");

            }
            if (varCheck(lex)){
                Lexeme temp = lex;
                bool varUndefined = true;
                lex = getLexeme();
                if (lex.kind == ASSIGNMENT && varUndefined ){
                    printf("DEFVAR GF@%s", temp.string);
                    varUndefined = false;
                }
                // lex.string++;
                // printf("DEFVAR LF@%s\n", lex.string);
            }
        }
        free(tokenString);
        tokenString = NULL;
        
    }
    return 0;
}








// if(writeCheck(lex)){
            //     if (labelWrite == false){
            //         labelWrite = true;
            //         printf("LABEL write\n");
            //         printf("CREATEFRAME\n");
            //         printf("PUSHFRAME\n");
            //         printf("DEFVAR LF@current\n");
            //         printf("POPS LF@current");
            //         printf("WRITE LF@current");

            //         printf("PUSHS nil@nil");
            //         printf("POPFRAME");
            //         printf("RETURN");
            //     }
            // }

            // if (varCheck(lex)){
            //     // printf("CREATEFRAME\n");
            //     // printf("PUSHFRAME\n");
            //     // lex.string++;
            //     // printf("DEFVAR LF@%s\n", lex.string);
            // }