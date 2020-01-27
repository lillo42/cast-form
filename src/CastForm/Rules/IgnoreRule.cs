using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using CastForm.Generator;

namespace CastForm.Rules
{
    public class IgnoreRule : IRuleMapper
    {
        private readonly MemberInfo _property;

        public IgnoreRule(MemberInfo property)
        {
            _property = property ?? throw new ArgumentNullException(nameof(property));
        }

        public bool Match(PropertyInfo property) 
            => property.Equals(_property);

        public void Execute(ILGenerator il, FieldMapper local)
        {
            
        }
    }
}
