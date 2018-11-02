
#include "list.h"

#ifndef FREE_POINTER
#define FREE_POINTER(pointer)                                   \
    do {                                                        \
        if (pointer) {                                          \
            free(pointer);                                      \
            pointer = NULL;                                     \
        }                                                       \
    } while (0)
#endif /* FREE_POINTER */


#ifndef ZERO_FILL_POINTER
#define ZERO_FILL_POINTER(v)                                    \
    do {                                                        \
        (void)memset(v, 0, sizeof(*v));                         \
    } while (0)
#endif /* ZERO_FILL_POINTER */


#ifndef MALLOC_POINTER
#define MALLOC_POINTER(v)                                       \
    do {                                                        \
        v = (typeof(v))malloc(sizeof(*v));                      \
    } while (0)
#endif /* MALLOC_POINTER */

#ifndef ASSERT
#define ASSERT(v)                                       \
    do {                                                        \
                                                       \
    } while (0)
#endif /* ASSERT */  


CCLinkNode* _searchListHead(CCLinkNode *head, void *item)
{
    CCLinkNode *node = head;
    while ( node ) {
        if ( node->element == item ) {
            return node;
        }
        node = node->next;
    }
    return NULL;
}

int searchList(CCLinkList *list, void *item)
{
    if ( _searchListHead(list->head, item) ) {
        return 1;
    }
    else {
        return 0;
    }
}

#define MAGIC_LIST 0x8A9BCEDF
void initList(CCLinkList *list)
{
    ASSERT(list->magic != MAGIC_LIST); // to avoid double init
    ZERO_FILL_POINTER(list);
    list->magic = MAGIC_LIST;
    return;
}

void deinitList(CCLinkList *list)
{
    while ( list->num ) {
        //printf("num=%d,remove!\n", list->num);
        removeList(list, getListNextItem(list, NULL));
    }
    //printf("remove.list.done!\n");
    ZERO_FILL_POINTER(list);
    return;
}

int getListSize(CCLinkList *list)
{
    return list->num;
}

void* getListNextItem(CCLinkList *list, void *item)
{
    CCLinkNode *node, *_node;

    if ( !item ) { // item == NULL ====> get head
        node = list->head;
    }
    else {
        if ( list->cache && list->cache->element == item ) { // common case: cache hit
            node = list->cache->next;
        }
        else { // corner case: getListNextItem is called simultaneously
            _node = _searchListHead(list->head, item); // find reference
            if ( _node ) {
                node = _node->next;
            }
            else { // if reference node is deleted by someone else
                node = list->head;
            }
        }
    }

    list->cache = node;
    if ( node ) {
        //printf("query.return:%p(%p)\n", node, node->element);
        return node->element;
    }
    //printf("query.return:NULL\n");
    return NULL;
}

void* getListPrevItem(CCLinkList *list, void *item)
{
    CCLinkNode *node, *_node;

    if ( !item ) { // item == NULL ====> get tail
        node = list->tail;
    }
    else {
        if ( list->cache && list->cache->element == item ) { // common case: cache hit
            node = list->cache->prior;
        }
        else { // corner case: getListNextItem is called simultaneously
            _node = _searchListHead(list->head, item); // find reference
            if ( _node ) {
                node = _node->prior;
            }
            else { // if reference node is deleted by someone else
                node = list->tail;
            }
        }
    }

    list->cache = node;
    if ( node ) {
        //printf("query.return:%p(%p)\n", node, node->element);
        return node->element;
    }
    //printf("query.return:NULL\n");
    return NULL;
}

int appendList(CCLinkList *list, void *item)
{
    if ( !item ) {
        return -2;
    }

    CCLinkNode *node;
    node = _searchListHead(list->head, item);
    if ( node ) { // found: already exist
        return -1;
    }
    MALLOC_POINTER(node);
    ASSERT(node);
    ZERO_FILL_POINTER(node);

    if ( list->tail ) { // list has at least 1 element
        list->tail->next = node;
        node->prior = list->tail;
        node->next = NULL;
        list->tail = node;
    }
    else { // empty
        list->head = node;
        list->tail = node;
        node->prior = NULL;
        node->next = NULL;
    }
    node->element = item;
    //printf("append:%p(%p)\n", node, node->element);
    list->num++;
    return 0;
}

int removeList(CCLinkList *list, void *item)
{
    CCLinkNode *node;
    node = _searchListHead(list->head, item);
    if ( !node ) { // not found
        return -1;
    }
    if ( list->cache == node ) {
        list->cache = NULL;
    }
    if ( list->head != node ) { // previous item exits
        node->prior->next = node->next;
    }
    else { // node is head
        list->head = node->next; // may be NULL
    }

    if ( list->tail != node ) { // following item exits
        node->next->prior = node->prior;
    }
    else { // node is tail
        list->tail = node->prior; // may be NULL
    }
    FREE_POINTER(node);
    list->num--;
    //printf("after remove, head=%p, num=%d\n", list->head, list->num);
    return 0;
}

