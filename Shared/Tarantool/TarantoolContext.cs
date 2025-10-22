// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Text;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Exceptions;
using nanoFramework.Tarantool.Client;
using nanoFramework.Tarantool.Client.Interfaces;
using nanoFramework.Tarantool.Converters;
using nanoFramework.Tarantool.Dto;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Headers;
using nanoFramework.Tarantool.Model.Requests;
using nanoFramework.Tarantool.Model.Responses;
using nanoFramework.Tarantool.Model.UpdateOperations;
using Index = nanoFramework.Tarantool.Client.Index;

namespace nanoFramework.Tarantool
{
    /// <summary>
    /// <see cref="Tarantool"/> context.
    /// </summary>
    public class TarantoolContext
    {        
        private static readonly object LockInstance;
        private static readonly InsertReplacePacketConverter InsertReplacePacketConverter;
#nullable enable
        private static TarantoolContext? _instance = null;
#nullable disable
        private readonly Hashtable _tarantoolConvertersHashtable = new Hashtable();
        private readonly Hashtable _tarantoolTupleTypeHashtable = new Hashtable();
        private readonly Hashtable _tarantoolTupleArrayTypeHashtable = new Hashtable();
        private readonly object _converterContextTupleTypeLock = new object();
        private readonly object _converterContextTupleArrayTypeLock = new object();

        static TarantoolContext()
        {
            LockInstance = new object();
            InsertReplacePacketConverter = new InsertReplacePacketConverter();
        }
        
        /// <summary>
        /// Prevents a default instance of the <see cref="TarantoolContext" /> class from being created.
        /// </summary>
        private TarantoolContext()
        {
            ConverterContext.Add(typeof(PingRequest), new PingPacketConverter());
            ConverterContext.Add(typeof(AuthenticationRequest), new AuthenticationPacketConverter());
            ConverterContext.Add(typeof(BoxInfo), new BoxInfo.BoxInfoConverter());
            ConverterContext.Add(typeof(CallRequest), new CallPacketConverter());
            ConverterContext.Add(typeof(DeleteRequest), new DeletePacketConverter());
            ConverterContext.Add(typeof(EmptyResponse), new EmptyResponseConverter());
            ConverterContext.Add(typeof(ErrorResponse), new ErrorResponsePacketConverter());
            ConverterContext.Add(typeof(EvalRequest), new EvalPacketConverter());
            ConverterContext.Add(typeof(ExecuteSqlRequest), new ExecuteSqlRequestConverter());

            FieldTypeConverter = new FieldTypeConverter();
            ConverterContext.Add(typeof(FieldType), FieldTypeConverter);

            ConverterContext.Add(typeof(Index), new IndexConverter());

            IndexOptionsConverter = new IndexCreationOptionsConverter();
            ConverterContext.Add(typeof(IndexCreationOptions), IndexOptionsConverter);

            IndexPartConverter = new IndexPartConverter();
            ConverterContext.Add(typeof(IndexPart), IndexPartConverter);

            IndexPartsConverter = new SimpleArrayConverter(typeof(IndexPart));
            ConverterContext.Add(typeof(IndexPart[]), IndexPartsConverter);
            
            IndexTypeConverter = new IndexTypeConverter();
            ConverterContext.Add(typeof(IndexType), IndexTypeConverter);

            ConverterContext.Add(typeof(InsertReplaceRequest), InsertReplacePacketConverter);
            ConverterContext.Add(typeof(InsertRequest), InsertReplacePacketConverter);
            ConverterContext.Add(typeof(ReplaceRequest), InsertReplacePacketConverter);
            ConverterContext.Add(typeof(PacketSize), new PacketSizeConverter());
            ConverterContext.Add(typeof(RequestHeader), new RequestHeaderConverter());

            RequestIdConverter = new RequestIdConverter();
            ConverterContext.Add(typeof(RequestId), RequestIdConverter);

            ConverterContext.Add(typeof(ResponseHeader), new ResponseHeaderConverter());
            ConverterContext.Add(typeof(SelectRequest), new SelectPacketConverter());
            ConverterContext.Add(typeof(Space), new SpaceConverter());
            ConverterContext.Add(typeof(SpaceField), new SpaceFieldConverter());

            StorageEngineConverter = new StorageEngineConverter();
            ConverterContext.Add(typeof(StorageEngine), StorageEngineConverter);

            ConverterContext.Add(typeof(StringSpliceOperation), new StringSpliceOperationConverter());
            ConverterContext.Add(typeof(UpdateOperation), new UpdateOperationConverter());
            ConverterContext.Add(typeof(UpdateRequest), new UpdatePacketConverter());
            ConverterContext.Add(typeof(UpsertRequest), new UpsertPacketConverter());
            ConverterContext.Add(typeof(TarantoolTuple), new TarantoolTupleConverter());
            ConverterContext.Add(typeof(SqlInfoResponse), new SqlInfoResponsePacketConverter());
            ConverterContext.Add(typeof(DataResponse), new ResponsePacketConverter(typeof(ArrayList)));
            ConverterContext.Add(typeof(Space[]), new SimpleArrayConverter(typeof(Space)));
            ConverterContext.Add(typeof(Index[]), new SimpleArrayConverter(typeof(Index)));

            SpaceFieldsConverter = new SimpleArrayConverter(typeof(SpaceField));
            ConverterContext.Add(typeof(SpaceField[]), SpaceFieldsConverter);

            ConverterContext.Add(typeof(TarantoolTuple[][]), new SimpleArrayConverter(typeof(TarantoolTuple[])));
            ConverterContext.Add(typeof(TarantoolTuple[]), new SimpleArrayConverter(typeof(TarantoolTuple)));
            ConverterContext.Add(typeof(InsertRequest.AutoIncrementKey), new AutoIncrementKeyConverter());
        }

