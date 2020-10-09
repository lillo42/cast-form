using Microsoft.CodeAnalysis;

namespace CastForm.Generators.Rules
{
    internal interface IRule
    {
        IPropertySymbol Destiny { get; }

        void Apply(MapperBuilder builder);
    }
}
