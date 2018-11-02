#ifndef _LIST_H_
#define _LIST_H_

#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <stdint.h>
#include <string.h>

////////////////////////////////////////////////////
// queue implemented with Doubly Link List
struct _CCLinkNode;
typedef struct _CCLinkNode {
    void *element;
    struct _CCLinkNode *prior;
    struct _CCLinkNode *next;
} CCLinkNode;

typedef struct {
    CCLinkNode *head;
    CCLinkNode *tail;
    CCLinkNode *cache;
    int num;
    unsigned int magic;
} CCLinkList;

void initList(CCLinkList *list);
void deinitList(CCLinkList *list);
int getListSize(CCLinkList *list);
int searchList(CCLinkList *list, void *item);
void* getListNextItem(CCLinkList *list, void* item); // return the element after *item; and return list head if item is NULL
void* getListPrevItem(CCLinkList *list, void *item);
int appendList(CCLinkList *list, void* item); // append an element at the end of the list
int removeList(CCLinkList *list, void* item); // remove item


#endif  /*_LIST_H_*/
