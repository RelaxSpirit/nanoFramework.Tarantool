// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.Tarantool.Model.Enums
{
    /// <summary>
    /// Enum <see cref="Tarantool"/> key type.
    /// </summary>
    public enum Key : uint
    {
        /// <summary>
        /// Code request key type.
        /// </summary>
        Code = 0x00,

        /// <summary>
        /// Sync key type.
        /// </summary>
        Sync = 0x01,

        /// <summary>
        /// Schema id request key type.
        /// </summary>
        SchemaId = 0x05,

        /// <summary>
        /// Space id request key type.
        /// </summary>
        SpaceId = 0x10,

        /// <summary>
        /// Index id request key type.
        /// </summary>
        IndexId = 0x11,

        /// <summary>
        /// Limit request key type.
        /// </summary>
        Limit = 0x12,

        /// <summary>
        /// Offset request key type.
        /// </summary>
        Offset = 0x13,

        /// <summary>
        /// Iterator request key type.
        /// </summary>
        Iterator = 0x14,

        /// <summary>
        /// Key request type.
        /// </summary>
        Key = 0x20,

        /// <summary>
        /// Tuple request key type.
        /// </summary>
        Tuple = 0x21,

        /// <summary>
        /// Function name request key type.
        /// </summary>
        FunctionName = 0x22,

        /// <summary>
        /// User name request key type.
        /// </summary>
        Username = 0x23,

        /// <summary>
        /// Expression request key type.
        /// </summary>
        Expression = 0x27,

        /// <summary>
        /// Ops request key type.
        /// </summary>
        Ops = 0x28,

        /// <summary>
        /// Field name <see cref="Tarantool"/> version 2.0.4 request key type.
        /// </summary>
        FieldName_2_0_4 = 0x29,

        /// <summary>
        /// Field name <see cref="Tarantool"/> version before 2.0.4 request key type.
        /// </summary>
        FieldName = 0x0,

        /// <summary>
        /// Data response key type.
        /// </summary>
        Data = 0x30,

        /// <summary>
        /// Error <see cref="Tarantool"/> version 2.4 response key type.
        /// </summary>
        Error24 = 0x31,

        /// <summary>
        /// Metadate response key type.
        /// </summary>
        Metadata = 0x32,

        /// <summary>
        /// Error response key type.
        /// </summary>
        Error = 0x52,

        /// <summary>
        /// Sql query text key type.
        /// </summary>
        SqlQueryText = 0x40,

        /// <summary>
        /// Sql query parameters key type.
        /// </summary>
        SqlParameters = 0x41,

        /// <summary>
        /// Sql options key type.
        /// </summary>
        SqlOptions = 0x42,

        /// <summary>
        /// Sql info key type.
        /// </summary>
        SqlInfo = 0x42,

        /// <summary>
        /// Sql info <see cref="Tarantool"/> version 2.0.4 key type.
        /// </summary>
        SqlInfo_2_0_4 = 0x43,

        /// <summary>
        /// Sql row count <see cref="Tarantool"/> version 2.0.4 key type.
        /// </summary>
        SqlRowCount_2_0_4 = 0x44,

        /// <summary>
        /// Sql row count key type.
        /// </summary>
        SqlRowCount = 0x0,

        /// <summary>
        /// Server id replication key type.
        /// </summary>
        ServerId = 0x02,

        /// <summary>
        /// Lns replication key type.
        /// </summary>
        Lsn = 0x03,

        /// <summary>
        /// Timestamp replication key type.
        /// </summary>
        Timestamp = 0x04,

        /// <summary>
        /// Server uuid replication key type.
        /// </summary>
        ServerUuid = 0x24,

        /// <summary>
        /// Cluster uuid replication key type.
        /// </summary>
        ClusterUuid = 0x25,

        /// <summary>
        /// Vclock replication key type.
        /// </summary>
        Vclock = 0x26,
    }
}
