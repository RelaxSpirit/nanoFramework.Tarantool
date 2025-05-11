// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;

namespace nanoFramework.Tarantool.Model.Responses
{
    /// <summary>
    /// Universal response to a <see cref="Tarantool"/> request.
    /// </summary>
    public class DataResponse : SqlInfoResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataResponse"/> class.
        /// </summary>
        /// <param name="data">Response data object.</param>
        /// <param name="metadata">Response metadata.</param>
        /// <param name="sqlInfo">Response SQL info.</param>
#nullable enable
        internal DataResponse(object? data, FieldMetadata[] metadata, SqlInfo? sqlInfo) : this(data, sqlInfo)
        {
            this.MetaData = metadata;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataResponse"/> class.
        /// </summary>
        /// <param name="data">Response data object.</param>
        /// <param name="sqlInfo">Response SQL info.</param>
        internal DataResponse(object? data, SqlInfo? sqlInfo) : base(sqlInfo)
        {
            if (data is ArrayList arrayList)
            {
                this.Data = (object[])arrayList.ToArray();
            }
            else
            {
                if (data == null)
                {
                    this.Data = new object[0];
                }
                else
                {
                    if (data.GetType().IsArray)
                    {
                        this.Data = (object[])data;
                    }
                    else
                    {
                        this.Data = new object[1] { data };
                    }
                }
            }

            this.MetaData = new FieldMetadata[0];
        }

        /// <summary>
        /// Gets data to response.
        /// </summary>
        public object[] Data { get; }

        /// <summary>
        /// Gets fields metadata.
        /// </summary>
        public FieldMetadata[] MetaData { get; }
    }
}
