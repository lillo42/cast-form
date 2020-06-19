using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CastForm.Generators
{
    internal class MapperReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> CandidateClass { get; } = new List<ClassDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclaration)
            {
                CandidateClass.Add(classDeclaration);
            }
        }
    }
}