        /// <summary>
        /// Gets <see cref="TarantoolContext"/> instance.
        /// </summary>
        public static TarantoolContext Instance 
        {
            get
            {
                if (LockInstance == null)
                {
                    _instance = new TarantoolContext();
                }

                if (_instance == null)
                {
                    lock (LockInstance)
                    {
                        if (_instance == null)
                        {
                            _instance = new TarantoolContext();
                        }
                    }
                }

                return _instance;
            }
        }

        #region Converters
        internal IConverter UintConverter { get; } = ConverterContext.GetConverter(typeof(uint));

        internal IConverter IntConverter { get; } = ConverterContext.GetConverter(typeof(int));

        internal IConverter BytesConverter { get; } = ConverterContext.GetConverter(typeof(byte[]));

        internal IConverter StringConverter { get; }  = ConverterContext.GetConverter(typeof(string));

        internal IConverter LongConverter { get; } = ConverterContext.GetConverter(typeof(long));

        internal IConverter UlongConverter { get; } = ConverterContext.GetConverter(typeof(ulong));

        internal IConverter BoolConverter { get; } = ConverterContext.GetConverter(typeof(bool));

        internal IConverter ArrayListConverter { get; } = ConverterContext.GetConverter(typeof(ArrayList));

        internal IConverter IndexTypeConverter { get; }

        internal IConverter IndexOptionsConverter { get; }

        internal IConverter IndexPartsConverter { get; }

        internal IConverter IndexPartConverter { get; }

        internal IConverter FieldTypeConverter { get; }

        internal IConverter RequestIdConverter { get; }

        internal IConverter SpaceFieldsConverter { get; }

        internal IConverter StorageEngineConverter { get; }
        #endregion

        internal static string GetTypeArrayTypesFullName(Type[] tupleItemsTypes)
        {
            if (tupleItemsTypes.Length < 1)
            {
                return typeof(TarantoolTuple).FullName;
            }

            StringBuilder sbFullName = new StringBuilder();

            foreach (Type type in tupleItemsTypes)
            {
                sbFullName.Append(type.FullName);
                sbFullName.Append(',');
            }

            sbFullName.Remove(sbFullName.Length - 1, 1);

            return sbFullName.ToString();
        }

