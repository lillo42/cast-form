using System;
using System.Linq;
using System.Threading.Tasks;
using CastForm.Generators.Rules;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;

namespace CastForm.Generators.Factories
{
    internal class RuleSyntaxFactory
    {
        public static async Task<IRule> CreateAsync(Compilation compilation,
            MemberAccessExpressionSyntax memberAccessExpressionSyntax,
            InvocationExpressionSyntax node)
        {
            var methodModel = compilation.GetSemanticModel(memberAccessExpressionSyntax.Name.SyntaxTree);
            var method = ((await SymbolFinder
                    .FindSymbolAtPositionAsync(methodModel, memberAccessExpressionSyntax.Name.SpanStart,
                        new AdhocWorkspace()))
                as IMethodSymbol)!;

            var source = method.ContainingType.TypeArguments[0];
            var destiny = method.ContainingType.TypeArguments[1];

            var destinyProperty = GetProperty(destiny, node.ArgumentList.Arguments[0]);
            if (method.Name == For)
            {
                return new ForWithSameTypeRule(destinyProperty, GetProperty(source, node.ArgumentList.Arguments[1]));
            }

            if (method.Name == Ignore)
            {
                return new IgnorePropertyRule(destinyProperty);
            }

            throw new Exception();
        }

        private static IPropertySymbol GetProperty(INamespaceOrTypeSymbol type,  ArgumentSyntax argumentSyntax)
        {
            if (argumentSyntax.Expression is SimpleLambdaExpressionSyntax lambda)
            {
                if (lambda.Body is MemberAccessExpressionSyntax member)
                {
                    return (type.GetMembers()
                        .First(x => x is IPropertySymbol 
                                    && x.Name == member.Name.ToFullString()) as IPropertySymbol)!;
                }

            }

            throw new Exception();
        }


        private const string For = "For";
        private const string Ignore = "Ignore";
    }
}
