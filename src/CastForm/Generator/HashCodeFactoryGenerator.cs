using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;

namespace CastForm.Generator
{
    /// <summary>
    /// Create a class HashCodeFactory with methods calls GenHashCode with Type that was added.
    /// </summary>
    public class HashCodeFactoryGenerator
    {
        private readonly TypeBuilder _builder;
        private static readonly MethodInfo s_taskId = typeof(Task).GetProperty(nameof(Task.CurrentId), BindingFlags.Public | BindingFlags.Static).GetMethod;
        private static readonly MethodInfo s_threadCurrent = typeof(Thread).GetProperty(nameof(Thread.CurrentThread), BindingFlags.Public | BindingFlags.Static).GetMethod;
        private static readonly MethodInfo s_threadId = typeof(Thread).GetProperty(nameof(Thread.ManagedThreadId), BindingFlags.Public | BindingFlags.Instance).GetMethod;
        private readonly HashSet<Type> _typesRegister = new HashSet<Type>();


        // This code generate HashCodeFactory, that should be like:
        // https://sharplab.io/#v2:C4LglgNgPgAgTARgLACgYAYAEMEDoAqAFgE4CmAhgCZgB2A5gNyobYICsTKzAzNnJgGFMAb1SZx2XjAAsmALIAKAJQixEgL6pNXNFIQA2PpgDipGgAlyAZ0KpRKCZNaHawE2cs2BAe0qkFAMpgALYADhCkAIKYVt4ArsQAxqRKauL2jo4wAOyYnoQ+frg+wQBGtP5pmY75haTF3mUVCrEJybgAkpQANDHxSfX4pAAewL2tA51WAKI05KURSt1V1eII6OgA+hvoyw6rjkRkVMUJZDTARxSUuHLkc3SklFdUXXsHWfqnxOfAXSsSJScRzabSoHhGIJhCKROxVGC8VyYLoiTCPYAMGKkDGYbRZPRYIajVHozFWbGYvESBGYUrebwQZEzOYLUgkilYnGgoA=
        // Thread.CurrentThread.ManagedThreadId and Task.CurrentId is used to ensure we a are creating unique hash code foreach process.
        // 123 = type.GetHashCode()
        // public static class HashCodeFactory
        // {
        //      public static int GenHashCode(object o)
        //      {
        //          return HashCode.Combine(o.GetHashCode(), 123, Thread.CurrentThread.ManagedThreadId, Task.CurrentId);
        //      }
        // }

        /// <summary>
        /// Initialize a new instance of <see cref="HashCodeFactoryGenerator"/>
        /// </summary>
        public HashCodeFactoryGenerator()
        {
            const string typeName = "HashCodeFactory";

            var assemblyName = new AssemblyName($"{typeName}Assembly");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            var moduleBuilder = assemblyBuilder.DefineDynamicModule($"{typeName}Module");
            const TypeAttributes typeAttributes = TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.BeforeFieldInit;

            _builder = moduleBuilder.DefineType(typeName, typeAttributes, null, Type.EmptyTypes);
        }

        /// <summary>
        /// Add override GenHashCode with type
        /// </summary>
        /// <param name="type">Type parameter that should be received in GenHashCode</param>
        public void Add(Type type)
        {
            if (!_typesRegister.Add(type))
            {
                return;
            }

            var hashCode = type.GetMethod(nameof(GetHashCode));
            if (hashCode.DeclaringType == type)
            {
                HasHashCode(type);
            }
            else
            {
                HasNotHashCode(type);
            }
        }

        /// <summary>
        /// Build class to create HashCodeFactory
        /// </summary>
        public void Build() => _builder.CreateType();

        /// <summary>
        /// Type of HashCodeFactory.
        /// </summary>
        public Type Type => _builder;

        private void HasHashCode(Type type)
        {
            // This Method should generate this code
            // public static int GenHashCode(Foo o)
            // {
            //      return HashCode.Combine(HashCode.Combine(o.Id, o.Text), 123, Thread.CurrentThread.ManagedThreadId, Task.CurrentId);
            //  }

            var getHashCode = _builder.DefineMethod("GenHashCode",
                MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig, CallingConventions.Standard,
                typeof(int), new[] { type });

            var il = getHashCode.GetILGenerator();

            var hashCombine = typeof(HashCode).GetMethods(BindingFlags.Public | BindingFlags.Static).ElementAt(3)
                .MakeGenericMethod(type, typeof(int), typeof(int), typeof(int?));

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldc_I4, type.GetHashCode());
            il.EmitCall(OpCodes.Call, s_threadCurrent, null);
            il.EmitCall(OpCodes.Callvirt, s_threadId, null);
            il.EmitCall(OpCodes.Call, s_taskId, null);
            il.EmitCall(OpCodes.Call, hashCombine, null);
            il.Emit(OpCodes.Ret);
        }

        private void HasNotHashCode(Type type)
        {
            // This Method should generate this code
            // public static int GenHashCode(Foo o)
            // {
            //      return HashCode.Combine(o.GetHashCode(), 123, Thread.CurrentThread.ManagedThreadId, Task.CurrentId);
            //  }
            var getHashCode = _builder.DefineMethod("GenHashCode",
                MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard,
                typeof(int), new[] { type });
            var il = getHashCode.GetILGenerator();

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.PropertyType.IsNetType() || x.PropertyType.IsNullable()).ToArray();
            var hashCodes = typeof(HashCode).GetMethods(BindingFlags.Public | BindingFlags.Static).ToArray();

            var isFirst = true;

            for (var i = 0; i < properties.Length; i += 7)
            {
                var tmp = i + 7 < properties.Length ? properties[i..(i + 7)] : properties[i..];

                var hashCodeTypes = tmp.Select(x => x.PropertyType).ToList();

                foreach (var property in tmp)
                {
                    il.Emit(OpCodes.Ldarg_0);
                    il.EmitCall(OpCodes.Callvirt, property.GetMethod, null);
                }

                MethodInfo? hashCodeFun = null;
                if (isFirst)
                {
                    hashCodeFun = hashCodes[tmp.Length - 1].MakeGenericMethod(hashCodeTypes.ToArray());
                }
                else
                {
                    hashCodeTypes.Insert(0, typeof(int));
                    hashCodeFun = hashCodes[tmp.Length].MakeGenericMethod(hashCodeTypes.ToArray());
                }
                isFirst = false;

                il.EmitCall(OpCodes.Call, hashCodeFun, null);
            }

            var hashCodeFinal = hashCodes[3].MakeGenericMethod(typeof(int), typeof(int), typeof(int), typeof(int?));
            il.Emit(OpCodes.Ldc_I4, type.GetHashCode());
            il.EmitCall(OpCodes.Call, s_threadCurrent, null);
            il.EmitCall(OpCodes.Callvirt, s_threadId, null);
            il.EmitCall(OpCodes.Call, s_taskId, null);
            il.EmitCall(OpCodes.Call, hashCodeFinal, null);
            il.Emit(OpCodes.Ret);
        }
    }
}
