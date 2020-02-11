using System;
using System.Collections.Generic;

namespace CastForm.Rules
{
    /// <summary>
    /// If Rule need a local field
    /// </summary>
    public interface IRuleNeedLocalField
    {
        /// <summary>
        /// Local filed used in Method
        /// </summary>
        IEnumerable<Type> LocalFields { get; }
    }
}
