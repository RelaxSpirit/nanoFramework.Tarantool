// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
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
        private static readonly object LockInstance = new object();
#nullable enable
        private static TarantoolContext? _instance = null;
#nullable disable
        private readonly Hashtable _convertersHashtable = new Hashtable();
        private readonly Hashtable _tarantoolTupleTypeHashtable = new Hashtable();
        private readonly object _lock = new object();
        private readonly object _arrayLock = new object();
        private readonly object _convertersHashtableLock = new object();
        private readonly object _tarantoolTupleTypeHashtableLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="TarantoolContext"/> class.
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
            ConverterContext.Add(typeof(FieldType), new FieldTypeConverter());
            ConverterContext.Add(typeof(Index), new IndexConverter());
            ConverterContext.Add(typeof(IndexCreationOptions), new IndexCreationOptionsConverter());
            ConverterContext.Add(typeof(IndexPart), new IndexPartConverter());
            ConverterContext.Add(typeof(IndexPart[]), new SimpleArrayConverter(typeof(IndexPart)));
            ConverterContext.Add(typeof(IndexType), new IndexTypeConverter());
            ConverterContext.Add(typeof(InsertReplaceRequest), new InsertReplacePacketConverter());
            ConverterContext.Add(typeof(PacketSize), new PacketSizeConverter());
            ConverterContext.Add(typeof(RequestHeader), new RequestHeaderConverter());
            ConverterContext.Add(typeof(RequestId), new RequestIdConverter());
            ConverterContext.Add(typeof(ResponseHeader), new ResponseHeaderConverter());
            ConverterContext.Add(typeof(SelectRequest), new SelectPacketConverter());
            ConverterContext.Add(typeof(Space), new SpaceConverter());
            ConverterContext.Add(typeof(SpaceField), new SpaceFieldConverter());
            ConverterContext.Add(typeof(StorageEngine), new StorageEngineConverter());
            ConverterContext.Add(typeof(StringSpliceOperation), new StringSpliceOperationConverter());
            ConverterContext.Add(typeof(UpdateOperation), new UpdateOperationConverter());
            ConverterContext.Add(typeof(UpdateRequest), new UpdatePacketConverter());
            ConverterContext.Add(typeof(UpsertRequest), new UpsertPacketConverter());
            ConverterContext.Add(typeof(TarantoolTuple), new TarantoolTupleConverter());
            ConverterContext.Add(typeof(SqlInfoResponse), new SqlInfoResponsePacketConverter());
            ConverterContext.Add(typeof(DataResponse), new ResponsePacketConverter(typeof(ArrayList)));
            ConverterContext.Add(typeof(Space[]), new SimpleArrayConverter(typeof(Space)));
            ConverterContext.Add(typeof(Index[]), new SimpleArrayConverter(typeof(Index)));
            ConverterContext.Add(typeof(SpaceField[]), new SimpleArrayConverter(typeof(SpaceField)));
            ConverterContext.Add(typeof(TarantoolTuple[][]), new SimpleArrayConverter(typeof(TarantoolTuple[])));
            ConverterContext.Add(typeof(TarantoolTuple[]), new SimpleArrayConverter(typeof(TarantoolTuple)));
        }

        /// <summary>
        /// Gets <see cref="TarantoolContext"/> instance.
        /// </summary>
        public static TarantoolContext Instance 
        {
            get
            {
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

        internal static string GetTypeArrayTypesFullName(Type[] tupleItemsTypes)
        {
            if (tupleItemsTypes.Length < 1)
            {
                return typeof(Type[]).FullName ?? typeof(Type[]).Name;
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

        internal IConverter GetTarantoolTupleConverter(TarantoolTuple tarantoolTuple)
        {
            var tupleType = tarantoolTuple.GetType();
            return GetTarantoolTupleConverter((TarantoolTupleType)tupleType);
        }

        internal IConverter GetDataResponseDataTypeConverter(Type dataType)
        {
            var converter = _convertersHashtable[dataType.Name];

            if (converter == null)
            {
                lock (_convertersHashtableLock)
                {
                    converter = _convertersHashtable[dataType.Name];

                    if (converter == null)
                    {
                        converter = GetResponsePacketConverter(dataType);
                        _convertersHashtable.Add(dataType.Name, converter);
                    }
                }
            }

            return (IConverter)converter;
        }

        internal TarantoolTupleType GetTarantoolTupleType(params Type[] tupleItemsTypes)
        {
            var typesFullName = GetTypeArrayTypesFullName(tupleItemsTypes);
            var tarantoolTupleType = _tarantoolTupleTypeHashtable[typesFullName];

            if (tarantoolTupleType == null)
            {
                lock (_tarantoolTupleTypeHashtableLock)
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

        private IConverter GetTarantoolTupleConverter(TarantoolTupleType tarantoolTupleType)
        {
            var converter = ConverterContext.GetConverter(tarantoolTupleType);
            if (converter == null)
            {
                lock (_lock)
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

        private IConverter GetTarantoolArrayTupleConverter(TarantoolTupleArrayType tarantoolTupleArrayType)
        {
            var converter = ConverterContext.GetConverter(tarantoolTupleArrayType);
            if (converter == null)
            {
                lock (_arrayLock)
                {
                    converter = ConverterContext.GetConverter(tarantoolTupleArrayType);
                    if (converter == null)
                    {
                        converter = new SimpleArrayConverter(tarantoolTupleArrayType.GetElementType());
                        ConverterContext.Add(tarantoolTupleArrayType, converter);
                    }
                }
            }

            return converter;
        }
    }
}
