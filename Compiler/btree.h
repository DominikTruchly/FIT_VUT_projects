/*
 * Hlavičkový súbor pre binárny vyhľadávací strom.
 * Tento súbor neupravujte.
 */

#ifndef IAL_BTREE_H
#define IAL_BTREE_H

#include <stdbool.h>
#include "symboltable.h"

// Uzol stromu
/* typedef struct bst_node {
  char key;               // kľúč
  int value;              // hodnota
  struct bst_node *left;  // ľavý potomok
  struct bst_node *right; // pravý potomok
} bst_node_t; */ 

void bst_init(NODE **tree);
void bst_insert(NODE **tree, char *key); //key je nazov aktualnej funkcie
void bst_insert_local(NODE_LOCAL **tree, char *key, token *token); //key je nazov vkladaneho parametru albeo premennej



NODE *bst_search(NODE *tree, char *key);
token *bst_search_local(NODE_LOCAL *tree, char *key);


void bst_delete(NODE **tree, char *key);
void bst_dispose(NODE **tree);
void bst_dispose_local(NODE_LOCAL **tree);


void bst_replace_by_rightmost(NODE *target, NODE **tree);

void bst_print_node(NODE *node);

#endif
