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
        /// <summary>
        /// Initialize a new instance of <see cref="IgnoreRule"/>
        /// </summary>
        /// <param name="property">The property to be ignore.</param>
        public IgnoreRule(MemberInfo property)
        {
            DestinyProperty = (property as PropertyInfo)?? throw new ArgumentNullException(nameof(property));
        }

        /// <inheritdoc/>
        public PropertyInfo DestinyProperty { get; }

        /// <inheritdoc/>
        public PropertyInfo? SourceProperty => null;

        /// <inheritdoc/>
        public void Execute(ILGenerator il, IReadOnlyDictionary<string, FieldBuilder> fields, IReadOnlyDictionary<Type, LocalBuilder> localFields, IEnumerable<MapperProperty> mapperProperties)
        {
            
        }
    }
}
