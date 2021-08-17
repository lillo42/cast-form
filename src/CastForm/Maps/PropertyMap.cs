using Microsoft.CodeAnalysis;

namespace CastForm.Maps
{
    internal class PropertyMap
    {
        public PropertyMap(IPropertySymbol destiny, 
            IPropertySymbol? source,
            Rule rule)
        {
            Source = source;
            Rule = rule;
            Destiny = destiny;
        }
        
        public IPropertySymbol Destiny { get; }
        public IPropertySymbol? Source { get; }
        public Rule Rule { get; }
    }
}
