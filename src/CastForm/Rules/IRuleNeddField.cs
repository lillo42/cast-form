using System;
using System.Collections.Generic;

namespace CastForm.Rules
{
    /// <summary>
    /// If Rule need a field, in the class
    /// </summary>
    public interface IRuleNeedField
    {
        /// <summary>
        /// Field use in class
        /// </summary>
        IEnumerable<(string name, Type type)>  Fields { get; }
    }
}
