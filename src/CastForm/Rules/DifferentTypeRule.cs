using System;
using System.Collections.Generic;
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
            Field = typeof(IMap<,>).MakeGenericType(_source.PropertyType, _destiny.PropertyType);
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

        public Type Field { get; }
    }
}
