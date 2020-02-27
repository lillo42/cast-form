using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;

namespace CastForm.Generator
{
    public class HashCodeFactoryGenerator
    {
        public static HashCodeFactoryGenerator Instance { get; set; }

        private readonly TypeBuilder _builder;
        private readonly MethodInfo _taskId;
        private readonly MethodInfo _threadCurrent;
        private readonly MethodInfo _threadId;
        private readonly HashSet<Type> _typesRegister = new HashSet<Type>();

        public HashCodeFactoryGenerator()
        {
            var typeName = "HashCodeFactory";

            var assemblyName = new AssemblyName($"{typeName}Assembly");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            var moduleBuilder = assemblyBuilder.DefineDynamicModule($"{typeName}Module");
            const TypeAttributes typeAttributes = TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.BeforeFieldInit;

            _builder = moduleBuilder.DefineType(typeName, typeAttributes, null, Type.EmptyTypes);

            _taskId = typeof(Task).GetProperty(nameof(Task.CurrentId), BindingFlags.Public | BindingFlags.Static).GetMethod;
            _threadCurrent = typeof(Thread).GetProperty(nameof(Thread.CurrentThread), BindingFlags.Public | BindingFlags.Static).GetMethod;
            _threadId = typeof(Thread).GetProperty(nameof(Thread.ManagedThreadId), BindingFlags.Public | BindingFlags.Instance).GetMethod;
        }


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

        public void Build() => _builder.CreateType();

        public Type Type => _builder;

        private void HasHashCode(Type type)
        {
            var getHashCode = _builder.DefineMethod("GenHashCode",
                MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig, CallingConventions.Any,
                typeof(int), new[] { type });

            var il = getHashCode.GetILGenerator();

            var hashCombine = typeof(HashCode).GetMethods(BindingFlags.Public | BindingFlags.Static).ElementAt(3)
                .MakeGenericMethod(type, typeof(int), typeof(int), typeof(int?));

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldc_I4, type.GetHashCode());
            il.EmitCall(OpCodes.Call, _threadCurrent, null);
            il.EmitCall(OpCodes.Callvirt, _threadId, null);
            il.EmitCall(OpCodes.Call, _taskId, null);
            il.EmitCall(OpCodes.Call, hashCombine, null);
            il.Emit(OpCodes.Ret);
        }

        private void HasNotHashCode(Type type)
        {
            var getHashCode = _builder.DefineMethod("GenHashCode",
                MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Any,
                typeof(int), new[] { type });
            var il = getHashCode.GetILGenerator();

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.FieldType.IsNetType() || x.FieldType.IsNullable()).ToArray();
            var hashCodes = typeof(HashCode).GetMethods(BindingFlags.Public | BindingFlags.Static).ToArray();

            bool isFirst = true;

            for (var i = 0; i < fields.Length; i += 7)
            {
                var tmp = i + 7 < fields.Length ? fields[i..(i + 7)] : fields[i..];

                var hashCodeTypes = tmp.Select(x => x.FieldType).ToList();

                foreach (var field in tmp)
                {
                    il.Emit(OpCodes.Ldfld, field);
                }

                MethodInfo? hashCodeFun = null;
                if (isFirst)
                {
                    hashCodeFun = hashCodes[tmp.Length - 1].MakeGenericMethod(hashCodeTypes.ToArray());
                }
                else
                {
                    hashCodeTypes.Add(typeof(int));
                    hashCodeFun = hashCodes[tmp.Length].MakeGenericMethod(hashCodeTypes.ToArray());
                }
                isFirst = false;

                il.EmitCall(OpCodes.Call, hashCodeFun, null);
            }

            var hashCodeFinal = hashCodes[3].MakeGenericMethod(type, typeof(int), typeof(int?), typeof(int));

            il.EmitCall(OpCodes.Call, _threadCurrent, null);
            il.EmitCall(OpCodes.Callvirt, _threadId, null);
            il.EmitCall(OpCodes.Call, _taskId, null);
            il.Emit(OpCodes.Ldc_I4_S, type.GetHashCode());
            il.EmitCall(OpCodes.Call, hashCodeFinal, null);
            il.Emit(OpCodes.Ret);
        }
    }
}