        /// <summary>
        /// Connect to <see cref="Tarantool"/> 
        /// </summary>
        /// <param name="clientOptions">Client options fo connect.</param>
        /// <returns><see cref="Tarantool"/> <see cref="IBox"/> interface.</returns>
        /// <exception cref="NotSupportedException">If <see cref="Tarantool"/> context not initialize.</exception>
        public static IBox Connect(ClientOptions clientOptions)
        {
            if (Instance != null)
            {
                return Box.Connect(clientOptions);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Connect to <see cref="Tarantool"/> 
        /// </summary>
        /// <param name="replicationSource"><see cref="Tarantool"/> replica connection string.</param>
        /// <returns><see cref="Tarantool"/> <see cref="IBox"/> interface.</returns>
        /// <exception cref="NotSupportedException">If <see cref="Tarantool"/> context not initialize.</exception>
        public static IBox Connect(string replicationSource)
        {
            if (Instance != null)
            {
                return Box.Connect(replicationSource);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Connect to <see cref="Tarantool"/> as guest.
        /// </summary>
        /// <param name="host"><see cref="Tarantool"/> instance network host name.</param>
        /// <param name="port"><see cref="Tarantool"/> instance network port number.</param>
        /// <returns><see cref="Tarantool"/> <see cref="IBox"/> interface.</returns>
        /// <exception cref="NotSupportedException">If <see cref="Tarantool"/> context not initialize.</exception>
        public static IBox Connect(string host, int port)
        {
            if (Instance != null)
            {
                return Box.Connect(host, port);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Connect to <see cref="Tarantool"/> as specific user.
        /// </summary>
        /// <param name="host"><see cref="Tarantool"/> instance network host name.</param>
        /// <param name="port"><see cref="Tarantool"/> instance network port number.</param>
        /// <param name="user">User name.</param>
        /// <param name="password">User password.</param>
        /// <returns><see cref="Tarantool"/> <see cref="IBox"/> interface.</returns>
        /// <exception cref="NotSupportedException">If <see cref="Tarantool"/> context not initialize.</exception>
        public static IBox Connect(string host, int port, string user, string password)
        {
            if (Instance != null)
            {
                return Box.Connect(host, port, user, password);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Create <see cref="Tarantool"/> tuple type.
        /// </summary>
        /// <param name="tupleItemsTypes">Tuple items types.</param>
        /// <returns>Instance of the <see cref="TarantoolTupleType"/> class.</returns>
        public TarantoolTupleType GetTarantoolTupleType(params Type[] tupleItemsTypes)
        {
            var typesFullName = GetTypeArrayTypesFullName(tupleItemsTypes);
            var tarantoolTupleType = _tarantoolTupleTypeHashtable[typesFullName];

            if (tarantoolTupleType == null)
            {
                lock (_tarantoolTupleTypeHashtable.SyncRoot)
                {
                    tarantoolTupleType = _tarantoolTupleTypeHashtable[typesFullName];

                    if (tarantoolTupleType == null)
                    {
                        tarantoolTupleType = new TarantoolTupleType(tupleItemsTypes);
                        _tarantoolTupleTypeHashtable.Add(typesFullName, tarantoolTupleType);
                    }
                }
            }

            return (TarantoolTupleType)tarantoolTupleType;
        }

        /// <summary>
        /// Create <see cref="Tarantool"/> tuple array type.
        /// </summary>
        /// <param name="arrayElementType"><see cref="Tarantool"/> tuple type.</param>
        /// <returns>Instance of the <see cref="TarantoolTupleArrayType"/> class.</returns>
        public TarantoolTupleArrayType GetTarantoolTupleArrayType(TarantoolTupleType arrayElementType)
        {
            var elementTypeFullName = arrayElementType.FullName;
            var tarantoolTupleArrayType = _tarantoolTupleArrayTypeHashtable[elementTypeFullName];
            if (tarantoolTupleArrayType == null)
            {
                lock (_tarantoolTupleArrayTypeHashtable.SyncRoot)
                {
                    tarantoolTupleArrayType = _tarantoolTupleArrayTypeHashtable[elementTypeFullName];
                    if (tarantoolTupleArrayType == null)
                    {
                        tarantoolTupleArrayType = new TarantoolTupleArrayType(arrayElementType);
                        _tarantoolTupleArrayTypeHashtable[elementTypeFullName] = tarantoolTupleArrayType;
                    }
                }
            }

            return (TarantoolTupleArrayType)tarantoolTupleArrayType;
        }

        internal IConverter GetTarantoolTupleConverter(TarantoolTuple tarantoolTuple)
        {
            var tupleType = tarantoolTuple.GetType();
            return GetTarantoolTupleConverter((TarantoolTupleType)tupleType);
        }

        internal IConverter GetTarantoolTupleConverter(TarantoolTupleType tarantoolTupleType)
        {
            var converter = ConverterContext.GetConverter(tarantoolTupleType);
            if (converter == null)
            {
                lock (_converterContextTupleTypeLock)
                {
                    converter = ConverterContext.GetConverter(tarantoolTupleType);

                    if (converter == null)
                    {
                        converter = new TarantoolTupleConverter(tarantoolTupleType.TupleTypes);
                        ConverterContext.Add(tarantoolTupleType, converter);
                    }
                }
            }

            return converter;
        }

        internal IConverter GetDataResponseDataTypeConverter(Type dataType)
        {
            var converter = _tarantoolConvertersHashtable[dataType.Name];

            if (converter == null)
            {
                lock (_tarantoolConvertersHashtable.SyncRoot)
                {
                    converter = _tarantoolConvertersHashtable[dataType.Name];

                    if (converter == null)
                    {
                        converter = GetResponsePacketConverter(dataType);
                        _tarantoolConvertersHashtable.Add(dataType.Name, converter);
                    }
                }
            }

            return (IConverter)converter;
        }

        private ResponsePacketConverter GetResponsePacketConverter(Type dataType)
        {
            if (dataType is TarantoolTupleType tarantoolTupleType)
            {
                if (GetTarantoolTupleConverter(tarantoolTupleType) == null)
                {
                    throw new ConverterNotFoundException(tarantoolTupleType);
                }

                return new ResponsePacketConverter(tarantoolTupleType);
            }
            else
            {
                if (dataType is TarantoolTupleArrayType tarantoolTupleArrayType)
                {
                    var elementType = (TarantoolTupleType)tarantoolTupleArrayType.GetElementType();

                    if (GetTarantoolTupleConverter(elementType) == null)
                    {
                        throw new ConverterNotFoundException(elementType);
                    }

                    if (GetTarantoolArrayTupleConverter(tarantoolTupleArrayType) == null)
                    {
                        throw new ConverterNotFoundException(tarantoolTupleArrayType);
                    }

                    return new ResponsePacketConverter(tarantoolTupleArrayType);
                }
                else
                {
                    if (ConverterContext.GetConverter(dataType) == null)
                    {
                        throw new ConverterNotFoundException(dataType);
                    }

                    return new ResponsePacketConverter(dataType);
                }
            }
        }

        private IConverter GetTarantoolArrayTupleConverter(TarantoolTupleArrayType tarantoolTupleArrayType)
        {
            var converter = ConverterContext.GetConverter(tarantoolTupleArrayType);
            if (converter == null)
            {
                lock (_converterContextTupleArrayTypeLock)
                {
                    converter = ConverterContext.GetConverter(tarantoolTupleArrayType);
                    if (converter == null)
                    {
                        converter = new TarantoolTupleArrayConverter((TarantoolTupleType)tarantoolTupleArrayType.GetElementType());
                        ConverterContext.Add(tarantoolTupleArrayType, converter);
                    }
                }
            }

            return converter;
        }
    }
}
