/*
 * Binárny vyhľadávací strom — iteratívna varianta
 *
 * S využitím dátových typov zo súboru btree.h, zásobníkov zo súborov stack.h a
 * stack.c a pripravených kostier funkcií implementujte binárny vyhľadávací
 * strom bez použitia rekurzie.
 */

#include "../btree.h"
#include "stack.h"
#include <stdio.h>
#include <stdlib.h>

/*
 * Inicializácia stromu.
 *
 * Užívateľ musí zaistiť, že incializácia sa nebude opakovane volať nad
 * inicializovaným stromom. V opačnom prípade môže dôjsť k úniku pamäte (memory
 * leak). Keďže neinicializovaný ukazovateľ má nedefinovanú hodnotu, nie je
 * možné toto detegovať vo funkcii.
 */
void bst_init(bst_node_t **tree) {
  *tree = NULL;
}

/*
 * Nájdenie uzlu v strome.
 *
 * V prípade úspechu vráti funkcia hodnotu true a do premennej value zapíše
 * hodnotu daného uzlu. V opačnom prípade funckia vráti hodnotu false a premenná
 * value ostáva nezmenená.
 *
 * Funkciu implementujte iteratívne bez použitia vlastných pomocných funkcií.
 */
bool bst_search(bst_node_t *tree, char key, int *value) {
  if (!tree) return false;
  bst_node_t *item = tree;
  while (item) {
    if (item->key == key) {
      *value = item->value;
      return true;
    }
    else if (item->key < key){ 
		item = item->right;
	}
    else if (item->key > key){ 
		item = item->left;
	}
  }
  return false;
}

/*
 * Vloženie uzlu do stromu.
 *
 * Pokiaľ uzol so zadaným kľúčom v strome už existuje, nahraďte jeho hodnotu.
 * Inak vložte nový listový uzol.
 *
 * Výsledný strom musí spĺňať podmienku vyhľadávacieho stromu — ľavý podstrom
 * uzlu obsahuje iba menšie kľúče, pravý väčšie.
 *
 * Funkciu implementujte iteratívne bez použitia vlastných pomocných funkcií.
 */
void bst_insert(bst_node_t **tree, char key, int value) {
  if (!(*tree)) {
    (*tree) = malloc(sizeof(bst_node_t));
	if ((*tree) == NULL){
		return;
	}
	else{
		(*tree)->key = key;
		(*tree)->value = value;
		(*tree)->left = (*tree)->right = NULL;
		return;
	}
  }
  
  bst_node_t *item = *tree;
  
  while (true) {
    if (item->key < key) {
      if (item->right == NULL) {
        bst_node_t *insert = malloc (sizeof(bst_node_t));
        insert->key = key;
        insert->value = value;
        item->right = insert;
		insert->right = NULL;
		insert->left = NULL;
        return;
      }
      item = item->right;
    }

    else if (item->key > key) {
      if (item->left == NULL) {
        bst_node_t *insert = malloc (sizeof(bst_node_t));
        insert->key = key;
        insert->value = value;
        item->left = insert;
		insert->left = insert->right = NULL;
        return;
      }
      item = item->left;
    }
	
    else {
      item->value = value;
      return;
    }
  }
}

/*
 * Pomocná funkcia ktorá nahradí uzol najpravejším potomkom.
 *
 * Kľúč a hodnota uzlu target budú nahradené kľúčom a hodnotou najpravejšieho
 * uzlu podstromu tree. Najpravejší potomok bude odstránený. Funkcia korektne
 * uvoľní všetky alokované zdroje odstráneného uzlu.
 *
 * Funkcia predpokladá že hodnota tree nie je NULL.
 *
 * Táto pomocná funkcia bude využitá pri implementácii funkcie bst_delete.
 *
 * Funkciu implementujte iteratívne bez použitia vlastných pomocných funkcií.
 */
void bst_replace_by_rightmost(bst_node_t *target, bst_node_t **tree) {
  bst_node_t *previous = *tree;
  bst_node_t *item = *tree;
  while (item->right) {
    previous = item;
    item = item->right;
  }
  
  target->key = item->key;
  target->value = item->value;

  previous->right = NULL;
  if (item->left) {
    previous->right = item->left;
  }

  free(item);
}

/*
 * Odstránenie uzlu v strome.
 *
 * Pokiaľ uzol so zadaným kľúčom neexistuje, funkcia nič nerobí.
 * Pokiaľ má odstránený uzol jeden podstrom, zdedí ho otec odstráneného uzla.
 * Pokiaľ má odstránený uzol oba podstromy, je nahradený najpravejším uzlom
 * ľavého podstromu. Najpravejší uzol nemusí byť listom!
 * Funkcia korektne uvoľní všetky alokované zdroje odstráneného uzlu.
 *
 * Funkciu implementujte iteratívne pomocou bst_replace_by_rightmost a bez
 * použitia vlastných pomocných funkcií.
 */
void bst_delete(bst_node_t **tree, char key) {
  if (!(*tree)) return;
  bst_node_t *item = *tree;
  bst_node_t *previous = *tree;
  bool left = false;

  while (item) {
    if (item->key == key) {
      break;
    }
    else if (item->key < key) {
      previous = item;
      item = item->right;
      left = false;
    }
    else if (item->key > key) {
      previous = item;
      item = item->left;
      left = true;
    }
  }
  if (item == NULL) return;

  if (item->left && item->right) {
    bst_replace_by_rightmost(item, &(item->left));
  }
  else {
    if (item->right) {
      if (left){
	  	previous->left = item->right;
		}
      else {
		previous->right = item->right;
	  }
    }
    else {
      if (left){ 
		previous->left = item->left;
	  }
      else {
		previous->right = item->left;
	  }
    }

    free(item);
  }
}

