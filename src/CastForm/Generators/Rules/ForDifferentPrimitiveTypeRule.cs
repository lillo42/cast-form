using System;
using Microsoft.CodeAnalysis;

namespace CastForm.Generators.Rules
{
    internal class ForDifferentPrimitiveTypeRule : IRule
    {
        public ForDifferentPrimitiveTypeRule(IPropertySymbol destiny, IPropertySymbol source)
        {
            Source = source;
            Destiny = destiny;
        }
        
        public IPropertySymbol Source { get; }
        public IPropertySymbol Destiny { get; }

        public void Apply(MapperBuilder builder)
        {
            builder.AppendWithTab("destiny.").Append(Destiny.Name).Append(" = Convert.To")
                .Append(Map(Destiny.Type))
                .Append("(source.").Append(Source.Name).AppendLine(");");

            static string Map(ITypeSymbol type)
            {
                return type.SpecialType switch
                {
                    SpecialType.System_Boolean => "Boolean",
                    SpecialType.System_Char => "Char",
                    SpecialType.System_SByte => "SByte",
                    SpecialType.System_Byte => "Byte",
                    SpecialType.System_Int16 => "Int16",
                    SpecialType.System_UInt16 => "UInt16",
                    SpecialType.System_Int32 => "Int32",
                    SpecialType.System_UInt32 => "UInt32",
                    SpecialType.System_Int64 => "Int64",
                    SpecialType.System_UInt64 => "UInt64",
                    SpecialType.System_Decimal => "Decimal",
                    SpecialType.System_Single => "Single",
                    SpecialType.System_Double => "Double",
                    SpecialType.System_String => "String",
                    SpecialType.System_DateTime => "DateTime",
                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                };
            }
        }
    }
}
