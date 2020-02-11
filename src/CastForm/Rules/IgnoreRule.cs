using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace CastForm.Rules
{
    /// <summary>
    /// Ignore Property
    /// </summary>
    public class IgnoreRule : IRuleMapper
    {
        private readonly MemberInfo _property;

        public IgnoreRule(MemberInfo property)
        {
            _property = property ?? throw new ArgumentNullException(nameof(property));
        }

        public bool Match(PropertyInfo property) 
            => property.Equals(_property);

        public void Execute(ILGenerator il, IReadOnlyDictionary<Type, FieldBuilder> fields, IReadOnlyDictionary<Type, LocalBuilder> localFields)
        {
            
        }
    }
}
