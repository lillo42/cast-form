using AutoFixture;
using FluentAssertions;
using Xunit;

namespace CastForm.Acceptance.Test
{
    public class MappingSimpleObject
    {
        private readonly Fixture _fixture;

        public MappingSimpleObject()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void SameType()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleA, SimpleB>()
                .Build();

            var a = _fixture.Create<SimpleA>();
            var b = mapper.Map<SimpleB>(a);
            b.Should().NotBeNull();
            b.Should().BeEquivalentTo(a);
        }

        [Fact]
        public void DifferentType()
        {
            var builder = new MapperBuilder()
                .AddMapper<SimpleA, SimpleC>();

            var mapper = builder.Build();

            var a = _fixture.Create<SimpleA>();
            var b = mapper.Map<SimpleC>(a);
            b.Should().NotBeNull();
            b.Text.Should().Be(a.Text);
            b.Id.Should().Be(a.Id.ToString());
        }

        public class SimpleA
        {
            public int Id { get; set; }
            public string Text { get; set; }
        }

        public class SimpleB
        {
            public int Id { get; set; }
            public string Text { get; set; }
        }

        public class SimpleC
        {
            public string Id { get; set; }
            public string Text { get; set; }
        }
    }
}
