using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CastForm.Generator
{
    public class FieldMapper
    {
        public FieldMapper(IReadOnlyDictionary<Type, FieldBuilder> fields,
            IReadOnlyDictionary<Type, LocalBuilder> localFields)
        {
            Fields = fields ?? throw new ArgumentNullException(nameof(fields));
            LocalFields = localFields ?? throw new ArgumentNullException(nameof(localFields));
        }

        public IReadOnlyDictionary<Type, FieldBuilder> Fields { get; }
        public IReadOnlyDictionary<Type, LocalBuilder> LocalFields { get; set; }
    }
}
