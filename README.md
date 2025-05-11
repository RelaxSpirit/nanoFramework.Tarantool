# Welcome to the .NET nanoFramework nanoFramework.Tarantool repository

This repository contains the client library for working with [Tarantool](https://www.tarantool.io/en/) NoSQL database for .NET nanoFramework.

# Key features
 -  Getting closer to the most complete [IProto](https://tarantool.org/doc/dev_guide/box-protocol.html) protocol coverage

# Limitations

We were trying to make API similar with tarantool lua API. So this connector is straightforward implementing of [IProto protocol](https://github.com/tarantool/tarantool/wiki/Binary-Protocol-v1.6/b9db62e848a0ec011416ffc53dcb2418467a0f0a?ysclid=m9x8u9airq740869903). Some methods are not implemented yet because there are no direct analogs in IProto. Implementing some methods (like Pairs) does not make any sense, because it return a lua-iterator.

Methods not implemented:

* Index methods:
    1. Pairs
    2. Count
    3. Alter
    4. Drop
    5. Rename
    6. BSize
    7. Alter
* Schema methods
    1. CreateSpace
* Space methods
    1. CreateIndex
    2. Drop
    3. Rename
    4. Count
    5. Lengh
    6. Increment
    7. Decrement
    8. AutoIncrement
    9. Pairs
