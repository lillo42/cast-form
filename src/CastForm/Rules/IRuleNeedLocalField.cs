using System;
using System.Collections.Generic;

namespace CastForm.Rules
{
    public interface IRuleNeedLocalField
    {
        IEnumerable<Type> LocalField { get; }
    }
}
