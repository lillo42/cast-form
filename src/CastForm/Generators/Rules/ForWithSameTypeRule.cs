using System.Text;
using CastForm.Generators.Rules;
using Microsoft.CodeAnalysis;

namespace CastForm.Generators
{
    internal class ForWithSameTypeRule : IRule
    {
        public ForWithSameTypeRule(IPropertySymbol destiny, IPropertySymbol source)
        {
            Source = source;
            Destiny = destiny;
        }
        
        public IPropertySymbol Source { get; }
        public IPropertySymbol Destiny { get; }
        
        public void Apply(StringBuilder classBuilder)
        {
            classBuilder.AppendLine($"destiny.{Destiny.Name} = source.{Source.Name};");
        }
    }
}
