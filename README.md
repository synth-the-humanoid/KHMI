Work in Progress Modding API for Kingdom Hearts Final Mix.


After including the .dll in your project, use the MemoryInterface class to edit game memory. Alternatively, use CodeInterface to inject assembly code. offsets.csv stores all the offsets used.

offsets.csv format:

OffsetName,<provider><version>,0x01020304,<provider><version>,0x05060708


each new line in offsets.csv is a new offset. valid providers currently are "epic" or "steam". going to finish the offsets for "epic1.0.0.9" before anything else.

"--" or "//" at the start of a line in offsets.csv will denote a comment
