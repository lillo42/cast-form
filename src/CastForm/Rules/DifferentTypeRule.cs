﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace CastForm.Rules
{
    /// <summary>
    /// Rule for map property when the are different and there are not .NET Type(int, long, decimal, DateTime e etc.)
    /// </summary>
    public class DifferentTypeRule : IRuleMapper, IRuleNeedField
    {
        // based on https://sharplab.io/#v2:C4LglgNgPgAgTARgLACgYGYAE9MGFMiYCSAsgIYAOAPAMpgC2FEApgIIA0mdjLAQgHyYA3qkxjMFAE5gAbmWDNMk5mQAmAewB2EAJ7Fy1bk2a5ORlgBFBAfQDGAFXUWA3KPFuxGLg2O9MBgApzNkwAZ3UAV0lbZgBKYQ9xTwB2TE1mAHdvHmZeRKSRFCTi8Rp1emZgAAswTQBzTABeTDtHCwA6QPComPayiurautj2fPEAX1ciidRx1HmUVC9ahUkAMzIY/UpaditUQqSLf0ogsMjouKm5xbQsHGDWA8SvYPx+ypr64Uw6yucwv9MDcbkt7nBsr5ntNPFhgscPoNvkJfkDQkCQQsvA8fCxcNCkstNMBiKofn9gAD0ZTgS97ggAAyYezMAAeJJRFKpGLpmAARup1BBiKEAKKaMh8ljktE8lCgu7YCHwgniLwQLQNIhkzmymk3Qn0pks9kymnUgEgoA===
        private readonly PropertyInfo _source;
        private readonly PropertyInfo _destiny;

        public DifferentTypeRule(MemberInfo source, MemberInfo destiny)
        {
            _source = source as PropertyInfo ?? throw new ArgumentNullException(nameof(source));
            _destiny = destiny as PropertyInfo ?? throw new ArgumentNullException(nameof(destiny));
        }

        public bool Match(PropertyInfo property)
            => _destiny.Equals(property);

        public void Execute(ILGenerator il, IReadOnlyDictionary<Type, FieldBuilder> fields, IReadOnlyDictionary<Type, LocalBuilder> localFields)
        {
            var builder = fields[Field];

            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, builder);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, _source.GetMethod, null);
            il.EmitCall(OpCodes.Callvirt, Field.GetMethod("Map"), null);
            il.EmitCall(OpCodes.Callvirt, _destiny.SetMethod, null);
        }

        public Type Field
        {
            get
            {

                var sourceType = _source.PropertyType;
                if (sourceType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                {
                    sourceType = typeof(IEnumerable<>).MakeGenericType(sourceType.GetGenericArguments()[0]);
                }

                return typeof(IMap<,>).MakeGenericType(sourceType, _destiny.PropertyType);
            }
        }
    }
}
