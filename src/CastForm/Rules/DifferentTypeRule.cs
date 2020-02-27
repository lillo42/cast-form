using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CastForm.Generator;

namespace CastForm.Rules
{
    /// <summary>
    /// Rule for map property when the are different and there are not .NET Type(int, long, decimal, DateTime e etc.)
    /// </summary>
    public class DifferentTypeRule : IRuleMapper, IRuleNeedField
    {
        // based on https://sharplab.io/#v2:C4LglgNgPgAgTARgLACgYGYAE9MGFMiYCSAsgIYAOAPAMpgC2FEApgIIA0mdjLAQgHyYA3qkxjMFAE5gAbmWDNMk5mQAmAewB2EAJ7Fy1bk2a5ORlgBFBAfQDGAFXUWA3KPFuxGLg2O9MBgApzNkwAZ3UAV0lbZgBKYQ9xTwB2TE1mAHdvHmZeRKSRFCTi8Rp1emZgAAswTQBzTABeTDtHCwA6QPComPayiurautj2fPEAX1ciidRx1HmUVC9ahUkAMzIY/UpaditUQqSLf0ogsMjouKm5xbQsHGDWA8SvYPx+ypr64Uw6yucwv9MDcbkt7nBsr5ntNPFhgscPoNvkJfkDQkCQQsvA8fCxcNCkstNMBiKofn9gAD0ZTgS97ggAAyYezMAAeJJRFKpGLpmAARup1BBiKEAKKaMh8ljktE8lCgu7YCHwgniLwQLQNIhkzmymk3Qn0pks9kymnUgEgoA===

        private readonly string _mapName;
        public DifferentTypeRule(MemberInfo source, MemberInfo destiny)
        {
            SourceProperty = source as PropertyInfo ?? throw new ArgumentNullException(nameof(source));
            DestinyProperty = destiny as PropertyInfo ?? throw new ArgumentNullException(nameof(destiny));

            _mapName = $"_map{SourceProperty.Name}To{DestinyProperty.Name}";
            var fields = new LinkedList<(string, Type)>();
            var sourceType = SourceProperty!.PropertyType;
            if (sourceType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                sourceType = typeof(IEnumerable<>).MakeGenericType(sourceType.GetGenericArguments()[0]);
            }

            fields.AddLast((_mapName, typeof(IMap<,>).MakeGenericType(sourceType, DestinyProperty.PropertyType)));
            fields.AddLast(("_counter", typeof(Counter))); 
            Fields = fields;
        }

        public PropertyInfo DestinyProperty { get; }
        public PropertyInfo? SourceProperty { get; }

