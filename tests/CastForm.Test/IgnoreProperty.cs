using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace CastForm.Test
{
    public class IgnoreProperty : BaseTest
    {
        [Fact] 
        public async Task Should_Ignore_When_PropertyExistButIsMappedToIgnore()
        {
            const string code = @"
using System;
using System.Linq.Expressions;
using Oi;
using CastForm;

namespace Ola
{
    public class Test : MapperClass
    {
        public Test()
        {
            CreateMapper<Oi.Foo, Bar>()
                .Ignore(x => x.Text);
        }
    }
}

namespace Oi
{
    public class Foo
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }

    public class Bar
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }

}
";

            var (generated, diagnostics) = await GenerateMapperAsync(code);
            
            diagnostics.Should().BeEmpty();

            generated.SyntaxTrees.Should().HaveCount(3);
            generated.SyntaxTrees.ToList()[2].ToString().Should().Be(
                @"using Oi;

namespace CastForm.Mappers
{
    public class FooToBarMapper : IMapper<Foo, Bar>
    {
        public Bar Map(Foo source)
        {
            var destiny = new Bar();
            destiny.Id = source.Id;
            return destiny;
        }
    }
}
");
        }
        
        [Fact] 
        public async Task Should_Ignore_When_PropertyNotExist()
        {
            const string code = @"
using System;
using System.Linq.Expressions;
using Oi;
using CastForm;

namespace Ola
{
    public class Test : MapperClass
    {
        public Test()
        {
            CreateMapper<Oi.Foo, Bar>();
        }
    }
}

namespace Oi
{
    public class Foo
    {
        public int Id { get; set; }
    }

    public class Bar
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }

}
";

            var (generated, diagnostics) = await GenerateMapperAsync(code);
            diagnostics.Should().BeEmpty();

            generated.SyntaxTrees.Should().HaveCount(3);
            generated.SyntaxTrees.ToList()[2].ToString().Should().Be(
                @"using Oi;

namespace CastForm.Mappers
{
    public class FooToBarMapper : IMapper<Foo, Bar>
    {
        public Bar Map(Foo source)
        {
            var destiny = new Bar();
            destiny.Id = source.Id;
            return destiny;
        }
    }
}
");
        }
    }
}
