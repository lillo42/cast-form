using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace CastForm
{
    /// <summary>
    /// Rule for property
    /// </summary>
    public interface IRuleMapper
    {
        /// <summary>
        /// If that rule can be applied
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        bool Match(PropertyInfo property);

        /// <summary>
        /// Execute rule
        /// </summary>
        /// <param name="il"></param>
        /// <param name="fields"></param>
        /// <param name="localFields"></param>
        void Execute(ILGenerator il, IReadOnlyDictionary<Type, FieldBuilder> fields, IReadOnlyDictionary<Type, LocalBuilder> localFields);
    }
}
