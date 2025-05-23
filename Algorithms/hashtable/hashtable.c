/*
 * Tabuľka s rozptýlenými položkami
 *
 * S využitím dátových typov zo súboru hashtable.h a pripravených kostier
 * funkcií implementujte tabuľku s rozptýlenými položkami s explicitne
 * zreťazenými synonymami.
 *
 * Pri implementácii uvažujte veľkosť tabuľky HT_SIZE.
 */

#include "hashtable.h"
#include <stdlib.h>
#include <string.h>

int HT_SIZE = MAX_HT_SIZE;

/*
 * Rozptyľovacia funkcia ktorá pridelí zadanému kľúču index z intervalu
 * <0,HT_SIZE-1>. Ideálna rozptyľovacia funkcia by mala rozprestrieť kľúče
 * rovnomerne po všetkých indexoch. Zamyslite sa nad kvalitou zvolenej funkcie.
 */
int get_hash(char *key) {
	int result = 1;
	int length = strlen(key);
	for (int i = 0; i < length; i++) {
		result += key[i];
	}
	return (result % HT_SIZE);
}

/*
 * Inicializácia tabuľky — zavolá sa pred prvým použitím tabuľky.
 */
void ht_init(ht_table_t *table) {
  for (int i = 0; i < HT_SIZE; i++)
	{
		(*table)[i] = NULL;
	}
}

/*
 * Vyhľadanie prvku v tabuľke.
 *
 * V prípade úspechu vráti ukazovateľ na nájdený prvok; v opačnom prípade vráti
 * hodnotu NULL.
 */
ht_item_t *ht_search(ht_table_t *table, char *key) {
	if (!table || !key) return NULL;

	int index = get_hash(key);

	if (table == NULL) {
		return NULL;
	}
	for (ht_item_t *temp = (*table)[index]; temp != NULL; temp = temp->next)
	if (strcmp(temp->key, key) == 0) {
		return temp;
	}
	return NULL;
}

/*
 * Vloženie nového prvku do tabuľky.
 *
 * Pokiaľ prvok s daným kľúčom už v tabuľke existuje, nahraďte jeho hodnotu.
 *
 * Pri implementácii využite funkciu ht_search. Pri vkladaní prvku do zoznamu
 * synonym zvoľte najefektívnejšiu možnosť a vložte prvok na začiatok zoznamu.
 */
void ht_insert(ht_table_t *table, char *key, float value) {
	ht_item_t *current = ht_search(table, key);
	if (current)
	{
		current->value = value;
		return;
	}
	ht_item_t *new = (ht_item_t *) malloc(sizeof(ht_item_t));
	if (!new)
	{
		return;
	}
	new->key = key;
	new->value = value;
	new->next = NULL;

	int hash = get_hash(key);
	if ((current = (*table)[hash]))
	{
		new->next = current;
	}

	(*table)[hash] = new;
}

/*
 * Získanie hodnoty z tabuľky.
 *
 * V prípade úspechu vráti funkcia ukazovateľ na hodnotu prvku, v opačnom
 * prípade hodnotu NULL.
 *
 * Pri implementácii využite funkciu ht_search.
 */
float *ht_get(ht_table_t *table, char *key) {
	ht_item_t *current = ht_search(table, key);
	return current ? &(current->value) : NULL;
}

/*
 * Zmazanie prvku z tabuľky.
 *
 * Funkcia korektne uvoľní všetky alokované zdroje priradené k danému prvku.
 * Pokiaľ prvok neexistuje, nerobte nič.
 *
 * Pri implementácii NEVYUŽÍVAJTE funkciu ht_search.
 */
void ht_delete(ht_table_t *table, char *key) {
	int hash = get_hash(key);
	ht_item_t *current = (*table)[hash];
	ht_item_t *previous = NULL, *next_item = NULL;
	for (; current; previous = current, current = current->next)
	{
		next_item = current->next;
		if (strcmp(key, current->key) == 0)
		{
			free(current);
			if (!previous)
			{
				(*table)[hash] = next_item; 
				return;
			}
			previous->next = next_item;
			return;
		}
	}
}

/*
 * Zmazanie všetkých prvkov z tabuľky.
 *
 * Funkcia korektne uvoľní všetky alokované zdroje a uvedie tabuľku do stavu po
 * inicializácii.
 */
void ht_delete_all(ht_table_t *table) {
	ht_item_t *current; 
	ht_item_t *delete;
	for (int i = 0; i < HT_SIZE; i++)
	{
		current = (*table)[i];
		while (current)
		{
			delete = current;
			current	 = current->next;
			free(delete);
		}
		(*table)[i] = NULL;
	}
}
