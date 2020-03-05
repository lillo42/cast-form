using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace CastForm.Rules
{
    /// <summary>
    /// Rule when One is nullable and other not and is same type. like:
    ///     int Id -> int? Id
    /// </summary>
    public class NullableRuleForSameTypeWhenOneIsNullable : IRuleMapper, IRuleNeedLocalField
    {
        /// <summary>
        /// Initialize a new instance of <see cref="NullableRuleForSameTypeWhenOneIsNullable"/>
        /// </summary>
        /// <param name="source">The source member</param>
        /// <param name="destiny">The source member</param>
        public NullableRuleForSameTypeWhenOneIsNullable(MemberInfo source, MemberInfo destiny)
        {
            SourceProperty = source as PropertyInfo ?? throw new ArgumentNullException(nameof(source));
            DestinyProperty = destiny as PropertyInfo ?? throw new ArgumentNullException(nameof(destiny));

            if (SourceProperty.PropertyType.IsNullable())
            {
                LocalFields = new []{ SourceProperty.PropertyType };
            }
        }

        /// <summary>
        /// Property Destiny
        /// </summary>
        public PropertyInfo DestinyProperty { get; }

        /// <summary>
        /// Property Source
        /// </summary>
        public PropertyInfo? SourceProperty { get; }

        /// <summary>
        /// Execute rule
        /// </summary>
        /// <param name="il">The <see cref="ILGenerator"/> that generate method  </param>
        /// <param name="fields">The <see cref="IReadOnlyDictionary{TKey, TValue}"/> that have all field in class that was already added.</param>
        /// <param name="localFields">The <see cref="IReadOnlyDictionary{TKey, TValue}"/> that have all locals field that was already added.</param>
        /// <param name="mapperProperties">The <see cref="IEnumerable{MapperProperty}"/> that have map.</param>
        public void Execute(ILGenerator il, IReadOnlyDictionary<string, FieldBuilder> fields, IReadOnlyDictionary<Type, LocalBuilder> localFields, IEnumerable<MapperProperty> mapperProperties)
        {
            if (DestinyProperty.PropertyType.IsNullable())
            {
                // based on https://sharplab.io/#v2:C4LglgNgPgAgTARgLACgYGYAE9MGFMiYCSAsgIYAOmA3qpvdlgMpgC2FEApgEKbkUAKFuy4BBTAGcA9gFcATgGNOAShp0GGmAHZMAO04B3TMI491G+rRQWbxACaYAvJNmLOAOiJ2ANOYsBfAG4/f1RQlFRUDEwwXWBOOQAzMiViflQrDRMuXn4hNlNxaXklZWCUcKisHGzOUQy/aNjgexpMAHNOYEDJLp7wyrRquGMCnIbrBia4gH5W6g6+3u7MAaA==
                // public class Map
                // {
                //      public SimpleB Map(SimpleA source)
                //      {
                //          return new SimpleB
                //          {
                //              Int = source.Int // This mapper is create this line
                //          };
                //      }
                // }
                // public class SimpleA
                // {
                //      public int Int { get; set;}
                // }
                // public class SimpleB
                // {
                //      public int? Int { get; set;}
                // }
                GenerateMapWithDestinyAsNullable(il);
            }
            else
            {
                // based on https://sharplab.io/#v2:C4LglgNgPgAgTARgLACgYGYAE9MGFMiYCSAsgIYAOmA3qpvdlgMpgC2FEApgEKbkUAKFuy4BBTAGcA9gFcATgGNOAShp0GGmAHZMAO04B3TMI491G+rRQWbxACaYAvJNmLOAOiIOA/N8x3OADMyGQhgABpzCwBfAG4o6NRElFRUDEwwXWBOOWClYn5UKw0TLl5+ITZTcWl5JWV4lGS0rBxSzlEiqPTM4D8vGkwAc05gWMlR8eTmtFa4YyqyrusGHqz7QZGxie3poA===
                // public class Map
                // {
                //      public SimpleB Map(SimpleA source)
                //      {
                //          return new SimpleB
                //          {
                //              Int = source.Int ?? default // This mapper is create this line
                //          };
                //      }
                // }
                // public class SimpleA
                // {
                //      public int? Int { get; set;}
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
            var constructor = typeof(Nullable<>).MakeGenericType(Nullable.GetUnderlyingType(DestinyProperty.PropertyType))
                .GetConstructors()[0];
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, SourceProperty!.GetMethod, null);
            il.Emit(OpCodes.Newobj, constructor);
            il.EmitCall(OpCodes.Callvirt, DestinyProperty.SetMethod, null);
        }


        private void GenerateMapWithDestinyAsNotNullable(ILGenerator il, IReadOnlyDictionary<Type, LocalBuilder> localField)
        {
            var getValueOrDefault = SourceProperty!.PropertyType.GetMethods().First(x => x.Name == "GetValueOrDefault" && x.GetParameters().Length == 0);
            var field = localField[SourceProperty.PropertyType];
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, SourceProperty.GetMethod, null);
            il.Emit(OpCodes.Stloc_S, field.LocalIndex);
            il.Emit(OpCodes.Ldloca_S, field.LocalIndex);
            il.EmitCall(OpCodes.Call, getValueOrDefault, null);
            il.EmitCall(OpCodes.Callvirt, DestinyProperty.SetMethod, null);
        }

        public IEnumerable<Type> LocalFields { get; }
    }
}
