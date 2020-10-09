using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace CastForm.Test
{
    public class SamePropertyType : BaseTest
    {
        [Fact]
        public async Task Should_MapAutomatic_When_PropertyNameAreEqual()
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
        public async Task Should_Map_When_PropertyNameIsDifferent()
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
                .For(x => x.Value, x => x.Id);
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
        public int Value { get; set; }
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
            destiny.Value = source.Id;
            return destiny;
        }
    }
}
");
        }
        
        [Fact]
        public async Task Should_MapAllProperties_When_PropertyNameAreEqual()
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
        public string Text { get; set; }
        public bool Enable { get; set; }
        public long Version { get; set; }
        public DateTime OccuredOn { get; set; }
    }

    public class Bar
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Enable { get; set; }
        public long Version { get; set; }
        public DateTime OccuredOn { get; set; }
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
            destiny.Text = source.Text;
            destiny.Enable = source.Enable;
            destiny.Version = source.Version;
            destiny.OccuredOn = source.OccuredOn;
            return destiny;
        }
    }
}
");
        }
        
        [Fact]
        public async Task Should_MapAllProperties_When_PropertyNameAreDifferent()
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
                .For(x => x.Value, x => x.Id)
                .For(x => x.Description, x => x.Text)
                .For(x => x.IsAvailable, x => x.Enable)
                .For(x => x.Token, x => x.Version)
                .For(x => x.LastUpdate, x => x.OccuredOn);
        }
    }
}

namespace Oi
{
    public class Foo
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Enable { get; set; }
        public long Version { get; set; }
        public DateTime OccuredOn { get; set; }
    }
   
    public class Bar
    {
        public int Value { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public long Token { get; set; }
        public DateTime LastUpdate { get; set; }
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
            destiny.Value = source.Id;
            destiny.Description = source.Text;
            destiny.IsAvailable = source.Enable;
            destiny.Token = source.Version;
            destiny.LastUpdate = source.OccuredOn;
            return destiny;
        }
    }
}
");
        }
        
        [Fact]
        public async Task Should_MapAllProperties_When_PropertyHaveSameNameAndDifferentPropertyType()
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
                .For(x => x.Enable, x => x.Id)
                .For(x => x.Version, x => x.Text)
                .For(x => x.OccuredOn, x => x.Enable)
                .For(x => x.Id, x => x.Version)
                .For(x => x.Text, x => x.OccuredOn);
        }
    }
}

namespace Oi
{
    public class Foo
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Enable { get; set; }
        public long Version { get; set; }
        public DateTime OccuredOn { get; set; }
    }
   
    public class Bar
    {
        public int Enable { get; set; }
        public string Version { get; set; }
        public bool OccuredOn { get; set; }
        public long Id { get; set; }
        public DateTime Text { get; set; }
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
            destiny.Enable = source.Id;
            destiny.Version = source.Text;
            destiny.OccuredOn = source.Enable;
            destiny.Id = source.Version;
            destiny.Text = source.OccuredOn;
            return destiny;
        }
    }
}
");
        }
        
        [Fact]
        public async Task Should_MapAllProperties_When_PropertyHaveSameAndDifferentPropertyName()
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
                .For(x => x.Description, x => x.Text)
                .For(x => x.IsAvailable, x => x.Enable)
                .For(x => x.LastUpdate, x => x.OccuredOn);
        }
    }
}

namespace Oi
{
    public class Foo
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Enable { get; set; }
        public long Version { get; set; }
        public DateTime OccuredOn { get; set; }
    }

    public class Bar
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public long Version { get; set; }
        public DateTime LastUpdate { get; set; }
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
            destiny.Description = source.Text;
            destiny.IsAvailable = source.Enable;
            destiny.Version = source.Version;
            destiny.LastUpdate = source.OccuredOn;
            return destiny;
        }
    }
}
");
        }
    }
}
