using System.Collections.Generic;
using CastForm.Generators.Factories;
using CastForm.Generators.Rules;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CastForm.Generators
{
    internal class MapperWalker : CSharpSyntaxWalker
    {
        private readonly Compilation _compilation;
        public HashSet<MapperSyntax> Mappers { get; } = new HashSet<MapperSyntax>(); 
        private readonly List<IRule> _rules = new List<IRule>();
            
        public MapperWalker(Compilation compilation)
            : base(SyntaxWalkerDepth.StructuredTrivia)
        {
            _compilation = compilation;
        }
            
        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            switch (node.Expression)
            {
                case GenericNameSyntax genericNameSyntax:
                {
                    var generic = genericNameSyntax.TypeArgumentList;
                    var mapper = new MapperSyntax(generic.Arguments[0], generic.Arguments[1]);
                    Mappers.Add(mapper);
                    mapper.Rules.AddRange(_rules);
                    _rules.Clear();
                    break;
                }
                case MemberAccessExpressionSyntax memberAccessExpressionSyntax:
                    _rules.Add(RuleSyntaxFactory
                        .CreateAsync(_compilation, memberAccessExpressionSyntax, node)
                        .GetAwaiter().GetResult());
                    // _rules.Add(new RuleSyntax(memberAccessExpressionSyntax,
                    //     memberAccessExpressionSyntax.Name,
                    //     node.ArgumentList.Arguments[0], node.ArgumentList.Arguments[1]));
                    break;
            }

            base.VisitInvocationExpression(node);
        }
    }
}
