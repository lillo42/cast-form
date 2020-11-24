using System.Collections.Generic;
using System.Linq;
using System.Text;
using CastForm.Generators.Rules;
using CastForm.Generators.Rules.For;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.Text;

namespace CastForm.Generators
{
    /// <summary>
    /// Mapper Generator
    /// </summary>
    [Generator]
    public class MapperGenerator : ISourceGenerator
    {
        /// <inheritdoc />
        public void Initialize(InitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new MapperReceiver());
        }

        /// <inheritdoc />
        public void Execute(SourceGeneratorContext context)
        {
            if (!(context.SyntaxReceiver is MapperReceiver mapperReceiver))
            {
                return;
            }

            var compilation = context.Compilation;
            INamedTypeSymbol builderSymbol = compilation.GetTypeByMetadataName(typeof(MapperClass).FullName)!;

            var mapperBuilder = new List<ClassDeclarationSyntax>();
            foreach (var candidate in mapperReceiver.CandidateClass)
            {
                var model = compilation.GetSemanticModel(candidate.SyntaxTree);
                var symbol = model.GetDeclaredSymbol(candidate);
                if (symbol != null && !symbol.IsAbstract && symbol.BaseType != null && SymbolEqualityComparer.Default.Equals(symbol.BaseType, builderSymbol))
                {
                    mapperBuilder.Add(candidate);
                }
            }
            
            foreach (var mapper in mapperBuilder)
            {
                var walker = new MapperWalker(compilation);
                walker.Visit(mapper);

                if (walker.Mappers.Count == 0)
                {
                    continue;
                }
                
                var mapperModel = compilation.GetSemanticModel(mapper.SyntaxTree);
                var mapperSymbol = mapperModel.GetDeclaredSymbol(mapper)!;

                foreach (var mapperSyntax in walker.Mappers)
                {
                    var @class = Process(mapperSyntax, mapperSymbol, compilation);
                    context.AddSource($"{mapperSyntax.From}To{mapperSyntax.To}Mapper.cs", SourceText.From(@class, Encoding.UTF8));
                }
            }
        }

        private static string Process(MapperSyntax mapperSyntax, INamedTypeSymbol mapperSymbol, Compilation compilation)
        {
            static TypeSyntax GetSimpleNameSyntax(TypeSyntax syntax)
            {
                while (true)
                {
                    if (syntax is QualifiedNameSyntax qualifiedNameSyntax)
                    {
                        syntax = qualifiedNameSyntax.Right;
                        continue;
                    }

                    return syntax;
                }
            }

            var adhoc = new AdhocWorkspace();
            var source = mapperSyntax.From;
            var sourceName = GetSimpleNameSyntax(source);
            var sourceModel = compilation.GetSemanticModel(source.SyntaxTree);
            var sourceSymbol = (SymbolFinder
                .FindSymbolAtPositionAsync(sourceModel, sourceName.SpanStart, adhoc)
                .GetAwaiter().GetResult() as INamedTypeSymbol)!;

            var destiny = mapperSyntax.To;
            var destinyName = GetSimpleNameSyntax(destiny);
            var destinyModel = compilation.GetSemanticModel(destiny.SyntaxTree);
            var destinySymbol = (SymbolFinder
                .FindSymbolAtPositionAsync(destinyModel, destinyName.SpanStart, adhoc)
                .GetAwaiter().GetResult() as INamedTypeSymbol)!;

            var builder = new MapperBuilder(source, destiny);
            
            builder
                .AddUsing(sourceSymbol.ContainingNamespace.ToDisplayString())
                .AddUsing(destinySymbol.ContainingNamespace.ToDisplayString());
            
            var destinyProperties = destinySymbol.GetMembers().Where(x => x is IPropertySymbol).Cast<IPropertySymbol>();
            var sourceProperties = sourceSymbol.GetMembers().Where(x => x is IPropertySymbol).Cast<IPropertySymbol>().ToList();

            ProcessRule(destinyProperties, sourceProperties, mapperSyntax.Rules, builder);
            
            return builder.Build();
        }


        private static void ProcessRule(IEnumerable<IPropertySymbol> destinyProperties, 
            IReadOnlyCollection<IPropertySymbol> sourceProperties, 
            IReadOnlyCollection<IRule> rules, 
            MapperBuilder mapperBuilder)
        {
            foreach (var property in destinyProperties)
            {
                var rule = rules.FirstOrDefault(x => x.Destiny.Name == property.Name);
                
                if (rule != null)
                {
                    ExecuteRule(rule, mapperBuilder);
                }
                else
                {
                    var sourceProperty = sourceProperties.FirstOrDefault(x => x.Name == property.Name);
                    if (sourceProperty != null)
                    {
                        if (SymbolEqualityComparer.Default.Equals(property.Type, sourceProperty.Type))
                        {
                            ExecuteRule(new ForEqualPrimitiveTypeRule(property, sourceProperty), mapperBuilder);
                        }
                    }    
                }
            }

            static void ExecuteRule(IRule rule, MapperBuilder mapperBuilder)
            {
                rule.Apply(mapperBuilder);
            }
        }
    }
}
