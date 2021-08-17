using System.Threading.Tasks;
using Xunit;

namespace CastForm.Tests
{
    public class UnitTest1 : BaseTest
    {
        [Fact]
        public async Task Test1()
        {
            await GenerateMapperAsync(@"
namespace CastForm.Tests
{
    public class Foo
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
    
    public class Bar
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class Show
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class TestMap : MapConfiguration
    {
        public TestMap()
        {
            CreateMap<Foo>()
                .From<Bar>()
                .From<Show>();
        }
    }
}
");
        }
        
        [Fact]
        public async Task Test2()
        {
            await GenerateMapperAsync(@"
namespace CastForm.Tests
{
    public class Foo
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
    }
    
    public class Bar
    {
        public int Number { get; set; }
        public string Text { get; set; }
    }

    public class TestMap : MapConfiguration
    {
        public TestMap()
        {
            CreateMap<Foo>()
                .From<Bar>(opt => opt
                    .Map(x => x.Text).Ignore()
                    .Map(x => x.Id).From(x => x.Number)
                    .Map(x => x.Value).From(x => x.Text)
                );
        }
    }
}
");
        }
    }

    public class Foo
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
    
    public class Bar
    {
        public int Number { get; set; }
        public string Text { get; set; }
    }

    public class TestMap : MapConfiguration
    {
        public TestMap()
        {
            CreateMap<Foo>()
                .From<Bar>(cfg => cfg
                    .Map(x => x.Id).Ignore()
                    .Map(x => x.Value).When(x => x.Text != null).From(x => x.Text)
                    .Map(x => x.Value).FromConstant(() => new object())
                    .Map(x => x.Value).FromValue(() => new object())
                );
        }
    }
}
