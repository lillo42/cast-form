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
        public IgnoreRule(MemberInfo property)
        {
            DestinyProperty = (property as PropertyInfo)?? throw new ArgumentNullException(nameof(property));
        }

        public PropertyInfo DestinyProperty { get; }

        public PropertyInfo? SourceProperty => null;

        public void Execute(ILGenerator il, IReadOnlyDictionary<string, FieldBuilder> fields, IReadOnlyDictionary<Type, LocalBuilder> localFields)
        {
            
        }
    }
}
