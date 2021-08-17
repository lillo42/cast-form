using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace CastForm.Generators
{
    [Generator]
    public class CastFormSourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new CastFormReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxReceiver is CastFormReceiver receiver) || receiver.CandidateClasses.Count == 0)
            {
                return;
            }

            var parse = new Parse(context);
            foreach (var specification in receiver.CandidateClasses
                .Select(classDeclarationSyntax => parse.CreateMapSpecification(classDeclarationSyntax))
                .SelectMany(specifications => specifications))
            {
                Console.WriteLine(specification.ToString());
            }
        }
    }
}
