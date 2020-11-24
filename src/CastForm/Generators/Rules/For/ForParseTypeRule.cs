using System;
using Microsoft.CodeAnalysis;

namespace CastForm.Generators.Rules.For
{
    internal class ForParseTypeRule  : IRule
    {
        public ForParseTypeRule(IPropertySymbol source, IPropertySymbol destiny)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Destiny = destiny ?? throw new ArgumentNullException(nameof(destiny));
        }

        public IPropertySymbol Source { get; }
        public IPropertySymbol Destiny { get; }
        public void Apply(MapperBuilder builder)
        {
            builder.AddUsing("System");
            
            builder.AppendWithTab("destiny.").Append(Destiny.Name).Append(" = ")
                .Append(Destiny.Type.Name)
                .Append(".Parse(").Append(Source.Name).AppendLine(");");
        }
    }
}
