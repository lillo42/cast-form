using AutoFixture;
using FluentAssertions;
using Xunit;

namespace CastForm.Test
{
    public class IgnoreRule
    {
        private readonly Fixture _fixture;

        public IgnoreRule()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Ignore()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleA, SimpleB>()
                    .Ignore(x => x.Value)
                .Build();

            var a = _fixture.Create<SimpleA>();
            var b = mapper.Map<SimpleB>(a);
            b.Should().NotBeNull();

            b.Id.Should().Be(a.Id);
            b.IsEnable.Should().Be(a.IsEnable);
            b.Value.Should().BeNullOrEmpty();
        }

        public class SimpleA
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public bool IsEnable { get; set; }
        }

        public class SimpleB
        {
            public int Id { get; set; }
            public string Value { get; set; }
            public bool IsEnable { get; set; }
        }
    }
}
