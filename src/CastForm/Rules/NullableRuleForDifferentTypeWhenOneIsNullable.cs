using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace CastForm.Rules
{
    /// <summary>
    /// Rule when One is nullable and other not and different type. like:
    ///     int Id -> long? Id
    /// </summary>
    public class NullableRuleForDifferentTypeWhenOneIsNullable : IRuleMapper, IRuleNeedLocalField
    {
        /// <summary>
        /// Initialize a new instance of <see cref="NullableRuleForDifferentTypeWhenOneIsNullable"/>
        /// </summary>
        /// <param name="source">The source member</param>
        /// <param name="destiny">The source member</param>
        public NullableRuleForDifferentTypeWhenOneIsNullable(MemberInfo source, MemberInfo destiny)
        {
            SourceProperty = source as PropertyInfo ?? throw new ArgumentNullException(nameof(source));
            DestinyProperty = destiny as PropertyInfo ?? throw new ArgumentNullException(nameof(destiny));

            if (SourceProperty.PropertyType.IsNullable())
            {
                LocalFields = new [] { SourceProperty.PropertyType } ;
            }
        }

        /// <inheritdoc/>
        public PropertyInfo DestinyProperty { get; }

        /// <inheritdoc/>
        public PropertyInfo? SourceProperty { get; }

        /// <inheritdoc/>
        public void Execute(ILGenerator il, IReadOnlyDictionary<string, FieldBuilder> fields, IReadOnlyDictionary<Type, LocalBuilder> localFields, IEnumerable<MapperProperty> mapperProperties)
        {
            if (DestinyProperty.PropertyType.IsNullable())
            {
                // based on https://sharplab.io/#v2:C4LglgNgPgAgTARgLACgYGYAE9MGFMiYCSAsgIYAOmA3qpvdlgMpgC2FEApgEKbkUAKFuy4BBTAGcA9gFcATgGNOAShp0GGmAHZMAO04B3TMI491G+rRQWbxACaYAvHim6AbpznAAdABUpRLrA6HAC0vJK3kR2ygA05hYAvgDcCYmo6SioqBiYYEGeAGZkSsT8qFYaJly8/EJspuLhiiqpKJk5WDjVnKIVCbkQrgDm9jSYw5zAyZJTM5kdaF1wxg01/dYMufnAAPxj1BNzs9OYC0A===
                // public class Map
                // {
                //      public SimpleB Map(SimpleA source)
                //      {
                //          return new SimpleB
                //          {
                //              Int = Convert.ToInt32(source.Long) // This mapper is create this line
                //          };
                //      }
                // }
                // public class SimpleA
                // {
                //      public long Long { get; set;}
                // }
                // public class SimpleB
                // {
                //      public int? Int { get; set;}
                // }
                GenerateMapWithDestinyAsNullable(il);
            }
            else
            {
                // based on https://sharplab.io/#v2:C4LglgNgPgAgTARgLACgYGYAE9MGFMiYCSAsgIYAOmA3qpvdlgMpgC2FEApgEKbkUAKFuy4BBTAGcA9gFcATgGNOAShp0GGmAHZMAO04B3TMI491G+rRQWbxACaYAvHim6AbpznAAdABUpRLrA6HAC0vJK3kQOAPwxmHacAGZkMhDAyuYWAL4A3FnZqIUoqKgYmGBBnilKxPyoVhomXLz8Qmym4uGKKvkoxWVYOM2cog1Z5RCuAObx0TSY05zAuZLLq8UDaENwxh0t49YM5ZXA9gtLK2tXm0A===
                // public class Map
                // {
                //      public SimpleB Map(SimpleA source)
                //      {
                //          return new SimpleB
                //          {
                //              Int = Convert.ToInt32(source.Long ?? source.Int) // This mapper is create this line
                //          };
                //      }
                // }
                // public class SimpleA
                // {
                //      public long? Long { get; set;}
                // }
                // public class SimpleB
                // {
                //      public int Int { get; set;}
                // }
                GenerateMapWithDestinyAsNotNullable(il, localFields);
            }
        }
        
        private void GenerateMapWithDestinyAsNullable(ILGenerator il)
        {
            var constructor = typeof(Nullable<>).MakeGenericType(Nullable.GetUnderlyingType(DestinyProperty.PropertyType)).GetConstructors()[0];
            var convert = typeof(Convert).GetRuntimeMethod(GetConvertTo(DestinyProperty.PropertyType), new[] { SourceProperty!.PropertyType });

            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, SourceProperty.GetMethod, null);
            il.EmitCall(OpCodes.Call, convert, null);
            il.Emit(OpCodes.Newobj, constructor);
            il.EmitCall(OpCodes.Callvirt, DestinyProperty.SetMethod, null);
        }

        private void GenerateMapWithDestinyAsNotNullable(ILGenerator il, IReadOnlyDictionary<Type, LocalBuilder> localField)
        {
            var getValueOrDefault = SourceProperty!.PropertyType.GetMethods().First(x => x.Name == "GetValueOrDefault" && x.GetParameters().Length == 0);
            var field = localField[SourceProperty.PropertyType];
            var convert = typeof(Convert).GetRuntimeMethod(GetConvertTo(DestinyProperty.PropertyType), new[] { SourceProperty.PropertyType.GetUnderlyingType() });

            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, SourceProperty.GetMethod, null);
            il.Emit(OpCodes.Stloc_S, field.LocalIndex);
            il.Emit(OpCodes.Ldloca_S, field.LocalIndex);
            il.EmitCall(OpCodes.Call, getValueOrDefault, null);
            il.EmitCall(OpCodes.Call, convert, null);
            il.EmitCall(OpCodes.Callvirt, DestinyProperty.SetMethod, null);
        }

        private static string GetConvertTo(Type type)
        {
            type = type.GetUnderlyingType();

            if (type == typeof(float))
            {
                return "ToSingle";
            }

            return $"To{type.Name}";
        }

        /// <inheritdoc/>
        public IEnumerable<Type>? LocalFields { get; }
    }
}
