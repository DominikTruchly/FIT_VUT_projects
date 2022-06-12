Program that will implement basic mathematical operations on sets and binary relations. 
The input of the program will be text data representing sets and sessions and assignment of operations. 
The program will print the result of processing on standard output.


Translation and execution:

$ gcc -std = c99 -Wall -Wextra -Werror setcal.c -o setcal
$ ./setcal FILE


Input file format:
-The text file consists of three consecutive parts:
1. Definition of the universe - one line starting with "U" and continuing with space separated elements,
2. Definition of sets and relations - one or more lines starting with "S" or "R" and continuing with space separated elements (a line starting with "S" indicates the definition of the set,
   "R" is used to define the session),
3. Commands over sets and relations - one or more lines starting with "C" and continuing with the command identifier.

-Universe:
 The elements of the universe can be strings containing lowercase and uppercase letters of the English alphabet. The length of the string is a maximum of 30 characters. Universe elements cannot contain identifiers
 commands (see below) and the keywords true and false. All elements in sets and relations must belong to the universe. Example:
 Example: Apple Lemon Orange Banana Peach

-Sets
 Each set is defined on one line by space-separated elements from the universe. The set identifier is the line number on which the set is defined (given that at
 the first line of the file is the universe, so the set identifiers start with the number 2). Set identifiers are used in operations (see below). Example set definition:
 Example: S Apple Banana Peach

-Relation:
 Each session is defined by an enumeration of pairs. The pair is enclosed in parentheses, the first and second elements of the pair are separated by a space. Each pair is separated by a space. Example:
 Example: R (Apple Banana) (Apple Peach) (Apple Apple)

-Commands:
 Each command is defined on one line, starting with the command identifier, and the command arguments are separated by a space (from the identifier and each other). The arguments of the command are
 numeric identifiers of sets and relations (positive integers, number 1 identifies the universe set). Example:
 Example: C minus 1 2

-Commands over sets:
 The command works on sets and its result is either a set (in this case it prints the set in the same format as expected in the input file, ie it starts with "S" and continues
 space separated by elements) or results in a truth value (in which case prints true or false on a separate line) or results in a natural number (which prints
 on a separate line).


 empty A - prints true or false depending on whether the set defined on line A is empty or non-empty.
card A - prints the number of elements in the set A (defined on line A).
complement A - prints the complement of set A.
union A B - prints the unification of sets A and B.
intersect A B - prints the intersection of sets A and B.
minus A B - prints the difference of sets A \ B.
subseteq A B - prints true or false depending on whether set A is a subset of set B.
subset A B - prints true or false if set A is a proper subset of set B.
equals A B - prints true or false if sets are equal.
Relationship commands
The command works on sessions and results in either a truth value (prints true or false) or a set (prints a set in the format as in the input file).

reflexive R - prints true or false if the session is reflexive.
symmetric R - prints true or false if the session is symmetric.
antisymmetric R - prints true or false if the session is antisymmetric.
transitive R - prints true or false if the session is transitive.
function R - prints true or false if the R session is a function.
domain R - prints the domain of the R function (can also be applied to relations - the first elements of pairs).
codomain R - prints the range of values ​​of the R function (can also be applied to relations - second elements of pairs).
injective R A B - prints true or false if the R function is injective. A and B are sets; a∈A, b∈B, (a, b) ∈R.
surjective R A B - prints true or false if the R function is surjective. A and B are sets; a∈A, b∈B, (a, b) ∈R.
bijective R A B - prints true or false if the R function is bijective. A and B are sets; a∈A, b∈B, (a, b) ∈R.

----------

-Implementation details:
 The maximum number of rows supported is 1000.
 The order of the elements in the set and in the output session does not matter.
 All elements of sets and in relations must belong to the universe. If an element in a set or pair repeats in a session, this is an error.