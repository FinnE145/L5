# L5
L5, or Low Level Looping Linear Language, is a simple optionally-typed, non-object-oriented language written in C# that compiles to C# IL.
<hr>
It is centrally featured around working with a virtual memory strip, which can be accessed by index (called a pointer) or through ranges of indexes (called a range). Each index is 1 byte, but individual bits can be accessed, and overflow is handled manually, but syntactic sugar is present to help.
<hr>

## Typing

Strict typing can be turned on or off for each program, meaning that if it is off (default), data is read off of memory as-is, and can be interpreted in numerous ways (but could lead to undexpected results). Eg:

**Non-typed:**
```
@0 = 'a'            # assign the character a to index 0
@1 = int @0         # read from index 0 as an int, and assign to index 1
return @1 to out    #> 65

# Explanation: 'a' is stored as 01000001 in memory, which when read as a decimal integer, is the number 65
```
**Typed:**
```
TYPED
chr @0 = 'a'        # assign the character a to index 0, specifying that pointer 0 has a chr datatype
int @1 = int @0     #! TypeError: Attempted to read chr pointer as int
```
In addition, a default type can be set for non-typed programs. If it is not set, the language will remember the type of that data but not enforce it. Eg:

**No default type (type memory):**
```
@0 = 'a'            # assign the character a to index 0
@1 = @0             # read from index 0 without specifying a type, and assign to index 1
return @1 to out    #> `a`
```
**Default type of bool:**
```
DEFTYPE bool
@0 = 'a'            # assign the character a to index 0
@1 = @0             # read from index 0 as the default type, and assign to index 1
return @1 to out    #> T

# Explanation: 'a' is stored as 01000001 in memory, which when read as a bool is `T` (true) because the 1s bit is true (@0.0 == 1) (bitwise assignment and recall can be used to save memory on bools).
```