        public void Execute(ILGenerator il, IReadOnlyDictionary<string, FieldBuilder> fields, IReadOnlyDictionary<Type, LocalBuilder> localFields, IEnumerable<MapperProperty> mapperProperties)
        {
            var builder = fields[_mapName];
            var mapMethod = builder.FieldType.GetMethod("Map");
            
            il.Emit(OpCodes.Dup);
            var mapper = new MapperProperty(DestinyProperty.DeclaringType, DestinyProperty, SourceProperty!.DeclaringType, SourceProperty);
            if (mapperProperties.Count(x => x.GetHashCode() == mapper.GetHashCode()) > 1)
            {
                // https://sharplab.io/#v2:C4LglgNgPgAgTARgLACgYAYAEMEDoDCA9hBAKYDGwYhAdgM4G3kCuATq6TcANyqowBmbHExFmXUq1QBvVJnmYADqzAA3AIbBSmDuoAmtCAE9MAJXU0DAW0wB9VheuYAvJhqkA7mceErACgBKXhQFJRUNLR1SfUMTIhoWdk5gABEwSmoadVYjAB4wLgAaTALgAD47K3VFRUkXN09RJjYOLjSM2my80uLSssDguQVBEq5MAHFSYDEJVj9SzAALdTpFoj1SAKH5WRDQhVsqmslcABUcyeAANXUIZlI/ZdX10mLCZmBMDVZMcnfZoLbfaHaq1VgAbSea0IGwAuvUANQIv7iLSsQZ7fbYADsv3+aOCoQAvnxMdghDAACyiMgWeZjKEvAIuCog46sM45UykKyEVQPRkw16Yd6fWyAlBAkYLS7mSy+QJA0LOVkOeVWXAAOVIAA9gANUCTJWgKSIALKgyQAQVOhAAQpgQJgAJIWxS5ADKYCsijIVuKXp9ZDtZRkUqEruqnu9vtIdoDMb9FTttqthIUyjUmm0ugMNGMTVRdVsKNm6fkmYi2hwADYojF8yYFrYAEYrUgACRW0I29S7zyFjCsLYKD2ARlqhAAZn5A7GrQFiuPJzO58GAhLQiM13HMG7Z4nSFbMHR3qxyJsgbssfJviezxf+z3tK4ny8hyP3H5W+230LipM7gOFoeh/hsuCXGBDynmwF4bhiN5fNkUR0MwECfK47heDudpKgo16IQoHq+FMiwFAA5iIrgwee2gAISYWhECYAAZCxdilmiEFTDMaJ+DRj7dkymC5JgQgAPyYCmhBWrg+4CaQuDEVYpEUcyTo0ExhR4fIRIITeJb4ic+C0jQ/EPp2QlChKOk4ihaE8ECRpGvwFIIHW8ATJwkjZqBVkbGGZIjLWoyfJB/kPDux50IeC62SqmBQR+o78bF4m4M6egSi5xrSrMU7qBeLpup6xQpKGKAEfIKR7tUs73rBmzBDlwUiFFgVbkICyZZg0iYORUzcCeg2YEanWYDhE0kcAZE0ORvX9SNdAjc5pKtRNh64ZV4ahS6egLQNPDDUdY3DEIUVTSpM0USIfWHUNy0nYaQA=
                var counter = fields["_counter"];
                var getCounter = typeof(Counter).GetMethod(nameof(Counter.GetCounter));
                var hashCode = HashCodeFactoryGenerator.Instance.Type.GetMethod("GenHashCode", new[] { SourceProperty!.DeclaringType });

                var returnNull = il.DefineLabel();
                var canMap = il.DefineLabel();
                var setProperty = il.DefineLabel();

                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Brfalse_S, returnNull);

                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, counter);
                il.Emit(OpCodes.Ldarg_1);
                il.EmitCall(OpCodes.Call, hashCode, null);
                il.EmitCall(OpCodes.Callvirt, getCounter, null);
                il.Emit(OpCodes.Ldc_I4_S, 3);
                il.Emit(OpCodes.Blt_S, canMap);

                il.MarkLabel(returnNull);
                il.Emit(OpCodes.Ldnull);
                il.Emit(OpCodes.Br_S, setProperty);

                il.MarkLabel(canMap);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, builder);
                il.Emit(OpCodes.Ldarg_1);
                il.EmitCall(OpCodes.Callvirt, SourceProperty!.GetMethod, null);
                il.EmitCall(OpCodes.Callvirt, mapMethod, null);

                il.MarkLabel(setProperty);
                il.EmitCall(OpCodes.Callvirt, DestinyProperty.SetMethod, null);
            }
            else
            {
                // https://sharplab.io/#v2:C4LglgNgPgAgTARgLACgYGYAE9MGFMiYCSAsgIYAOAPAMpgC2FEApgIIA0mdjLAQgHyYA3qkxjMFAE5gAbmWDNMk5mQAmAewB2EAJ7Fy1bk2a5ORlgBFBAfQDGAFXUWA3KPFuxGLg2O9MBgApzNkwAZ3UAV0lbZgBKYQ9xTwB2TE1mAHdvHmZeRKSRFCTi8Rp1emZgAAswTQBzTABeTDtHCwA6QPComPayiurautj2fPEAX1ciidRx1HmUVC9ahUkAMzIY/UpaditUQqSLf0ogsMjouKm5xbQsHGDWA8SvYPx+ypr64Uw6yucwv9MDcbkt7nBsr5ntNPFhgscPoNvkJfkDQkCQQsvA8fCxcNCkstNMBiKofn9gAD0ZTgS97ggAAyYezMAAeJJRFKpGLpmAARup1BBiKEAKKaMh8ljktE8lCgu7YCHwgniLwQLQNIhkzmymk3Qn0pks9kymnUgEgoA===
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, builder);
                il.Emit(OpCodes.Ldarg_1);
                il.EmitCall(OpCodes.Callvirt, SourceProperty!.GetMethod, null);
                il.EmitCall(OpCodes.Callvirt, mapMethod, null);
                il.EmitCall(OpCodes.Callvirt, DestinyProperty.SetMethod, null);
            }
            
        }

        public IEnumerable<(string name, Type type)> Fields { get; }
    }
}
