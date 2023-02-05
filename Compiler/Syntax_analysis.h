/* ************************ Syntax_analysis.h **************************** */
/* ********** Implemetácia prekladaču imperatívneho jazyka IFJ22 ********* */
/*  Predmet: Formálne jazyky a prekladače (IFJ) - FIT VUT v Brne           */
/*  Team: xondre15, xtruch01, xbrnak01, xhrach06                           */
/*  Vytvoril: xhrach06, xbrnak01                                           */
/* *********************************************************************** */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdbool.h>
#include "Lexical_analysis.c"
#include "Precedent_analysis.c"
//#include "btree.h"
#include "btree.c"

typedef struct STACK {
    int size;
    token *array;
} STACK;

/*
! func_body -> p_prikaz func_body
! func_body -> ε
! assignment -> expression
! assignment -> func_call
! func_call -> ID ( params_in_start )
! params_in_start -> ε
! params_in_start -> params_in params_in_next
! params_in -> expression 
! params_in -> VAR
! params_in_next -> , params_in params_in_next
! params_in_next -> ε
! params -> ε 
! params -> var_types VAR params_next 
! params_next -> , var_types VAR params_next 
! params_next -> ε 
! func_typesq -> ? func_types 
! func_typesq -> func_types
! func_types -> types
! func_types -> void
! var_typesq -> ? types
! var_typesq -> types
! types -> int
! types -> float
! types -> string
! func_def -> function ID ( params ) : func_types { func_body }
! p_prikaz -> return ret_value ;
! p_prikaz -> function ID ( params ) : func_types { func_body }
! p_prikaz -> VAR = assignment ;
! p_prikaz -> func_call ;
! p_prikaz -> if ( expression ) { p_prikaz } else { p_prikaz } 
! p_prikaz -> while ( expression ) { p_prikaz }
! p_start  -> prologue p_body p_end
! prologue -> <?php declare ( strict_types = 1 ) ;
! p_end -> EOF
! p_end -> ?>
! p_body -> ε
! p_body -> p_prikaz p_body
*/
