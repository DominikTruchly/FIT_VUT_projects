##!/usr/bin/env python3

def first_with_given_key(iterable, key=lambda y: y):
    list = []
    for i in iterable:
        if key(i) not in list:
            yield i
            list.append(key(i))
        
def dupl_with_given_key(iterable, key=lambda x: x):
    lst = []
    for item in iterable:
        if key(item) in lst:
            yield item
        else:
            lst.append(key(item))

print(tuple(first_with_given_key([[1],[2,3],[4],[5,6,7],[8,9]], key = len)))