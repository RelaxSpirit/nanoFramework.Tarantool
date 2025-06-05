// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
#if !NANOFRAMEWORK_1_0
using System.Globalization;
#endif
using System.Reflection;
using System.Runtime.CompilerServices;

namespace nanoFramework.Tarantool.Dto
{
    /// <summary>
    /// The <see cref="Tarantool"/> tuple array type class.
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

        public override Assembly Assembly => DefaultTarantoolTupleArrayType.Assembly;

        public override MemberTypes MemberType => DefaultTarantoolTupleArrayType.MemberType;

        public override string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    InitializeName();
                }

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

        /// <summary>
        /// Overrides base class method <see cref="Type.GetField(string, BindingFlags)"/>.
        /// </summary>
        /// <param name="name">Field name.</param>
        /// <param name="bindingAttr">Binding attributes flags.</param>
        /// <returns>Field info or <see langword="null"/>.</returns>
        public override FieldInfo GetField(string name, BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleArrayType.GetField(name, bindingAttr);
        }
#endif

        public override object[] GetCustomAttributes(bool inherit)
        {
            return DefaultTarantoolTupleArrayType.GetCustomAttributes(inherit);
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.GetElementType()"/>.
        /// </summary>
        /// <returns>Element type.</returns>
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
                {
                    InitializeFullName();
                }

                return _fullName;
            }
        }

        public override FieldInfo? GetField(string name, BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleArrayType.GetField(name, bindingAttr);
        }

        public override string? AssemblyQualifiedName => DefaultTarantoolTupleArrayType.AssemblyQualifiedName;

        public override Type? BaseType => DefaultTarantoolTupleArrayType.BaseType;

        public override Guid GUID => DefaultTarantoolTupleArrayType.GUID;

        public override Module Module => DefaultTarantoolTupleArrayType.Module;

        public override string? Namespace => DefaultTarantoolTupleArrayType.Namespace;

        public override Type UnderlyingSystemType => DefaultTarantoolTupleArrayType.UnderlyingSystemType;

        public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleArrayType.GetConstructors(bindingAttr);
        }

        public override EventInfo? GetEvent(string name, BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleArrayType.GetEvent(name, bindingAttr);
        }

        public override EventInfo[] GetEvents(BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleArrayType.GetEvents(bindingAttr);
        }

        //// [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
        public override Type? GetInterface(string name, bool ignoreCase)
        {
            return DefaultTarantoolTupleArrayType.GetInterface(name, ignoreCase);
        }

        public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleArrayType.GetMembers(bindingAttr);
        }

        public override Type? GetNestedType(string name, BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleArrayType.GetNestedType(name, bindingAttr);
        }

        public override Type[] GetNestedTypes(BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleArrayType.GetNestedTypes(bindingAttr);
        }

        public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleArrayType.GetProperties(bindingAttr);
        }

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        public override object? InvokeMember(string name, BindingFlags invokeAttr, Binder? binder, object? target, object?[] args, ParameterModifier[] modifiers, CultureInfo? culture, string[] namedParameters)
        {
            return DefaultTarantoolTupleArrayType.InvokeMember(name, invokeAttr, binder, target, args);
        }
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return DefaultTarantoolTupleArrayType.GetCustomAttributes(attributeType, inherit);
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return DefaultTarantoolTupleArrayType.IsDefined(attributeType, inherit);
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.GetAttributeFlagsImpl()"/>.
        /// </summary>
        /// <returns><see cref="NotImplementedException"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        protected override TypeAttributes GetAttributeFlagsImpl()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.GetConstructorImpl(BindingFlags, Binder?, CallingConventions, Type[], ParameterModifier[])"/>.
        /// </summary>
        /// <param name="bindingAttr">Binding attribute.</param>
        /// <param name="binder">Binder.</param>
        /// <param name="callConvention">Call convention.</param>
        /// <param name="types">Types.</param>
        /// <param name="modifiers">Modifiers.</param>
        /// <returns><see cref="NotImplementedException"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        protected override ConstructorInfo? GetConstructorImpl(BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.GetMethodImpl(string, BindingFlags, Binder?, CallingConventions, Type[], ParameterModifier[])"/>.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="bindingAttr">Binding attribute.</param>
        /// <param name="binder">Binder.</param>
        /// <param name="callConvention">Call convention.</param>
        /// <param name="types">Types.</param>
        /// <param name="modifiers">Modifiers.</param>
        /// <returns><see cref="NotImplementedException"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        protected override MethodInfo? GetMethodImpl(string name, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.GetPropertyImpl(string, BindingFlags, Binder?, Type?, Type[], ParameterModifier[])"/>.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="bindingAttr">Binding attribute.</param>
        /// <param name="binder">Binder.</param>
        /// <param name="returnType">Return type.</param>
        /// <param name="types">Types.</param>
        /// <param name="modifiers">Modifiers.</param>
        /// <returns><see cref="NotImplementedException"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        protected override PropertyInfo? GetPropertyImpl(string name, BindingFlags bindingAttr, Binder? binder, Type? returnType, Type[] types, ParameterModifier[] modifiers)
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.HasElementTypeImpl()"/>.
        /// </summary>
        /// <returns><see cref="NotImplementedException"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        protected override bool HasElementTypeImpl()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.IsArrayImpl()"/>.
        /// </summary>
        /// <returns><see cref="NotImplementedException"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        protected override bool IsArrayImpl()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.IsByRefImpl()"/>.
        /// </summary>
        /// <returns><see cref="NotImplementedException"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        protected override bool IsByRefImpl()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.IsCOMObjectImpl()"/>.
        /// </summary>
        /// <returns><see cref="NotImplementedException"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        protected override bool IsCOMObjectImpl()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.IsPointerImpl()"/>.
        /// </summary>
        /// <returns><see cref="NotImplementedException"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        protected override bool IsPointerImpl()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.IsPrimitiveImpl()"/>.
        /// </summary>
        /// <returns><see cref="NotImplementedException"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        protected override bool IsPrimitiveImpl()
        {
            throw new NotImplementedException();
        }
#endif
    }
}
