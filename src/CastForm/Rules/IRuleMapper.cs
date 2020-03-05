using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace CastForm.Rules
{
    /// <summary>
    /// Rule for property
    /// </summary>
    public interface IRuleMapper
    {

        /// <summary>
        /// Property Destiny
        /// </summary>
        PropertyInfo DestinyProperty { get; }

        /// <summary>
        /// Property Source
        /// </summary>
        PropertyInfo? SourceProperty { get; }

        /// <summary>
        /// If that rule can be applied
        /// </summary>
        /// <param name="property">Property to be match</param>
        /// <returns></returns>
        bool Match(PropertyInfo property)
            => DestinyProperty.Equals(property);

        /// <summary>
        /// Execute rule
        /// </summary>
        /// <param name="il">The <see cref="ILGenerator"/> that generate method  </param>
        /// <param name="fields">The <see cref="IReadOnlyDictionary{TKey, TValue}"/> that have all field in class that was already added.</param>
        /// <param name="localFields">The <see cref="IReadOnlyDictionary{TKey, TValue}"/> that have all locals field that was already added.</param>
        /// <param name="mapperProperties">The <see cref="IEnumerable{MapperProperty}"/> that have map.</param>
        void Execute(ILGenerator il, IReadOnlyDictionary<string, FieldBuilder> fields, IReadOnlyDictionary<Type, LocalBuilder> localFields, IEnumerable<MapperProperty> mapperProperties);
    }
}
