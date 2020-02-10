using System;

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
        Type Field { get; }
    }
}
