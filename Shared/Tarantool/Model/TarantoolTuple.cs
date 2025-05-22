// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
using System.Collections;
using System.Text;
using nanoFramework.Tarantool.Dto;

namespace nanoFramework.Tarantool.Model
{
    /// <summary>
    /// <see cref="Tarantool"/> tuple class.
    /// </summary>
    public class TarantoolTuple
    {
#nullable enable
        private readonly TarantoolTupleType _tarantoolTupleType;
        private readonly Type[] _tupleItemTypes;
        private readonly ArrayList _items = new ArrayList();

        /// <summary>
        ///  Prevents a default instance of the <see cref="TarantoolTuple" /> class from being created.
        /// </summary>
        private TarantoolTuple()
        {
            _tupleItemTypes = new Type[0];
            _tarantoolTupleType = TarantoolTupleType.Create(_tupleItemTypes);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TarantoolTuple"/> class.
        /// </summary>
        /// <param name="tupleObjects"><see cref="Tarantool"/> tuple items values.</param>
        private TarantoolTuple(params object?[] tupleObjects)
        {
            _tupleItemTypes = new Type[tupleObjects.Length];

            for (int i = 0; i < tupleObjects.Length; i++)
            {
                var itemObject = tupleObjects[i];
                if (itemObject != null)
                {
                    _tupleItemTypes[i] = itemObject.GetType();
                }

                _items.Add(tupleObjects[i]);
            }

            _tarantoolTupleType = TarantoolTupleType.Create(_tupleItemTypes);
        }

        /// <summary>
        /// Gets <see cref="Tarantool"/> tuple item value by item index.
        /// </summary>
        /// <param name="index">Item index number.</param>
        /// <returns><see cref="Tarantool"/> tuple item object.</returns>
        public object? this[int index]
        {
            get
            {
                return _items[index];
            }
        }

        /// <summary>
        /// Gets <see cref="Tarantool"/> tuple length (items count).
        /// </summary>
        public int Length => _tupleItemTypes.Length;

        /// <summary>
        /// Gets empty <see cref="Tarantool"/> tuple.
        /// </summary>
        public static TarantoolTuple Empty { get; } = new TarantoolTuple();

        /// <summary>
        /// Create <see cref="Tarantool"/> tuple.
        /// </summary>
        /// <param name="tupleObjects"><see cref="Tarantool"/> tuple items value.</param>
        /// <returns>New <see cref="TarantoolTuple"/> instance.</returns>
        public static TarantoolTuple Create(params object?[] tupleObjects)
        {
            return new TarantoolTuple(tupleObjects);
        }

        /// <summary>
        /// Overrides base <see cref="object.Equals(object?)"/> method. Equals instances <see cref="TarantoolTuple"/> and <see cref="object"/> as <see cref="TarantoolTuple"/>.
        /// </summary>
        /// <param name="obj">Object for equal.</param>
        /// <returns><see langword="true"/> if two object is equal as <see cref="TarantoolTuple"/>, other <see langword="false"/>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((TarantoolTuple)obj);
        }
#nullable disable

        /// <summary>
        /// Overrides base class method <see cref="object.GetHashCode"/>
        /// </summary>
        /// <returns>Hash code <see cref="Tarantool"/> tuple.</returns>
        public override int GetHashCode()
        {
            int result = 0;

            foreach (var item in _items)
            {
                result &= item.GetHashCode();
            }

            return result;
        }

        /// <summary>
        /// Overrides base class method <see cref="object.ToString"/>
        /// </summary>
        /// <returns><see cref="Tarantool"/> tuple string.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in _items)
            {
                sb.Append(item.ToString());
                sb.Append(", ");
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 2, 2);
            }

            return $"[{sb}]";
        }

        /// <summary>
        /// Overrides base class method <see cref="object.GetType"/>
        /// </summary>
        /// <returns><see cref="Type"/> <see cref="TarantoolTuple"/> instance.</returns>
        public new Type GetType()
        {
            return _tarantoolTupleType;
        }

        private bool Equals(TarantoolTuple other)
        {
            if (_items.Count != other._items.Count)
            {
                return false;
            }

            for (int i = 0; i < _items.Count; i++)
            {
                if (_tupleItemTypes[i] != other._tupleItemTypes[i] || !this[i].Equals(other[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
