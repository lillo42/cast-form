using System;
using System.Linq;
using System.Threading.Tasks;
using CastForm.Generators.Rules;
using CastForm.Generators.Rules.For;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;

namespace CastForm.Generators.Factories
{
    internal static class RuleSyntaxFactory
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
                var sourceProperty = GetProperty(source, node.ArgumentList.Arguments[1]);
                
                if (SymbolEqualityComparer.Default.Equals(sourceProperty.Type, destinyProperty.Type))
                {
                    return new ForEqualPrimitiveTypeRule(destinyProperty, sourceProperty);
                }
                
                if (IsPrimitiveType(sourceProperty) && IsPrimitiveType(destinyProperty))
                {
                    return new ForDifferentPrimitiveTypeRule(destinyProperty, sourceProperty);
                }
                
                if (IsString(sourceProperty) && IsSpecialType(destinyProperty))
                {
                    return new ForDifferentPrimitiveTypeRule(destinyProperty, sourceProperty);
                }
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

        private static bool IsPrimitiveType(IPropertySymbol property)
        {
            var type = property.Type;
            
            return type.SpecialType == SpecialType.System_Boolean
                   || type.SpecialType == SpecialType.System_Byte
                   || type.SpecialType == SpecialType.System_Char
                   || type.SpecialType == SpecialType.System_String
                   || type.SpecialType == SpecialType.System_Int16
                   || type.SpecialType == SpecialType.System_Int32
                   || type.SpecialType == SpecialType.System_Int64
                   || type.SpecialType == SpecialType.System_Single
                   || type.SpecialType == SpecialType.System_DateTime
                   || type.SpecialType == SpecialType.System_Double
                   || type.SpecialType == SpecialType.System_Decimal
                   || type.SpecialType == SpecialType.System_SByte
                   || type.SpecialType == SpecialType.System_UInt16
                   || type.SpecialType == SpecialType.System_UInt32
                   || type.SpecialType == SpecialType.System_UInt64;
        }

        private static bool IsSpecialType(IPropertySymbol property)
        {
            return property.Type.IsValueType
                   && (property.Type.Name == nameof(Guid) || property.Type.Name == nameof(TimeSpan));
        }
        
        private static bool IsString(IPropertySymbol property)
        {
            return property.Type.IsValueType 
                   && property.Type.SpecialType ==  SpecialType.System_String;
        }
        

        private const string For = "For";
        private const string Ignore = "Ignore";
    }
}
