# Welcome to the .NET nanoFramework nanoFramework.Tarantool repository

This repository contains the client library for working with [Tarantool](https://www.tarantool.io/en/) NoSQL database for .NET nanoFramework.

# Key features
 -  Getting closer to the most complete [IProto](https://www.tarantool.io/en/doc/latest/reference/internals/box_protocol/) protocol coverage.

# What can Tarantool for a microcontroller be useful for?

1. Getting various configuration parameters, settings, and data.
2. Transfer of complex calculations to more productive platforms (Tarantool).
3. Transfer of various data for subsequent storage, processing and aggregation.
4. Integration via Tarantool with various [relational databases](https://www.tarantool.io/en/doc/latest/reference/reference_rock/dbms/). Such as MySQL or PostgreSQL.
5. Integration via Tarantool with various Kafka and other products [through supported extension modules](https://www.tarantool.io/en/doc/latest/platform/app/rocksref/).

# Limitations

1. From the [list of client-server messages](https://www.tarantool.io/en/doc/latest/reference/internals/iproto/requests/), only IPROTO_CHUNK, IPROTO_NOP, and IPROTO_ID remained unrealized, since these protocol messages do not make much sense to a simple client.

2. We were trying to make API similar with tarantool lua API. So this connector is straightforward implementing of [IProto protocol](https://github.com/tarantool/tarantool/wiki/Binary-Protocol-v1.6/b9db62e848a0ec011416ffc53dcb2418467a0f0a?ysclid=m9x8u9airq740869903), some methods are not implemented yet because there are no direct analogs in IProto. Implementing some methods (like Pairs) does not make any sense, because it return a lua-iterator.
  
3. Methods not implemented:
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

   Almost all unrealized methods can be replaced by calling the Eval procedure. For some functions, you will need to connect a [user with the appropriate permissions](https://www.tarantool.io/en/doc/latest/admin/access_control/).

4. Since microcontrollers have a small amount of memory, and the size of the returned data can be large, the buffer for receiving responses from the Tarantool server must be limited. For these purposes, there is a corresponding parameter in the client settings.

# Usage
## What the client can do at the moment
### Box
<details>
  <summary>Samples</summary>
 
   1. Connect to the Tarantool server with the default user 'Guest':
   ```csharp
   ClientOptions clientOptions = new ClientOptions($"{TarantoolHostIp}:3301");
   using (var box = TarantoolContext.Connect(clientOptions))
   {
        //// .............
   }
   ```
   
   2. Connect to the Tarantool server with a special user, with a password transfer:
   ```csharp
    using (var box = TarantoolContext.Connect(TarantoolHostIp, 3301, "testuser", "test_password"))
    {
        //// .............
    }
   ```
   
   3. Get complete information about the schema, spaces, and indexes of the Tarantool database:
   ```csharp
   var clientOptions = new ClientOptions($"{TarantoolHostIp}:3301");
   clientOptions.ConnectionOptions.ReadSchemaOnConnect = true;
   clientOptions.ConnectionOptions.ReadBoxInfoOnConnect = true;
   
   using (var box = TarantoolContext.Connect(clientOptions))
   {
      //// box.Info;
      //// box.Schema;
      //// box.Schema.Spaces;
      //// box.Schema["_space"].Indices;
   }
   ```
   
   4. Calling various Tarantool Lua functions:
   ```csharp
   var clientOptions = new ClientOptions($"{TarantoolHostIp}:3301");
   clientOptions.ConnectionOptions.ReadSchemaOnConnect = false;
   clientOptions.ConnectionOptions.ReadBoxInfoOnConnect = false;
   clientOptions.ConnectionOptions.WriteStreamBufferSize = 512;
   clientOptions.ConnectionOptions.ReadStreamBufferSize = 512;
   
   using (var box = TarantoolContext.Connect(clientOptions))
   {
       var tupleParam = TarantoolTuple.Create(1.3d);
       var callResult = box.Call("math.sqrt", tupleParam, tupleParam.GetType());
   
       if (callResult != null && callResult.Data[0] != null)
       {
            var resultData = (TarantoolTuple)callResult.Data[0];
            Console.WriteLine($"The square root of the number {tupleParam[0]} is {resultData[0]}");
       }
   }
   ```
   
   5. Execute any correct Tarantool expressions:
   ```csharp
   var clientOptions = new ClientOptions($"{TarantoolHostIp}:3301");
   clientOptions.ConnectionOptions.ReadSchemaOnConnect = false;
   clientOptions.ConnectionOptions.ReadBoxInfoOnConnect = false;
   clientOptions.ConnectionOptions.WriteStreamBufferSize = 512;
   clientOptions.ConnectionOptions.ReadStreamBufferSize = 512;
   
   using (var box = TarantoolContext.Connect(clientOptions))
   {
      var evalResult = box.Eval("return box.space.bands:select{...}", typeof(TarantoolTuple[][]));
   
      if (evalResult != null && evalResult.Data[0] is not null)
      {
         var selectResult = (TarantoolTuple[])evalResult.Data[0];
         
         foreach (var tuple in selectResult)
         {
             Console.WriteLine(tuple.ToString());
         }
      }
   
      Console.WriteLine(new string('=', 10));
   
      evalResult = box.Eval("return box.space.bands:select({3}, {iterator='GT', limit = 3})");
   
      if (evalResult != null && evalResult.Data[0] is not null)
      {
         var arrayListResult = evalResult.Data[0] as ArrayList;
   
         foreach (IList tuple in arrayListResult)
         {
             Console.WriteLine($"[{tuple[0]}, {tuple[1]}, {tuple[2]}]");
         }
      }
   }
   ```
   
   6. Work with Tarantool-supported SQL:
   ```csharp
   var clientOptions = new ClientOptions($"testuser:test_password@{TarantoolHostIp}:3301");
   clientOptions.ConnectionOptions.ReadSchemaOnConnect = false;
   clientOptions.ConnectionOptions.ReadBoxInfoOnConnect = false;
   clientOptions.ConnectionOptions.WriteStreamBufferSize = 512;
   clientOptions.ConnectionOptions.ReadStreamBufferSize = 512;
   
   using (var box = TarantoolContext.Connect(clientOptions))
   {
        try
        {
            var executeSqlResult = box.ExecuteSql("create table sql_test(id int primary key, name text)");
            isTableCreate = true;
   
            executeSqlResult = box.ExecuteSql("insert into sql_test values (1, 'asdf'), (2, 'zxcv'), (3, 'qwer')");
      
            executeSqlResult = box.ExecuteSql("update sql_test set name = '1234' where id = 2");
      
            executeSqlResult = box.ExecuteSql("SELECT * FROM sql_test WHERE id = $1", typeof(TarantoolTuple[]), new SqlParameter(2, "$1"));
   
            if (evalResult != null && evalResult.Data[0] != null)
            {
                var resultData = (TarantoolTuple)executeSqlResult.Data[0];
                Console.WriteLine(resultData.ToString());
            }
        }
        finally
        {
            if (isTableCreate)
            {
                box.ExecuteSql("drop table sql_test");
            }
        }
   }
   ```
</details>

### Space
<details>
  <summary>Samples</summary>
 
   1. Insert tuple in space:
   ```csharp
     ClientOptions clientOptions = new ClientOptions($"{TarantoolHostIp}:3301");
     using (var box = TarantoolContext.Connect(clientOptions))
     {
          var space = box.Schema["bands"];
          var testTuple = TarantoolTuple.Create(15, "Black Sabbath", 1968);
          var responseData = space.Insert(testTuple);
          if (responseData != null && responseData.Data[0] != null)
          {
              var resultData = (TarantoolTuple)responseData.Data[0];
              Console.WriteLine(resultData.ToString());
          }
     }
   ```
   
   2. Getting and Select tuple from the space by key:
   ```csharp
      ClientOptions clientOptions = new ClientOptions($"{TarantoolHostIp}:3301");
      using (var box = TarantoolContext.Connect(clientOptions))
      {
           var space = box.Schema["bands"];
           var selectedTuple = space.GetTuple(TarantoolTuple.Create(15), TarantoolTupleType.Create(typeof(int), typeof(string), tupeof(uint));
           Console.WriteLine(selectedTuple.ToString());
           var responseData = space.Select(TarantoolTuple.Create(15));
           if (responseData != null && responseData.Data[0] != null)
           {
               var resultData = (TarantoolTuple)responseData.Data[0];
               Console.WriteLine(resultData.ToString());
           }
      }
   ```

  3. Put, Update, Replace tuple from the space:
   ```csharp
     ClientOptions clientOptions = new ClientOptions($"{TarantoolHostIp}:3301");
     using (var box = TarantoolContext.Connect(clientOptions))
     {
          var space = box.Schema["bands"];
          var testTuple = TarantoolTuple.Create(16, "Nazareth", 1988);
          var responseTuple = space.PutTuple(testTuple);
          Console.WriteLine(responseTuple.ToString());
          var responseData = space.Update(TarantoolTuple.Create(16), new UpdateOperation[] { UpdateOperation.CreateAssign(2, 1968) }, (TarantoolTupleType)testTuple.GetType());
          if (responseData != null && responseData.Data[0] != null)
          {
              var resultData = (TarantoolTuple)responseData.Data[0];
              Console.WriteLine(resultData.ToString());
          }
     }
   ```
     
  4. Upsert tuple from the space:
   ```csharp
     ClientOptions clientOptions = new ClientOptions($"{TarantoolHostIp}:3301");
     using (var box = TarantoolContext.Connect(clientOptions))
     {
          var space = box.Schema["bands"];
          var testTuple = TarantoolTuple.Create(17, "BonJovi", 1983);
          bool recordCreate = false;
          space.Upsert(testTuple, new UpdateOperation[] { UpdateOperation.CreateStringSlice(1, 3, 0, " ") });
          var selectedTuple = space.GetTuple(TarantoolTuple.Create(17), TarantoolTupleType.Create(typeof(int), typeof(string), tupeof(uint));
          Console.WriteLine(selectedTuple.ToString());
   
          space.Upsert(testTuple, new UpdateOperation[] { UpdateOperation.CreateStringSlice(1, 3, 0, " ") });
          selectedTuple = space.GetTuple(TarantoolTuple.Create(17), TarantoolTupleType.Create(typeof(int), typeof(string), tupeof(uint));
          Console.WriteLine(selectedTuple.ToString());
     }
   ```

   5. Delete tuple from the space:
   ```csharp
     ClientOptions clientOptions = new ClientOptions($"{TarantoolHostIp}:3301");
     using (var box = TarantoolContext.Connect(clientOptions))
     {
          var space = box.Schema["bands"];
          var deleteKeyTuple = TarantoolTuple.Create(17);
   
          var responseData = space.Delete(deleteKeyTuple);
          if (responseData != null && responseData.Data[0] != null)
          {
              var resultData = (TarantoolTuple)responseData.Data[0];
              Console.WriteLine(resultData.ToString());
          }
     }
   ```
</details>

### Index
<details>
  <summary>Samples</summary>
 
   1. Getting tuple by min index:
   ```csharp
      ClientOptions clientOptions = new ClientOptions($"{TarantoolHostIp}:3301");
      using (var box = TarantoolContext.Connect(clientOptions))
      {
            var index = box.Schema["bands"]["secondary"];
            var responseTupleType = TarantoolTupleType.Create(typeof(int), typeof(string), typeof(uint));
    
            var responseTuple = index.MinTuple(responseTupleType);
            Console.WriteLine(responseTuple.ToString());
      }
   ```

   3. Getting tuple by max index:
   ```csharp
      ClientOptions clientOptions = new ClientOptions($"{TarantoolHostIp}:3301");
      using (var box = TarantoolContext.Connect(clientOptions))
      {
            var index = box.Schema["bands"]["secondary"];
            var responseTupleType = TarantoolTupleType.Create(typeof(int), typeof(string), typeof(uint));
    
            var responseTuple = index.MaxTuple(responseTupleType);
            Console.WriteLine(responseTuple.ToString());
      }
   ```

   4. Select tuples:
   ```csharp
      ClientOptions clientOptions = new ClientOptions($"{TarantoolHostIp}:3301");
      using (var box = TarantoolContext.Connect(clientOptions))
      {
            var index = box.Schema["bands"]["secondary"];
            var responseTupleType = TarantoolTupleType.Create(typeof(int), typeof(string), typeof(uint));
    
            var responseData = index.Select(TarantoolTuple.Create("Scorpions"), new SelectOptions() { Iterator = Model.Enums.Iterator.Ge, Limit = 5}, responseTupleType);
            ////...........
      }
   ```

   4. Update tuple by index.

   5. Delete tuple by index.
</details>

An example of the direct implementation of interaction with Tarantool in a microcontroller can be seen in the corresponding repository of the [demo project](https://github.com/RelaxSpirit/nanoFramework.Tarantool/tree/master/Samples/WeatherTracker).

## Acknowledgements

The initial version of the MessagePack library was coded by [Spirin Dmitriy](https://github.com/RelaxSpirit), who has kindly handed over the library to the .NET **nanoFramework** project.

## Feedback and documentation

For documentation, providing feedback, issues, and finding out how to contribute, please refer to the [Home repo](https://github.com/nanoframework/Home).

Join our Discord community [here](https://discord.gg/gCyBu8T).

## Credits

The list of contributors to this project can be found at [CONTRIBUTORS](https://github.com/nanoframework/Home/blob/main/CONTRIBUTORS.md).

## License

The **nanoFramework** WebServer library is licensed under the [MIT license](LICENSE.md).

## Code of Conduct

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behaviour in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

## .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).
