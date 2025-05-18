// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !NANOFRAMEWORK_1_0
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
#else
using System;
#endif
using System.Reflection;
using System.Runtime.CompilerServices;

namespace nanoFramework.Tarantool.Dto
{
    /// <summary>
    /// The <see cref="Tarantool"/> tuple array type class
    /// </summary>
    internal class TarantoolTupleArrayType : Type
    {
        private static readonly Type DefaultTarantoolTupleArrayType = typeof(TarantoolTupleType[]);
        private readonly TarantoolTupleType _elementType;
        private string _name = string.Empty;
        private string _fullName = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Dto.TarantoolTupleArrayType"/> class.
        /// </summary>
        /// <param name="elementType"><see cref="Tarantool"/> element tuple type.</param>
        internal TarantoolTupleArrayType(TarantoolTupleType elementType)
        {
            _elementType = elementType;
        }

        public new static bool IsArray => true;

        public override Assembly Assembly => DefaultTarantoolTupleArrayType.Assembly;

        public override MemberTypes MemberType => DefaultTarantoolTupleArrayType.MemberType;

        public override string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                    InitializeName();
                return _name;
            }
        }

#if NANOFRAMEWORK_1_0
        public override string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(_fullName))
                {
                    InitializeFullName();
                }

                return _fullName;
            }
        }

        public override string AssemblyQualifiedName => DefaultTarantoolTupleArrayType.AssemblyQualifiedName;

        public override Type BaseType => DefaultTarantoolTupleArrayType.BaseType;

        public override FieldInfo GetField(string name, BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleArrayType.GetField(name, bindingAttr);
        }
#endif

        public override object[] GetCustomAttributes(bool inherit)
        {
            return DefaultTarantoolTupleArrayType.GetCustomAttributes(inherit);
        }

        public override Type GetElementType()
        {
            return _elementType;
        }

        public override FieldInfo[] GetFields(BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleArrayType.GetFields(bindingAttr);
        }

        public override Type[] GetInterfaces()
        {
            return DefaultTarantoolTupleArrayType.GetInterfaces();
        }

        public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleArrayType.GetMethods(bindingAttr);
        }
        
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void InitializeName()
        {
            _name = $"[{_elementType.Name}]";
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void InitializeFullName()
        {
            _fullName = $"[{_elementType.FullName}]";
        }

#if !NANOFRAMEWORK_1_0
        public override string? FullName
        {
            get
            {
                if (string.IsNullOrEmpty(_fullName))
                    InitializeFullName();
                return _fullName;
            }
        }

        public override FieldInfo? GetField(string name, BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleArrayType.GetField(name, bindingAttr);
        }

        public override string? AssemblyQualifiedName => DefaultTarantoolTupleArrayType.AssemblyQualifiedName;

        public override Type? BaseType => DefaultTarantoolTupleArrayType.BaseType;

        public override Guid GUID => throw new NotImplementedException();

        public override Module Module => throw new NotImplementedException();

        public override string? Namespace => throw new NotImplementedException();

        public override Type UnderlyingSystemType => throw new NotImplementedException();

        protected override TypeAttributes GetAttributeFlagsImpl()
        {
            throw new NotImplementedException();
        }

        protected override ConstructorInfo? GetConstructorImpl(
            BindingFlags bindingAttr, 
            Binder? binder, 
            CallingConventions callConvention, 
            Type[] types,
            ParameterModifier[]? modifiers)
        {
            throw new NotImplementedException();
        }

        public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override EventInfo? GetEvent(string name, BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override EventInfo[] GetEvents(BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
        public override Type? GetInterface(string name, bool ignoreCase)
        {
            throw new NotImplementedException();
        }

        public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        protected override MethodInfo? GetMethodImpl(string name, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[]? types, ParameterModifier[]? modifiers)
        {
            throw new NotImplementedException();
        }

        public override Type? GetNestedType(string name, BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override Type[] GetNestedTypes(BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        protected override PropertyInfo? GetPropertyImpl(string name, BindingFlags bindingAttr, Binder? binder, Type? returnType, Type[]? types, ParameterModifier[]? modifiers)
        {
            throw new NotImplementedException();
        }

        protected override bool HasElementTypeImpl()
        {
            throw new NotImplementedException();
        }

        public override object? InvokeMember(string name, BindingFlags invokeAttr, Binder? binder, object? target, object?[]? args, ParameterModifier[]? modifiers, CultureInfo? culture, string[]? namedParameters)
        {
            throw new NotImplementedException();
        }

        protected override bool IsArrayImpl()
        {
            throw new NotImplementedException();
        }

        protected override bool IsByRefImpl()
        {
            throw new NotImplementedException();
        }

        protected override bool IsCOMObjectImpl()
        {
            throw new NotImplementedException();
        }

        protected override bool IsPointerImpl()
        {
            throw new NotImplementedException();
        }

        protected override bool IsPrimitiveImpl()
        {
            throw new NotImplementedException();
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }
#endif
    }
}
