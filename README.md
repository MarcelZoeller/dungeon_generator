# Dungeon Generator

Here is a very simplified description of how it works.

1. The start room is created.
2. A main branch is created. This branch can be empty, contain a mini boss, a key item or other. It is random which room contains the item.
3. Next a locked room with a higher key level is created.
4. A sub branch is created at a random place. If the generator has already created the key item, it can also be locked with the key item.
5. Points 2 to 4 are repeated until all branches are created. The last point always contains the boss room and is locked with the boss key.
