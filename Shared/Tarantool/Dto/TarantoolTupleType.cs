// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
#if !NANOFRAMEWORK_1_0
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
#endif
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace nanoFramework.Tarantool.Dto
{
    /// <summary>
    /// <see cref="Tarantool"/> tuple type class.
    /// </summary>
    public class TarantoolTupleType : Type
    {
        private static readonly Type DefaultTarantoolTupleType = typeof(TarantoolTupleType);

        private string _name = string.Empty;
        private string _fullName = string.Empty;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TarantoolTupleType"/> class.
        /// </summary>
        /// <param name="tupleTypes">Tuple items types.</param>
        internal TarantoolTupleType(params Type[] tupleTypes)
        { 
            TupleTypes = tupleTypes;

            if (tupleTypes.Length < 1)
            {
                _name = DefaultTarantoolTupleType.Name;
                _fullName = DefaultTarantoolTupleType.FullName ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets tuple items types.
        /// </summary>
        public Type[] TupleTypes { get; }

        /// <summary>
        /// Gets override base class property <see cref="Type.Assembly"/>.
        /// </summary>
        public override Assembly Assembly => DefaultTarantoolTupleType.Assembly;

        /// <summary>
        /// Gets override base class property <see cref="Name"/>.
        /// </summary>
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

        /// <summary>
        /// Gets override base class property <see cref="MemberType"/>.
        /// </summary>
        public override MemberTypes MemberType => DefaultTarantoolTupleType.MemberType;

#if NANOFRAMEWORK_1_0
        /// <summary>
        /// Gets override base class property <see cref="Type.FullName"/>.
        /// </summary>
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

        /// <summary>
        /// Gets override base class property <see cref="Type.AssemblyQualifiedName"/>.
        /// </summary>
        public override string AssemblyQualifiedName => DefaultTarantoolTupleType.AssemblyQualifiedName;

        /// <summary>
        /// Gets override base class property <see cref="Type.BaseType"/>.
        /// </summary>
        public override Type BaseType => DefaultTarantoolTupleType.BaseType;

        /// <summary>
        /// Overrides base class method <see cref="Type.GetElementType()"/>.
        /// </summary>
        /// <returns>Element type.</returns>
        public override Type GetElementType()
        {
            return DefaultTarantoolTupleType.GetElementType();
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.GetField(string, BindingFlags)"/>.
        /// </summary>
        /// <param name="name">Field name.</param>
        /// <param name="bindingAttr">Binding attributes flags.</param>
        /// <returns>Field info or <see langword="null"/>.</returns>
        public override FieldInfo GetField(string name, BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleType.GetField(name, bindingAttr);
        }
#endif
        /// <summary>
        /// Overrides base class method <see cref="GetCustomAttributes(bool)"/>.
        /// </summary>
        /// <param name="inherit">Inherit flag.</param>
        /// <returns>Objects array.</returns>
        public override object[] GetCustomAttributes(bool inherit)
        {
            return DefaultTarantoolTupleType.GetCustomAttributes(inherit);
        }

        /// <summary>
        /// Overrides base class method <see cref="GetFields(BindingFlags)"/>.
        /// </summary>
        /// <param name="bindingAttr">Binding attributes flags.</param>
        /// <returns>Field info array.</returns>
        public override FieldInfo[] GetFields(BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleType.GetFields(bindingAttr);
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.GetInterfaces()"/>.
        /// </summary>
        /// <returns>Types array.</returns>
        public override Type[] GetInterfaces()
        {
            return DefaultTarantoolTupleType.GetInterfaces();
        }

        /// <summary>
        /// Overrides base class method <see cref="GetMethods(BindingFlags)"/>.
        /// </summary>
        /// <param name="bindingAttr">Binding attributes flags.</param>
        /// <returns>Method info array.</returns>
        public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleType.GetMethods();
        }
 
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void InitializeName()
        {
            if (string.IsNullOrEmpty(_name))
            {
                StringBuilder sbName = new StringBuilder();
                sbName.Append(DefaultTarantoolTupleType.Name);
                sbName.Append('<');
                foreach (Type t in TupleTypes)
                {
                    sbName.Append(t.Name);
                    sbName.Append(',');
                }

                sbName.Remove(sbName.Length - 1, 1);
                sbName.Append('>');

                _name = sbName.ToString();
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void InitializeFullName()
        {
            if (string.IsNullOrEmpty(_fullName))
            {
                _fullName = $"{DefaultTarantoolTupleType.FullName}<{TarantoolContext.GetTypeArrayTypesFullName(TupleTypes)}>";
            }
        }

#if !NANOFRAMEWORK_1_0
        /// <summary>
        /// Gets type full name.
        /// </summary>
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

        /// <summary>
        /// Gets assembly qualified name.
        /// </summary>
        public override string? AssemblyQualifiedName => DefaultTarantoolTupleType.AssemblyQualifiedName;

        /// <summary>
        /// Gets base type.
        /// </summary>
        public override Type? BaseType => DefaultTarantoolTupleType.BaseType;

        /// <summary>
        /// Gets guid.
        /// </summary>
        public override Guid GUID => DefaultTarantoolTupleType.GUID;

        /// <summary>
        /// Gets module.
        /// </summary>
        public override Module Module => DefaultTarantoolTupleType.Module;

        /// <summary>
        /// Gets namespace.
        /// </summary>
        public override string? Namespace => DefaultTarantoolTupleType.Namespace;

        /// <summary>
        /// Gets underlying system type.
        /// </summary>
        public override Type UnderlyingSystemType => DefaultTarantoolTupleType.UnderlyingSystemType;

        /// <summary>
        /// Overrides base class method <see cref="Type.GetElementType()"/>.
        /// </summary>
        /// <returns>Nullable type.</returns>
        public override Type? GetElementType()
        {
            return DefaultTarantoolTupleType.GetElementType();
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.GetField(string, BindingFlags)"/>.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="bindingAttr">Binding attributes flags.</param>
        /// <returns>Nullable field info.</returns>
        public override FieldInfo? GetField(string name, BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleType.GetField(name, bindingAttr);
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.GetConstructors(BindingFlags)"/>.
        /// </summary>
        /// <param name="bindingAttr">Binding attributes flags.</param>
        /// <returns>Constructor info array.</returns>
        public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleType.GetConstructors(bindingAttr);
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.GetCustomAttributes(Type, bool)"/>.
        /// </summary>
        /// <param name="attributeType">Attribute type.</param>
        /// <param name="inherit">Inherit flag.</param>
        /// <returns>Objects array.</returns>
        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return DefaultTarantoolTupleType.GetCustomAttributes(attributeType, inherit);
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.GetEvent(string, BindingFlags)"/>.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="bindingAttr">Binding attributes flags.</param>
        /// <returns>Event info array.</returns>
        public override EventInfo? GetEvent(string name, BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleType.GetEvent(name, bindingAttr);
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.GetEvents(BindingFlags)"/>.
        /// </summary>
        /// <param name="bindingAttr">Binding attributes flags.</param>
        /// <returns>Event info array.</returns>
        public override EventInfo[] GetEvents(BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleType.GetEvents(bindingAttr);
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.GetInterface(string, bool)"/>.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="ignoreCase">Ignore case flag.</param>
        /// <returns>Nullable type.</returns>
        ////[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
        public override Type? GetInterface(string name, bool ignoreCase)
        {
            return DefaultTarantoolTupleType.GetInterface(name, ignoreCase);
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.GetMembers(BindingFlags)"/>.
        /// </summary>
        /// <param name="bindingAttr">Binding attributes flags.</param>
        /// <returns>Member info array.</returns>
        public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleType.GetMembers(bindingAttr);
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.GetNestedType(string, BindingFlags)"/>.
        /// </summary>
        /// <param name="name">Nmae.</param>
        /// <param name="bindingAttr">Binding attributes flags.</param>
        /// <returns>Nullable type.</returns>
        public override Type? GetNestedType(string name, BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleType.GetNestedType(name, bindingAttr);
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.GetNestedTypes(BindingFlags)"/>.
        /// </summary>
        /// <param name="bindingAttr">Binding attributes flags.</param>
        /// <returns>Tapes array.</returns>
        public override Type[] GetNestedTypes(BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleType.GetNestedTypes(bindingAttr);
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.GetProperties(BindingFlags)"/>.
        /// </summary>
        /// <param name="bindingAttr">Binding attributes flags.</param>
        /// <returns>Property info array.</returns>
        public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleType.GetProperties(bindingAttr);
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.InvokeMember(string, BindingFlags, Binder?, object?, object?[]?, ParameterModifier[]?, CultureInfo?, string[]?)"/>.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="invokeAttr">Invoke attribute.</param>
        /// <param name="binder">Binder.</param>
        /// <param name="target">Target.</param>
        /// <param name="args">Args.</param>
        /// <param name="modifiers">Modifiers.</param>
        /// <param name="culture">Culture.</param>
        /// <param name="namedParameters">Named parameters.</param>
        /// <returns>Nullable object.</returns>
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        public override object? InvokeMember(
            string name, 
            BindingFlags invokeAttr, 
            Binder? binder, 
            object? target,
            object?[] args,
            ParameterModifier[] modifiers, 
            CultureInfo? culture,
            string[] namedParameters)
        {
            return DefaultTarantoolTupleType.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
        }
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

        /// <summary>
        /// Overrides base class method <see cref="Type.IsDefined(Type, bool)"/>.
        /// </summary>
        /// <param name="attributeType">Attribute type.</param>
        /// <param name="inherit">Inherit.</param>
        /// <returns><see langword="true"/> or <see langword="false"/>.</returns>
        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return DefaultTarantoolTupleType.IsDefined(attributeType, inherit);
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
