using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;

namespace CastForm.Generators
{
    /// <summary>
    /// 
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
            INamedTypeSymbol builderSymbol = compilation.GetTypeByMetadataName(nameof(MapperClass))!;

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
                var walker = new MapperWalker();
                walker.Visit(mapper);

                if (walker.Mapper.Count == 0)
                {
                    continue;
                }
                
                var mapperModel = compilation.GetSemanticModel(mapper.SyntaxTree);
                var mapperSymbol = mapperModel.GetDeclaredSymbol(mapper)!;

                foreach (var ((from, to), rules) in walker.Mapper)
                {
                    var fromName = GetSimpleNameSyntax(from);
                    var fromModel = compilation.GetSemanticModel(from.SyntaxTree);
                    var fromSymbol = (SymbolFinder
                        .FindSymbolAtPositionAsync(fromModel, fromName.SpanStart, new AdhocWorkspace())
                        .GetAwaiter().GetResult() as INamedTypeSymbol)!;
                    
                    var toName = GetSimpleNameSyntax(to);
                    var toModel = compilation.GetSemanticModel(to.SyntaxTree);
                    var toSymbol = (SymbolFinder
                        .FindSymbolAtPositionAsync(toModel, toName.SpanStart, new AdhocWorkspace())
                        .GetAwaiter().GetResult() as INamedTypeSymbol)!;

                    var sb = new StringBuilder()
                        .AppendLine($"using {fromSymbol.ContainingNamespace.ToDisplayString()};");
                    
                    if (toSymbol.ContainingNamespace.ToDisplayString() != fromSymbol.ContainingNamespace.ToDisplayString())
                    {
                        sb.AppendLine($"using {toSymbol.ContainingNamespace.ToDisplayString()};");
                    }

                    sb
                        .AppendLine($"namespace {mapperSymbol.ContainingNamespace.ToDisplayString()}")
                        .AppendLine("{")
                        .AppendLine($"    public class {from}To{to}Mapper : IMapper<{from}, {to}>")
                        .AppendLine("    {")
                        .AppendLine($"        public {to} Map({from} source)")
                        .AppendLine("        {")
                        .AppendLine($"            return new {to}")
                        .AppendLine("            {");

                    var toProperties = toSymbol.GetMembers().Where(x => x is IPropertySymbol).Cast<IPropertySymbol>();
                    var fromProperties = fromSymbol.GetMembers().Where(x => x is IPropertySymbol).Cast<IPropertySymbol>().ToList();

                    foreach (var property in toProperties)
                    {
                        if (fromProperties.Any(x => x.Name == property.Name))
                        {
                            sb.AppendLine($"                {property.Name} = source.{property.Name}");
                        }
                    }

                    sb
                        .AppendLine( "            }")
                        .AppendLine( "        }") 
                        .AppendLine( "    }") 
                        .AppendLine( "}");   
                }
            }

            static TypeSyntax GetSimpleNameSyntax(TypeSyntax syntax)
            {
                if (syntax is QualifiedNameSyntax qualifiedNameSyntax)
                {
                    return GetSimpleNameSyntax(qualifiedNameSyntax.Right);
                }
                
                return syntax;
            }
        }

        public class MapperReceiver : ISyntaxReceiver
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


        public class MapperWalker : CSharpSyntaxWalker
        {

            public Dictionary<(TypeSyntax soruce, TypeSyntax destiny), List<(ArgumentSyntax source, ArgumentSyntax destiny)>> Mapper { get; } = new Dictionary<(TypeSyntax soruce, TypeSyntax destiny), List<(ArgumentSyntax source, ArgumentSyntax destiny)>>(); 
            private readonly Stack<(ArgumentSyntax source, ArgumentSyntax destiny)> _properties = new Stack<(ArgumentSyntax source, ArgumentSyntax destiny)>();
            
            public MapperWalker()
                : base(SyntaxWalkerDepth.StructuredTrivia)
            {
                
            }
            
            public override void VisitInvocationExpression(InvocationExpressionSyntax node)
            {
                if (node.Expression is GenericNameSyntax genericNameSyntax)
                {
                    var generic = genericNameSyntax.TypeArgumentList;
                    var key = (generic.Arguments[0], generic.Arguments[1]);
                    if (!Mapper.TryGetValue(key, out var properties))
                    {
                        properties = new List<(ArgumentSyntax source, ArgumentSyntax destiny)>();
                    }

                    while (_properties.TryPop(out var property))
                    {
                        properties.Add(property);
                    }

                    Mapper[key] = properties;
                }
                else if (node.Expression is MemberAccessExpressionSyntax memberAccessExpressionSyntax && memberAccessExpressionSyntax.Name.ToString() == "For")
                {
                    _properties.Push((node.ArgumentList.Arguments[0], node.ArgumentList.Arguments[1]));
                }
                base.VisitInvocationExpression(node);
            }
        }
    }
}
