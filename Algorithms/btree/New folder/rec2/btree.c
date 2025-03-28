/*
 * Binárny vyhľadávací strom — rekurzívna varianta
 *
 * S využitím dátových typov zo súboru btree.h a pripravených kostier funkcií
 * implementujte binárny vyhľadávací strom pomocou rekurzie.
 */

#include "../btree.h"
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
 * Funkciu implementujte rekurzívne bez použitia vlastných pomocných funkcií.
 */
bool bst_search(bst_node_t *tree, char key, int *value) {
  if (!tree)
	{
		return false;
	}

	if (tree->key == key)
	{
		*value = tree->value;
		return true;
	}
	if (key < tree->key)
	{
		return bst_search(tree->left, key, value);
	}
	return bst_search(tree->right, key, value);
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
 * Funkciu implementujte rekurzívne bez použitia vlastných pomocných funkcií.
 */
void bst_insert(bst_node_t **tree, char key, int value) {
  if (!*tree){
    (*tree) = malloc(sizeof(bst_node_t));
    if (!(tree)){
      return;
    }
    (*tree) -> key = key;
    (*tree) -> value = value;
    (*tree) -> left = (*tree) -> right = NULL;
  }
	if ((*tree) -> key == key) 
	{
		(*tree) -> value = value;
		return;
	}

	if (key < (*tree) -> key) 
	{
		bst_insert(&((*tree) -> left), key, value);
		return;
	}
	bst_insert(&((*tree)->right), key, value);
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
 * Funkciu implementujte rekurzívne bez použitia vlastných pomocných funkcií.
 */
void bst_replace_by_rightmost(bst_node_t *target, bst_node_t **tree) {
  bst_node_t *right_most= malloc(sizeof(bst_node_t));
	if (!(*tree)->right) 
	{
		right_most = *tree; 
		target->key = right_most->key;
		target->value = right_most->value;
		target->left = NULL;
		if (right_most->left)
		{
			target->left = right_most->left;
		}

		free(right_most);
		return;
	}

	if (!(*tree)->right->right)
	{
		right_most = (*tree)->right;
		target->key = right_most->key;
		target->value = right_most->value;
		(*tree)->right = NULL;
		if (right_most->left)
		{
			(*tree)->right = right_most->left;
		}

		free(right_most);
		return;
	}
	bst_replace_by_rightmost(target, &((*tree)->right));
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
 * Funkciu implementujte rekurzívne pomocou bst_replace_by_rightmost a bez
 * použitia vlastných pomocných funkcií.
 */
void bst_delete(bst_node_t **tree, char key) {
  if (!(*tree)) return;
  if ((*tree)->key > key){
    bst_delete(&(*tree)->left, key);
  }

  else if ((*tree)->key < key){
    bst_delete(&(*tree)->right, key);
  }
  else if ((*tree)->left && (*tree)->right){
    bst_replace_by_rightmost((*tree), &((*tree)->left));
  }

  else {
    bst_node_t *temp = (*tree);
    if((*tree)->right)
      *tree = (*tree)->right;
    else 
      *tree = (*tree)->left;

    free(temp);
  }
}

/*
 * Zrušenie celého stromu.
 *
 * Po zrušení sa celý strom bude nachádzať v rovnakom stave ako po
 * inicializácii. Funkcia korektne uvoľní všetky alokované zdroje rušených
 * uzlov.
 *
 * Funkciu implementujte rekurzívne bez použitia vlastných pomocných funkcií.
 */
void bst_dispose(bst_node_t **tree) {
  if (!*tree)
	{
		return;
	}

	bst_dispose(&((*tree)->left));
	bst_dispose(&((*tree)->right));
	free(*tree);
	*tree = NULL;
}

/*
 * Preorder prechod stromom.
 *
 * Pre aktuálne spracovávaný uzol nad ním zavolajte funkciu bst_print_node.
 *
 * Funkciu implementujte rekurzívne bez použitia vlastných pomocných funkcií.
 */
void bst_preorder(bst_node_t *tree) {
  if (!tree) return;

  bst_print_node(tree);
  bst_preorder(tree->left);
  bst_preorder(tree->right);
}

/*
 * Inorder prechod stromom.
 *
 * Pre aktuálne spracovávaný uzol nad ním zavolajte funkciu bst_print_node.
 *
 * Funkciu implementujte rekurzívne bez použitia vlastných pomocných funkcií.
 */
void bst_inorder(bst_node_t *tree) {
  if (!tree) return;

  bst_inorder(tree->left);
  bst_print_node(tree);
  bst_inorder(tree->right);
}
/*
 * Postorder prechod stromom.
 *
 * Pre aktuálne spracovávaný uzol nad ním zavolajte funkciu bst_print_node.
 *
 * Funkciu implementujte rekurzívne bez použitia vlastných pomocných funkcií.
 */
void bst_postorder(bst_node_t *tree) {
  if (!tree) return;

  bst_postorder(tree->left);
  bst_postorder(tree->right);
  bst_print_node(tree);
}