/*
 * Zrušenie celého stromu.
 *
 * Po zrušení sa celý strom bude nachádzať v rovnakom stave ako po
 * inicializácii. Funkcia korektne uvoľní všetky alokované zdroje rušených
 * uzlov.
 *
 * Funkciu implementujte iteratívne pomocou zásobníku uzlov a bez použitia
 * vlastných pomocných funkcií.
 */
void bst_dispose(bst_node_t **tree) {
  	stack_bst_t stack;
	stack_bst_init(&stack);
	bst_node_t *delete;
	while ((*tree) || !stack_bst_empty(&stack)) {
		if (!(*tree)) {
			(*tree) = stack_bst_pop(&stack);
		}
		if ((*tree)->right) {
			stack_bst_push(&stack, (*tree)->right);
		}
		delete = (*tree);
		(*tree) = (*tree)->left;
		free(delete);
	}
}

/*
 * Pomocná funkcia pre iteratívny preorder.
 *
 * Prechádza po ľavej vetve k najľavejšiemu uzlu podstromu.
 * Nad spracovanými uzlami zavola bst_print_node a uloží ich do zásobníku uzlov.
 *
 * Funkciu implementujte iteratívne pomocou zásobníku uzlov a bez použitia
 * vlastných pomocných funkcií.
 */
void bst_leftmost_preorder(bst_node_t *tree, stack_bst_t *to_visit) {
  bst_node_t *item = tree;
  while (item) {
    bst_print_node(item);
    stack_bst_push(to_visit, item);
    item = item->left;
  }
}

/*
 * Preorder prechod stromom.
 *
 * Pre aktuálne spracovávaný uzol nad ním zavolajte funkciu bst_print_node.
 *
 * Funkciu implementujte iteratívne pomocou funkcie bst_leftmost_preorder a
 * zásobníku uzlov bez použitia vlastných pomocných funkcií.
 */
void bst_preorder(bst_node_t *tree) {
  stack_bst_t *stack = malloc(sizeof(stack_bst_t));
  stack_bst_init(stack);
  bst_leftmost_preorder(tree, stack);

  while (!stack_bst_empty(stack)) {
    tree = stack_bst_pop(stack);
    bst_leftmost_preorder(tree->right, stack);
  }

  free(stack);
}

/*
 * Pomocná funkcia pre iteratívny inorder.
 *
 * Prechádza po ľavej vetve k najľavejšiemu uzlu podstromu a ukladá uzly do
 * zásobníku uzlov.
 *
 * Funkciu implementujte iteratívne pomocou zásobníku uzlov a bez použitia
 * vlastných pomocných funkcií.
 */
void bst_leftmost_inorder(bst_node_t *tree, stack_bst_t *to_visit) {
	while (tree != NULL) {
		stack_bst_push(to_visit, tree);
		tree = tree->left;
	}
}

/*
 * Inorder prechod stromom.
 *
 * Pre aktuálne spracovávaný uzol nad ním zavolajte funkciu bst_print_node.
 *
 * Funkciu implementujte iteratívne pomocou funkcie bst_leftmost_inorder a
 * zásobníku uzlov bez použitia vlastných pomocných funkcií.
 */
void bst_inorder(bst_node_t *tree) {
  stack_bst_t *stack = malloc(sizeof(stack_bst_t));
  stack_bst_init(stack);

  bst_leftmost_inorder(tree, stack);

  while(!stack_bst_empty(stack)) {
    tree = stack_bst_pop(stack);
    bst_print_node(tree);
    bst_leftmost_inorder(tree->right, stack);
  }

  free(stack);
}

/*
 * Pomocná funkcia pre iteratívny postorder.
 *
 * Prechádza po ľavej vetve k najľavejšiemu uzlu podstromu a ukladá uzly do
 * zásobníku uzlov. Do zásobníku bool hodnôt ukladá informáciu že uzol
 * bol navštívený prvý krát.
 *
 * Funkciu implementujte iteratívne pomocou zásobníkov uzlov a bool hodnôt a bez použitia
 * vlastných pomocných funkcií.
 */
void bst_leftmost_postorder(bst_node_t *tree, stack_bst_t *to_visit,
                            stack_bool_t *first_visit) {
  bst_node_t *item = tree;
  while (item) {
    stack_bst_push(to_visit, item);
    stack_bool_push(first_visit, true);
    item = item->left;
  }
}

/*
 * Postorder prechod stromom.
 *
 * Pre aktuálne spracovávaný uzol nad ním zavolajte funkciu bst_print_node.
 *
 * Funkciu implementujte iteratívne pomocou funkcie bst_leftmost_postorder a
 * zásobníkov uzlov a bool hodnôt bez použitia vlastných pomocných funkcií.
 */
void bst_postorder(bst_node_t *tree) {
  stack_bst_t *bst_stack = malloc(sizeof(stack_bst_t));
  stack_bool_t *bool_stack = malloc(sizeof(stack_bool_t));
  bool itemBool;
  bst_node_t *itemBst = tree;

  stack_bst_init(bst_stack);
  stack_bool_init(bool_stack);
  bst_leftmost_postorder(tree, bst_stack, bool_stack);

  while (!stack_bst_empty(bst_stack)) {
    itemBst = stack_bst_pop(bst_stack);
    itemBool = stack_bool_pop(bool_stack);
    if (itemBool) {
      stack_bst_push(bst_stack, itemBst);
      stack_bool_push(bool_stack, false);
      bst_leftmost_postorder(itemBst->right, bst_stack, bool_stack);
    }
    else bst_print_node(itemBst);
  }

  free(bst_stack);
  free(bool_stack);
}
