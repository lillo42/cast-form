using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CastForm.Generators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.DependencyModel;

namespace CastForm.Test
{
    public abstract class BaseTest
    {
        protected virtual async Task<(Compilation generated, ImmutableArray<Diagnostic> diagnostics)> GenerateMapperAsync(string code)
        {
            var node = CSharpSyntaxTree.ParseText(code);
            var mapperClass = CSharpSyntaxTree.ParseText(await File.ReadAllTextAsync("../../../../../src/CastForm/MapperClass.cs"));
            
            var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);
            var compilation = CSharpCompilation.Create("CastForm.Test.Generated")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(DependencyContext.Default.CompileLibraries
                    .SelectMany(cl => cl.ResolveReferencePaths())
                    .Select(asm => MetadataReference.CreateFromFile(asm)))
                .AddSyntaxTrees(mapperClass)
                .AddSyntaxTrees(node);
            
            var mapper = new MapperGenerator();
            var driver = new CSharpGeneratorDriver(
                new CSharpParseOptions(),
                ImmutableArray.Create<ISourceGenerator>(mapper),
                ImmutableArray.Create<AdditionalText>()
            );
            
            driver.RunFullGeneration(compilation, out var generated, out var diagnostics);

            return (generated, diagnostics);
        }
    }
}
