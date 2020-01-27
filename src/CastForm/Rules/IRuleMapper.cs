using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using CastForm.Generator;

namespace CastForm
{
    public interface IRuleMapper
    {
        bool Match(PropertyInfo property);

        void Execute(ILGenerator il, FieldMapper local);
    }
}
