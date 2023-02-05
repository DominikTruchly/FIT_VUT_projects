/* ************************ Lexical_analysis.c *************************** */
/* ********** Implemetácia prekladaču imperatívneho jazyka IFJ22 ********* */
/*  Predmet: Formálne jazyky a prekladače (IFJ) - FIT VUT v Brne           */
/*  Team: xondre15, xtruch01, xbrnak01, xhrach06                           */
/*  Vytvoril: xtruch01                                                     */
/* *********************************************************************** */

typedef enum{
        STRING,
        ASSIGNMENT,
        EQUALS,
        NOTEQ,
        NOT,
        RCURLY,
        LCURLY,
        RBRACKET,
        LBRACKET,
        GREATER,
        GREATEREQ,
        LESS,
        LESSEQ,
        PROLOGUE,
        SUBSTRACTION,
        ADDITION,
        MULTIPLICATION,
        DIVISION,
        COMMENT,
        VAR,
        INT,
        FLOATA,
        FLOATB,
        ID,
        KEYWORD,
        END,
        DOT,
        COMMA,
        COLON,
        SEMICOLON,    
        LEOF,
    } Kind;

typedef struct{
    Kind kind;
    char *string;
    int value;
    float fvalue;
    int symtab_index;
    int lineNumber;
    } Lexeme;
Lexeme getLexeme();
