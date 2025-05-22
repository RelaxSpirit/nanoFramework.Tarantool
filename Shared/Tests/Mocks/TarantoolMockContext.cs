// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses _writer file to you under the MIT license.

using System.Collections;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Converters;
using nanoFramework.Tarantool.Converters;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Tests.Mocks.Converters;
using nanoFramework.Tarantool.Tests.Mocks.Data;

namespace nanoFramework.Tarantool.Tests.Mocks
{
    internal class TarantoolMockContext
    {
        internal const string TarantoolHelloString = "Tarantool 3.4.0 (Binary) 790215f6-c2d8-427c-9bb9-687f71c7e18a  \nVcSniiMfX+m5JH+a+WSffyJjkjHYO4Ku+d6afsjIT68=                    ";
        internal const string AdminUserName = "testuser";
        internal const string AdminPassword = "test_password";
        internal static readonly RequestHeaderConverterMock RequestHeaderConverter = new RequestHeaderConverterMock();
        internal static readonly AuthenticationPacketConverterMock AuthenticationPacketConverter = new AuthenticationPacketConverterMock();
        internal static readonly SelectPacketConverterMock SelectPacketConverter = new SelectPacketConverterMock();
        internal static readonly CallPacketConverterMock CallPacketConverter = new CallPacketConverterMock();
        internal static readonly EvalPacketConverterMock EvalPacketConverter = new EvalPacketConverterMock();
        internal static readonly ExecuteSqlRequestConverterMock ExecuteSqlRequestConverter = new ExecuteSqlRequestConverterMock();
        internal static readonly PingPacketConverter PingPacketConverter = new PingPacketConverter();
        internal static readonly InsertReplacePacketConverterMock InsertPacketConverter = new InsertReplacePacketConverterMock(false);
        internal static readonly InsertReplacePacketConverterMock ReplacePacketConverter = new InsertReplacePacketConverterMock(true);
        internal static readonly DeletePacketConverterMock DeletePacketConverter = new DeletePacketConverterMock();
        internal static readonly UpdatePacketConverterMock UpdatePacketConverter = new UpdatePacketConverterMock();

        private static object _lockInstance = new object();
#nullable enable
        private static TarantoolMockContext? _instance;
#nullable disable

        private TarantoolMockContext()
        {
            ConverterContext.Add(typeof(BoxInfoMock), new BoxInfoMock.BoxInfoConverterMock());
            ConverterContext.Add(typeof(SpaceMock), new SpaceConverterMock());
            ConverterContext.Add(typeof(SpaceMock[]), new SimpleArrayConverter(typeof(SpaceMock)));
            ConverterContext.Add(typeof(SpaceFieldMock), new SpaceFieldConverterMock());
            ConverterContext.Add(typeof(SpaceFieldMock[]), new SimpleArrayConverter(typeof(SpaceFieldMock)));
            ConverterContext.Add(typeof(IndexPartMock), new IndexPartConverterMock());
            ConverterContext.Add(typeof(IndexPartMock[]), new SimpleArrayConverter(typeof(IndexPartMock)));
            ConverterContext.Add(typeof(IndexCreationOptionsMock), new IndexCreationOptionsConverterMock());
            ConverterContext.Add(typeof(IndexMock), new IndexConverterMock());
            ConverterContext.Add(typeof(IndexMock[]), new SimpleArrayConverter(typeof(IndexMock)));
            ConverterContext.Add(typeof(DataResponseMock), new ResponsePacketConverterMock());
            ConverterContext.Add(typeof(EmptyResponseMock), new EmptyResponseConverterMock());
        }

        public static TarantoolMockContext Instanse
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockInstance)
                    {
                        if (_instance == null)
                        {
                            _instance = new TarantoolMockContext();
                        }
                    }
                }

                return _instance;
            }
        }

        internal TarantoolTuple[] TestTable { get; } = new TarantoolTuple[]
        {
                TarantoolTuple.Create(1, "Roxette", 1986),
                TarantoolTuple.Create(2, "Scorpions", 1965),
                TarantoolTuple.Create(3, "Ace of Base", 1987),
                TarantoolTuple.Create(4, "Kino", 1981),
                TarantoolTuple.Create(5, "Metallica", 1981),
                TarantoolTuple.Create(6, "Rammstein", 1994),
                TarantoolTuple.Create(7, "The Beatles", 1960),
                TarantoolTuple.Create(8, "Pink Floyd", 1965),
                TarantoolTuple.Create(9, "The Rolling Stones", 1962),
                TarantoolTuple.Create(10, "The Doors", 1965),
                TarantoolTuple.Create(11, "Nirvana", 1987),
                TarantoolTuple.Create(12, "Led Zeppelin", 1968),
                TarantoolTuple.Create(13, "Queen", 1970),
                TarantoolTuple.Create(14, "Sector Gaza", 1987)
        };

        internal Hashtable ModifyTable { get; } = new Hashtable();

        internal BoxInfoMock TestBoxInfo { get; } = BoxInfoMock.GetBoxInfo();

        internal SpaceMock[] Spaces { get; } = new SpaceMock[]
        {
            new SpaceMock(1, 1, "_space", Model.Enums.StorageEngine.Memtx, new SpaceField[] { new SpaceFieldMock("test", Model.Enums.FieldType.Num) }),
            new SpaceMock(2, 3, "bands", Model.Enums.StorageEngine.Vinyl, new SpaceField[] { new SpaceFieldMock("id", Model.Enums.FieldType.Unsigned), new SpaceFieldMock("band_name", Model.Enums.FieldType.String), new SpaceFieldMock("year", Model.Enums.FieldType.Unsigned) })
        };

        internal IndexMock[] Indices { get; } = new IndexMock[]
        {
            new IndexMock(0, 1, "main_index", true, Model.Enums.IndexType.RTree, new IndexPart[] { new IndexPartMock(1, Model.Enums.FieldType.Unsigned) }),
            new IndexMock(0, 2, "primary", true, Model.Enums.IndexType.Tree, new IndexPart[] { new IndexPartMock(1, Model.Enums.FieldType.Unsigned), new IndexPartMock(2, Model.Enums.FieldType.String) }),
            new IndexMock(1, 2, "secondary", true, Model.Enums.IndexType.Tree, new IndexPart[] { new IndexPartMock(2, Model.Enums.FieldType.String) })
        };

        internal TarantoolStreamMock GetTarantoolStreamMock(ClientOptions clientOptions)
        {
            return new TarantoolStreamMock(clientOptions);
        }
    }
}
