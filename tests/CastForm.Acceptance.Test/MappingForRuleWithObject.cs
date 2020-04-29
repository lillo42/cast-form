using AutoFixture;
using FluentAssertions;
using Xunit;

namespace CastForm.Acceptance.Test
{
    public class MappingForRuleWithObject
    {
        private readonly Fixture _fixture;

        public MappingForRuleWithObject()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void For()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleA, SimpleC>()
                    .For(x => x.SimpleD, x => x.SimpleB)
                .AddMapper<SimpleB, SimpleD>()
                .Build();

            var a = _fixture.Create<SimpleA>();
            var c = mapper.Map<SimpleC>(a);
            c.Should().NotBeNull();
            
            c.Id.Should().Be(a.Id);
            c.Text.Should().Be(a.Text);
            c.SimpleD.Should().NotBeNull();
            c.SimpleD.Number.Should().Be(a.SimpleB.Number);
            c.SimpleD.Value.Should().Be(a.SimpleB.Value);
        }

        [Fact]
        public void MapWithoutFor()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleA, SimpleE>()
                .AddMapper<SimpleB, SimpleD>()
                .Build();

            var a = _fixture.Create<SimpleA>();
            var c = mapper.Map<SimpleE>(a);
            c.Should().NotBeNull();

            c.Id.Should().Be(a.Id);
            c.Text.Should().Be(a.Text);
            c.SimpleB.Should().NotBeNull();
            c.SimpleB.Number.Should().Be(a.SimpleB.Number);
            c.SimpleB.Value.Should().Be(a.SimpleB.Value);
        }


        [Fact]
        public void MapWithOneProperty()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleF, SimpleG>()
                .AddMapper<SimpleB, SimpleD>()
                .Build();

            var a = _fixture.Create<SimpleF>();
            var c = mapper.Map<SimpleG>(a);
            c.Should().NotBeNull();

            c.Simple.Should().NotBeNull();
            c.Simple.Number.Should().Be(a.Simple.Number);
            c.Simple.Value.Should().Be(a.Simple.Value);
        }

        public class SimpleA
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public SimpleB SimpleB { get; set; }
        }

        public class SimpleB
        {
            public int Number { get; set; }
            public string Value { get; set; }
        }

        public class SimpleC
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public SimpleD SimpleD { get; set; }
        }

        public class SimpleD
        {
            public int Number { get; set; }
            public string Value { get; set; }
        }

        public class SimpleE
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public SimpleD SimpleB { get; set; }
        }

        public class SimpleF
        {
            public SimpleB Simple { get; set; }
        }

        public class SimpleG
        {
            public SimpleD Simple { get; set; }
        }
    }
}
