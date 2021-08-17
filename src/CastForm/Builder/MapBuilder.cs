using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace CastForm.Builder
{
    internal class MapBuilder
    {
        private readonly HashSet<string> _using = new HashSet<string>();

        public MapBuilder AddUsing(string @using)
        {
            _using.Add(@using);
            return this;
        }

        private string _namespace = string.Empty;
        public MapBuilder SetNamespace(string @namespace)
        {
            _namespace = @namespace; 
            return this;
        }

        private ITypeSymbol _source = null!;
        public MapBuilder SetSource(ITypeSymbol source)
        {
            _source = source;
            return this;
        }
        
        private ITypeSymbol _destiny = null!;
        public MapBuilder SetDestiny(ITypeSymbol destiny)
        {
            _destiny = destiny;
            return this;
        }

        public string MapName => $"Map{_source.Name}To{_destiny.Name}";

        public string Build()
        {
            var sb = new StringBuilder();
            foreach (var @using in _using)
            {
                sb.AppendLine($"using {@using};");
            }

            sb.AppendLine()
                .AppendLine($"namespace {_namespace}")
                .Append("{");

            sb.Append(IndentSource($"{Environment.NewLine}public class Map{_source.Name}To{_destiny.Name} : IMap<{_destiny.ToDisplayString()}, {_source.ToDisplayString()}>", 1))
                .Append(IndentSource($"{Environment.NewLine}{{", 1))
                .Append(IndentSource($"{Environment.NewLine}public {_destiny} Map({_source} source)", 2))
                .Append(IndentSource($"{Environment.NewLine}{{", 2))
                .Append(IndentSource($"{Environment.NewLine}}}", 2))
                .AppendLine(IndentSource($"{Environment.NewLine}}}", 1));

            sb.Append("}");
            return sb.ToString();
        }
        
        private static string IndentSource(string source, int numIndentations)
        {
            return source.Replace(Environment.NewLine, $"{Environment.NewLine}{new string(' ', 4 * numIndentations)}"); // 4 spaces per indentation.
        }
    }
}
