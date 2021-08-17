using System.Collections.Generic;
using System.Linq;
using CastForm.Builder;
using CastForm.Maps;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CastForm.Generators
{
    internal class Parse
    {
        private readonly GeneratorExecutionContext _executionContext;
        private readonly INamedTypeSymbol _mapConfiguration;
        private readonly INamedTypeSymbol _mappingConfiguration;

        public Parse(GeneratorExecutionContext executionContext)
        {
            _executionContext = executionContext;
            _mapConfiguration = executionContext.Compilation.GetTypeByMetadataName("CastForm.MapConfiguration")!;
            _mappingConfiguration = executionContext.Compilation.GetTypeByMetadataName("CastForm.IMappingConfiguration`1")!;
        }

        public IEnumerable<MapSpecification> CreateMapSpecification(ClassDeclarationSyntax classDeclarationSyntax)
        {
            var compilation = _executionContext.Compilation;
            var compilationUnitSyntax = classDeclarationSyntax.FirstAncestorOrSelf<CompilationUnitSyntax>()!;
            var compilationSemanticModel = compilation.GetSemanticModel(compilationUnitSyntax.SyntaxTree);
            
            if (!DerivesFrom(classDeclarationSyntax, _mapConfiguration, compilationSemanticModel))
            {
                return Enumerable.Empty<MapSpecification>();
            }

            var constructors = classDeclarationSyntax.Members.OfType<ConstructorDeclarationSyntax>().ToList();
            if (constructors.Count != 1 )
            {
                // TODO: set error
                return Enumerable.Empty<MapSpecification>();
            }

            var constructor = constructors[0];
            if (constructor.Body == null)
            {
                // TODO: mark error of empty body
                return Enumerable.Empty<MapSpecification>();
            }

            var mapSpecification = new List<MapSpecification>();
            foreach (var expressionStatement in constructor.Body.DescendantNodes().OfType<InvocationExpressionSyntax>())
            {
                if (!(expressionStatement.Expression is MemberAccessExpressionSyntax memberAccessExpressionSyntax)
                    || !(compilationSemanticModel.GetSymbolInfo(memberAccessExpressionSyntax).Symbol is IMethodSymbol methodSymbol) 
                    || !methodSymbol.ContainingType.ConstructedFrom.Equals(_mappingConfiguration, SymbolEqualityComparer.Default))
                {
                    continue;
                }

                var destiny = methodSymbol.ContainingType.TypeArguments[0];
                var source = methodSymbol.TypeArguments[0];
                
                var propertiesMap = GenerateDefaultMap(destiny, source);


                mapSpecification.Add(new MapSpecification(destiny, source, propertiesMap));
            }

            return mapSpecification;
        }

        private static PropertyMapCollection GenerateDefaultMap(INamespaceOrTypeSymbol destiny, INamespaceOrTypeSymbol source)
        {
            var propertiesMap = new PropertyMapCollection();

#pragma warning disable RS1024
            var sourceProperties = source.GetMembers().OfType<IPropertySymbol>().ToDictionary(x => x.Name, x => x);
#pragma warning restore RS1024


            foreach (var destinyProperty in destiny.GetMembers().OfType<IPropertySymbol>())
            {
                propertiesMap.Add(sourceProperties.TryGetValue(destinyProperty.Name, out var sourceProperty)
                    ? new PropertyMap(destinyProperty, sourceProperty, Rule.From)
                    : new PropertyMap(destinyProperty, null, Rule.Ignore));
            }

            return propertiesMap;
        }

        private static bool DerivesFrom(BaseTypeDeclarationSyntax? classDeclarationSyntax, ISymbol extendedSymbol, SemanticModel compilationSemanticModel)
        {
            var baseTypeSyntaxList = classDeclarationSyntax?.BaseList?.Types;
            if (baseTypeSyntaxList == null)
            {
                return false;
            }

            foreach (var baseTypeSyntax in baseTypeSyntaxList)
            {
                if (compilationSemanticModel.GetSymbolInfo(baseTypeSyntax.Type).Symbol is INamedTypeSymbol candidate 
                    && extendedSymbol.Equals(candidate.ConstructedFrom, SymbolEqualityComparer.Default))
                {
                    return true;
                }
            }
                
            return false;
        }
    }
}
