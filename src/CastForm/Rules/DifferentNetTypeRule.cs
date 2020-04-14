using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace CastForm.Rules
{
    /// <summary>
    /// Rule for map property when the are different but is .NET Type(int, long, decimal, DateTime e etc.)
    /// </summary>
    public class DifferentNetTypeRule : IRuleMapper
    {
        /// <summary>
        /// Initialize a new instance of <see cref="DifferentNetTypeRule"/>
        /// </summary>
        /// <param name="source">The source member</param>
        /// <param name="destiny">The source member</param>
        public DifferentNetTypeRule(MemberInfo source, MemberInfo destiny)
        {
            SourceProperty = source as PropertyInfo ?? throw new ArgumentNullException(nameof(source));
            DestinyProperty = destiny as PropertyInfo ?? throw new ArgumentNullException(nameof(destiny));
        }

        /// <inheritdoc/>
        public PropertyInfo DestinyProperty { get; }

        /// <inheritdoc/>
        public PropertyInfo? SourceProperty { get; }

        /// <inheritdoc/>
        public void Execute(ILGenerator il, IReadOnlyDictionary<string, FieldBuilder> fields, IReadOnlyDictionary<Type, LocalBuilder> localFields, IEnumerable<MapperProperty> mapperProperties)
            => Execute(il, SourceProperty!, DestinyProperty);

        private static void Execute(ILGenerator il, PropertyInfo source, PropertyInfo destiny)
        {
            if (ShouldUseSameType(source.PropertyType, destiny.PropertyType))
            {
                // based on https://sharplab.io/#v2:C4LglgNgPgAgTARgLACgYGYAE9MGFMiYCSAsgIYAOmA3qpvdlgMpgC2FEApgEKbkUAKFuy4BBTAGcA9gFcATgGNOAShp0GGmAHZMAO04B3TMI491G+rRQWbxACaYAvJNmLOAOiJ2ANOdv0AFU4AD2AnF3kldyDQvw0AXwBuP3jUVJRUVAxMMF1gTjkAMzIlYn5UKw0TLl5+ITZTcWlIlWSUdKysHGrOUQq/bNywrxpMAHNOYETJSen0zS6EAAZMGLDqcdmZqcx5hmyAIykpCGIJAFFdMgOuUYmdiS30jrQuuGMGmv7rfawIKV0Y3sdy2jx2e3o2Rgy1WIXWmweTzSQA=
                // There are same types that is different in .NET but to CLR they are the same pu
                // public class Map
                // {
                //      public SimpleB Map(SimpleA source)
                //      {
                //          return new SimpleB
                //          {
                //              Int = source.Short // This mapper is create this line
                //          };
                //      }
                // }
                // public class SimpleA
                // {
                //      public short Short { get; set;}
                // }
                // public class SimpleB
                // {
                //      public int Int { get; set;}
                // }

                SameTypeRule.Execute(il, source, destiny);
            }
            else
            {
                // based on https://sharplab.io/#v2:C4LglgNgPgAgTARgLACgYGYAE9MGFMiYCSAsgIYAOmA3qpvdlgMpgC2FEApgEKbkUAKFuy4BBTAGcA9gFcATgGNOAShp0GGmAHZMAO04B3TMI491G+rRQWbxACaYAvHim6AbpznAAdABUpRLrA6HAC0vJK3kR2ygA05rb0vpwAHsBOkrKKnH6pwAkaAL4A3AmFqOUoqKgYmGBBngBmZErE/KhWGiZcvPxCbKbi4dnKpSiVNVg43ZyiHQm1EK4A5vY0mMucwMWSWzuVmlMIAAyYyWnrm9u71wcMtQBGUlIQxBIAorpkD1yXezf7CrVNBTODGAY9ebWe5YerpaJ/a4Sf53ei1GAnM55RE7ZG3CpAA=
                // public class Map
                // {
                //      public SimpleB Map(SimpleA source)
                //      {
                //          return new SimpleB
                //          {
                //              Double = Convert.ToDouble(source.Int) // This mapper is create this line
                //          };
                //      }
                // }
                // public class SimpleA
                // {
                //      public int Int { get; set;}
                // }
                // public class SimpleB
                // {
                //      public double Double { get; set;}
                // }
                UseConverter(il, source, destiny);
            }
        }

        private static bool ShouldUseSameType(Type source, Type destiny)
        {
            if (destiny == typeof(int)
                || destiny == typeof(uint))
            {
                return source == typeof(int)
                       || source == typeof(uint)
                       || source == typeof(short)
                       || source == typeof(ushort)
                       || source == typeof(byte)
                       || source == typeof(sbyte);
            }

            if (destiny == typeof(long)
                || destiny == typeof(ulong))
            {
                return source == typeof(long)
                       || source == typeof(ulong)
                       || source == typeof(int)
                       || source == typeof(uint)
                       || source == typeof(short)
                       || source == typeof(ushort)
                       || source == typeof(byte)
                       || source == typeof(sbyte);
            }

            if (destiny == typeof(short)
                || destiny == typeof(ushort))
            {
                return source == typeof(short)
                       || source == typeof(ushort)
                       || source == typeof(byte)
                       || source == typeof(sbyte);
            }

            if (destiny == typeof(byte)
                || destiny == typeof(sbyte))
            {
                return source == typeof(byte)
                       || source == typeof(sbyte);
            }

            return false;
        }

        private static void UseConverter(ILGenerator il, PropertyInfo source, PropertyInfo destiny)
        {
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, source.GetMethod, null);
            var convert = typeof(Convert).GetRuntimeMethod(GetConvertTo(destiny.PropertyType), new[] { source.PropertyType });
            il.EmitCall(OpCodes.Call, convert, null);
            il.EmitCall(OpCodes.Callvirt, destiny.SetMethod, null);
        }

        private static string GetConvertTo(Type type)
        {
            if (type == typeof(float))
            {
                return "ToSingle";
            }

            return $"To{type.Name}";
        }
    }
}
