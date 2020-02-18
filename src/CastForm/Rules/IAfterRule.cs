using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CastForm.Rules
{
    public interface IAfterRule
    {
        void Execute(ILGenerator il, IReadOnlyDictionary<string, FieldBuilder> fields, IReadOnlyDictionary<Type, LocalBuilder> localFields);
    }
}
