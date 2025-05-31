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
                    InitializeName();
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
        /// Overrides base class method <see cref="Type.GetElementType"/>.
        /// </summary>
        public override Type GetElementType()
        {
            return DefaultTarantoolTupleType.GetElementType();
        }

        /// <summary>
        /// Overrides base class method <see cref="GetField"/>.
        /// </summary>
        public override FieldInfo GetField(string name, BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleType.GetField(name, bindingAttr);
        }
#endif
        /// <summary>
        /// Overrides base class method <see cref="GetCustomAttributes"/>.
        /// </summary>
        public override object[] GetCustomAttributes(bool inherit)
        {
            return DefaultTarantoolTupleType.GetCustomAttributes(inherit);
        }

        /// <summary>
        /// Overrides base class method <see cref="GetFields"/>.
        /// </summary>
        public override FieldInfo[] GetFields(BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleType.GetFields(bindingAttr);
        }

        /// <summary>
        /// Overrides base class method <see cref="Type.GetInterfaces"/>.
        /// </summary>
        public override Type[] GetInterfaces()
        {
            return DefaultTarantoolTupleType.GetInterfaces();
        }

        /// <summary>
        /// Overrides base class method <see cref="GetMethods"/>.
        /// </summary>
        public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleType.GetMethods();
        }
 
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void InitializeName()
        {
            if (string.IsNullOrEmpty(_name))
            {
                StringBuilder sbName = new();
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
        public override string? FullName
        {
            get
            {
                if (string.IsNullOrEmpty(_fullName))
                    InitializeFullName();
                return _fullName;
            }
        }

        public override Type? GetElementType()
        {
            return DefaultTarantoolTupleType.GetElementType();
        }

        public override FieldInfo? GetField(string name, BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleType.GetField(name, bindingAttr);
        }

        public override string? AssemblyQualifiedName => DefaultTarantoolTupleType.AssemblyQualifiedName;

        public override Type? BaseType => DefaultTarantoolTupleType.BaseType;

        public override Guid GUID => DefaultTarantoolTupleType.GUID;

        public override Module Module => DefaultTarantoolTupleType.Module;

        public override string? Namespace => DefaultTarantoolTupleType.Namespace;

        public override Type UnderlyingSystemType => DefaultTarantoolTupleType.UnderlyingSystemType;

        public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleType.GetConstructors(bindingAttr);
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return DefaultTarantoolTupleType.GetCustomAttributes(attributeType, inherit);
        }

        public override EventInfo? GetEvent(string name, BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleType.GetEvent(name, bindingAttr);
        }

        public override EventInfo[] GetEvents(BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleType.GetEvents(bindingAttr);
        }


        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
        public override Type? GetInterface(string name, bool ignoreCase)
        {
            return DefaultTarantoolTupleType.GetInterface(name, ignoreCase);
        }

        public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleType.GetMembers(bindingAttr);
        }


        public override Type? GetNestedType(string name, BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleType.GetNestedType(name, bindingAttr);
        }

        public override Type[] GetNestedTypes(BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleType.GetNestedTypes(bindingAttr);
        }

        public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            return DefaultTarantoolTupleType.GetProperties(bindingAttr);
        }

        public override object? InvokeMember(
            string name, 
            BindingFlags invokeAttr, 
            Binder? binder, 
            object? target,
            object?[]? args,
            ParameterModifier[]? modifiers, 
            CultureInfo? culture,
            string[]? namedParameters)
        {
            return DefaultTarantoolTupleType.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return DefaultTarantoolTupleType.IsDefined(attributeType, inherit);
        }

        protected override TypeAttributes GetAttributeFlagsImpl()
        {
            throw new NotImplementedException();
        }

        protected override ConstructorInfo? GetConstructorImpl(BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[] types, ParameterModifier[]? modifiers)
        {
            throw new NotImplementedException();
        }

        protected override MethodInfo? GetMethodImpl(string name, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[]? types, ParameterModifier[]? modifiers)
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
#endif
    }
}
