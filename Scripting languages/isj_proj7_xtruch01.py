#!/usr/bin/env python3
import collections

def log_and_count(**kwargs2):
    def decorator(function):
        def function_inside(*args, **kwargs):
            if  "key" in kwargs2:
                kwargs2["counts"][kwargs2["key"]] += 1
            print("called", function.__name__, "with", args, "and", kwargs)
        return function_inside
    return decorator

my_counter = collections.Counter()
