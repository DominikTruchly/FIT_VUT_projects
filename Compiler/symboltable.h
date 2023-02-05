/* ************************ symboltable.h  ******************************* */
/* ********** Implemetácia prekladaču imperatívneho jazyka IFJ22 ********* */
/*  Predmet: Formálne jazyky a prekladače (IFJ) - FIT VUT v Brne           */
/*  Team: xondre15, xtruch01, xbrnak01, xhrach06                           */
/*  Vytvoril: xhrach06, xbrnak01                                           */
/* *********************************************************************** */
typedef enum{
    T_VOID, T_INT, T_FLOAT, T_STRING
} type;

typedef struct {
    //char *name;
    type type;
    //int value;
    //float fvalue;
    //int line;
    bool param_is_q;
    //int column;
    //char *string;
} token;





typedef struct NODE_LOCAL
{
    char *key;
    struct NODE_LOCAL *left;
    struct NODE_LOCAL *right;
    token *token;
} NODE_LOCAL;

typedef struct NODE{
    char *key;
    struct NODE *left;
    struct NODE *right;
    int param_count;
    type return_type;
    bool return_type_is_q;
    type params[32];
    char *params_names[32];

    NODE_LOCAL *local;

    //token *tokens;
} NODE;
