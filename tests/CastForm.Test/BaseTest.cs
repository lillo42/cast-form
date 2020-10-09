using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using CastForm.Generators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CastForm.Test
{
    public abstract class BaseTest
    {
        protected virtual async Task<(Compilation generated, ImmutableArray<Diagnostic> diagnostics)> GenerateMapperAsync(string code)
        {
            var node = CSharpSyntaxTree.ParseText(code);
            var mapperClass = CSharpSyntaxTree.ParseText(await File.ReadAllTextAsync("../../../../../src/CastForm/MapperClass.cs"));

            var mapper = new MapperGenerator();
            var compilation = CSharpCompilation.Create("CastForm.Test.Generated", new[] {node, mapperClass});
            
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
