using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace CastForm.Rules
{
    public class DifferentTypeRule : IRuleMapper, IRuleNeedField
    {
        private readonly PropertyInfo _source;
        private readonly PropertyInfo _destiny;

        public DifferentTypeRule(MemberInfo source, MemberInfo destiny)
        {
            _source = source as PropertyInfo ?? throw new ArgumentNullException(nameof(source));
            _destiny = destiny as PropertyInfo ?? throw new ArgumentNullException(nameof(destiny));
        }

        public bool Match(PropertyInfo property)
            => _source.Equals(property);

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
