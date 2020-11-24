using Microsoft.CodeAnalysis;

namespace CastForm.Generators.Rules.For
{
    internal class ForEqualPrimitiveTypeRule : IRule
    {
        public ForEqualPrimitiveTypeRule(IPropertySymbol destiny, IPropertySymbol source)
        {
            Source = source;
            Destiny = destiny;
        }
        
        public IPropertySymbol Source { get; }
        public IPropertySymbol Destiny { get; }
        
        public void Apply(MapperBuilder builder)
        {
            builder.AppendWithTab("destiny.")
                .Append(Destiny.Name)
                .Append(" = source.")
                .Append(Source.Name)
                .AppendLine(";");
        }
    }
}
