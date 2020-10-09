using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CastForm.Generators
{
    internal class MapperBuilder
    {
        private const string s_tab = "    ";
        private readonly HashSet<string> _usingAdded;
        private readonly StringBuilder _using;
        private readonly StringBuilder _class;

        public MapperBuilder(TypeSyntax source, TypeSyntax destiny)
        {
            _using = new StringBuilder();
            _class = new StringBuilder();
            _usingAdded = new HashSet<string>();

            _class
                .AppendLine("namespace CastForm.Mappers")
                .AppendLine("{")
                .Append(s_tab).Append("public class ").Append(source).Append("To").Append(destiny).Append("Mapper : IMapper<").Append(source).Append(", ").Append(destiny).AppendLine(">")
                .Append(s_tab).AppendLine("{")
                .Append(s_tab).Append(s_tab).Append("public ").Append(destiny).Append(" Map(").Append(source).AppendLine(" source)")
                .Append(s_tab).Append(s_tab).AppendLine("{")
                .Append(s_tab).Append(s_tab).Append(s_tab).Append("var destiny = new ").Append(destiny).AppendLine("();");
        }

        public MapperBuilder AddUsing(string @namespace)
        {
            if (_usingAdded.Add(@namespace))
            {
                _using.Append("using ").Append(@namespace).AppendLine(";");
            }
            
            return this;
        }

        public MapperBuilder AppendWithTab(string text)
        {
            _class.Append(s_tab).Append(s_tab).Append(s_tab).Append(text);
            return this;
        }
        
        public MapperBuilder Append(string text)
        {
            _class.Append(text);
            return this;
        }
        
        public MapperBuilder AppendLine(string text)
        {
            _class.AppendLine(text);
            return this;
        }

        public string Build()
        {
            return new StringBuilder(_using.Length + _class.Length)
                .Append(_using)
                .AppendLine()
                .Append(_class)
                .Append(s_tab).Append(s_tab).Append(s_tab).AppendLine("return destiny;")
                .Append(s_tab).Append(s_tab).AppendLine("}")
                .Append(s_tab).AppendLine("}")
                .AppendLine("}")
                .ToString();
        }
    }
}
