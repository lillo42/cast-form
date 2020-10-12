using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace CastForm.Test
{
    public class DifferentPrimitivePropertyType : BaseTest
    {
        [Theory]
        [MemberData(nameof(ConvertibleData))]
        public async Task Should_MapAutomatic_When_PropertyNameAreEqual(string source, string destiny)
        {
            var code = $@"
using System;
using System.Linq.Expressions;
using Oi;
using CastForm;

namespace Ola
{{
    public class Test : MapperClass
    {{
        public Test()
        {{
            CreateMapper<Oi.Foo, Bar>();
        }}
    }}
}}

namespace Oi
{{
    public class Foo
    {{
        public {source} Id {{ get; set; }}
    }}

    public class Bar
    {{
        public {destiny} Id {{ get; set; }}
    }}

}}
";

            var (generated, diagnostics) = await GenerateMapperAsync(code);

            diagnostics.Should().BeEmpty();

            generated.SyntaxTrees.Should().HaveCount(3);
            generated.SyntaxTrees.ToList()[2].ToString().Should().Be(
                $@"using Oi;

namespace CastForm.Mappers
{{
    public class FooToBarMapper : IMapper<Foo, Bar>
    {{
        public Bar Map(Foo source)
        {{
            var destiny = new Bar();
            destiny.Id = Convert.To(source.Id);
            return destiny;
        }}
    }}
}}
");
        }

        [Theory]
        [MemberData(nameof(ConvertibleData))]
        public async Task Should_Map_When_PropertyNameIsDifferent(string source, string destiny)

        {
            var code = $@"
using System;
using System.Linq.Expressions;
using CastForm;

namespace Oi
{{
    public class Test : MapperClass
    {{
        public Test()
        {{
            CreateMapper<Oi.Foo, Bar>()
                .For(x => x.Value, x => x.Id);
        }}
    }}

    public class Foo
    {{
        public {source} Id {{ get; set; }}
    }}

    public class Bar
    {{
        public {destiny} Value {{ get; set; }}
    }}
}}
";

            var (generated, diagnostics) = await GenerateMapperAsync(code);

            diagnostics.Should().BeEmpty();

            generated.SyntaxTrees.Should().HaveCount(3);
            generated.SyntaxTrees.ToList()[2].ToString().Should().Be(
                $@"using Oi;

namespace CastForm.Mappers
{{
    public class FooToBarMapper : IMapper<Foo, Bar>
    {{
        public Bar Map(Foo source)
        {{
            var destiny = new Bar();
            destiny.Value = Convert.To{MapToConvert(destiny)}(source.Id);
            return destiny;
        }}
    }}
}}
");
        }

        private static string MapToConvert(string value)
        {
            return value switch
            {
                "bool" => "Boolean",
                "byte" => "Byte",
                "char" => "Char",
                "string" => "String",
                "short" => "Int16",
                "int" => "Int32",
                "long" => "Int64",
                "float" => "Single",
                "DateTime" => "DateTime",
                "double" => "Double",
                "decimal" => "Decimal",
                "sbyte" => "SByte",
                "ushort" => "UInt16",
                "uint" => "UInt32",
                "ulong" => "UInt64",
                _ => string.Empty
            };
        }


    public static IEnumerable<object[]> ConvertibleData
        {
            get
            {

                var types = new List<string>()
                {
                    "bool",
                    "byte",
                    "char",
                    "string",
                    "short",
                    "int",
                    "long",
                    "float",
                    "DateTime",
                    "double",
                    "decimal",
                    "sbyte",
                    "ushort",
                    "uint",
                    "ulong", 
                };

                for (var i = 0; i < types.Count; i++)
                {
                    for (var j = 0; j < types.Count; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }

                        yield return new object[] {types[i], types[j]};
                    }
                }
            }
        }
    }
}
