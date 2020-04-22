using System;
using System.Collections.Generic;
using CastForm.Rules;

namespace CastForm
{
    /// <summary>
    /// Generate <see cref="IMap"/> for define source and destiny.
    /// </summary>
    public interface IMapperGenerator
    {
        /// <summary>
        /// Build  <see cref="IMap"/>
        /// </summary>
        /// <returns>Generated <see cref="IMap"/></returns>
        Type Generate(IEnumerable<MapperProperty> mapperProperties);

        /// <summary>
        /// Create rules for source and destiny properties.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IRuleMapper> CreateRules();
    }
}
