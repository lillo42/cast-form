using System.Text;
using Microsoft.CodeAnalysis;

namespace CastForm.Generators.Rules
{
    internal interface IRule
    {
        IPropertySymbol Destiny { get; }

        void Apply(StringBuilder classBuilder);
    }
}
