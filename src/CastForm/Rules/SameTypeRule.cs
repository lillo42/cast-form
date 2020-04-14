using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace CastForm.Rules
{
    /// <summary>
    /// Rule for map property when the are same type.
    /// </summary>
    public class SameTypeRule : IRuleMapper
    {
        /// <summary>
        /// Initialize a new instance of <see cref="SameTypeRule"/>.
        /// </summary>
        /// <param name="source">The source member</param>
        /// <param name="destiny">The source member</param>
        public SameTypeRule(MemberInfo source, MemberInfo destiny)
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

        internal static void Execute(ILGenerator il, PropertyInfo source, PropertyInfo destiny)
        {
            // based on https://sharplab.io/#v2:C4LglgNgPgAgTARgLACgYGYAE9MGFMiYCSAsgIYAOmA3qpvdlgMpgC2FEApgEKbkUAKFuy4BBTAGcA9gFcATgGNOAShp0GGmAHZMAO04B3TMI491G+rRQWbxACaYAvHim6AbpznAAdABUpRLrA6HAC0vJK3kR2ygA05rb0vpwAHsBOkrKKnH6pwAkaAL4A3AmFqOUoqKgYmGBBngBmZErE/KhWGiZcvPxCbKbi4dnKpSiVNVg43ZyiHQm1EK4A5vY0mMucwMWSWzuVmlMIAAyYyWnrm9u71wcMtQBGUlIQxBIAorpkD1yXezf7CrVNBTODGAY9ebWe5YerpaJ/a4Sf53ei1GAnM55RE7ZG3CpAA=
            // public class Map
            // {
            //      public SimpleB Map(SimpleA source)
            //      {
            //          return new SimpleB
            //          {
            //              Int = Int // This mapper is create this line
            //          };
            //      }
            // }
            // public class SimpleA
            // {
            //      public int Int { get; set;}
            // }
            // public class SimpleB
            // {
            //      public int Int { get; set;}
            // }

            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, source.GetMethod, null);
            il.EmitCall(OpCodes.Callvirt, destiny.SetMethod, null);
        }
    }
}
