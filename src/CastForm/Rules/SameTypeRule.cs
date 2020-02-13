using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace CastForm.Rules
{
    /// <summary>
    /// Set when the type math
    /// </summary>
    public class SameTypeRule : IRuleMapper
    {
        // based on https://sharplab.io/#v2:C4LglgNgPgAgTARgLACgYGYAE9MGFMiYCSAsgIYAOmA3qpvdlgMpgC2FEApgEKbkUAKFuy4BBTAGcA9gFcATgGNOAShp0GGmAHZMAO04B3TMI491G+rRQWbxACaYAvJNmLOAOiJ2ANOdv0AFU4AD2AnF3kldyDQvw0AXwBuP3jUVJRUVAxMMF1gTjkAMzIlYn5UKw0TLl5+ITZTcWlIlWSUdKysHGrOUQq/bNywrxpMAHNOYETJSen0zS6EAAZMGLDqcdmZqcx5hmyAIykpCGIJAFFdMgOuUYmdiS30jrQuuGMGmv7rfawh+zuW0eOz29GyMGWqxC602DyeaSAA=
        private readonly PropertyInfo _source;
        private readonly PropertyInfo _destiny;

        public SameTypeRule(MemberInfo source, MemberInfo destiny)
        {
            _source = source as PropertyInfo ?? throw new ArgumentNullException(nameof(source));
            _destiny = destiny as PropertyInfo ?? throw new ArgumentNullException(nameof(destiny));
        }

        public bool Match(PropertyInfo property) 
            => _destiny.Equals(property);

        public void Execute(ILGenerator il, IReadOnlyDictionary<Type, FieldBuilder> fields, IReadOnlyDictionary<Type, LocalBuilder> localFields)
            => Execute(il, _source, _destiny);

        internal static void Execute(ILGenerator il, PropertyInfo source, PropertyInfo destiny)
        {
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, source.GetMethod, null);
            il.EmitCall(OpCodes.Callvirt, destiny.SetMethod, null);
        }
    }
}
