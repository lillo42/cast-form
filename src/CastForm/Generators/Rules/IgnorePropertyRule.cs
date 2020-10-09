using Microsoft.CodeAnalysis;

namespace CastForm.Generators.Rules
{
    internal class IgnorePropertyRule : IRule
    {
        public IgnorePropertyRule(IPropertySymbol destiny)
        {
            Destiny = destiny;
        }

        public IPropertySymbol Destiny { get; }
        public void Apply(MapperBuilder builder) { }
    }
}
