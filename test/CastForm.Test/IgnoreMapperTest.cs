using AutoFixture;
using FluentAssertions;
using Xunit;

namespace CastForm.Test
{
    public class IgnoreMapperTest
    {
        private readonly Fixture _fixture;

        public IgnoreMapperTest()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Ignore()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleA, SimpleB>()
                    .Ignore(x => x.Id)
                .Build();

            var a = _fixture.Create<SimpleA>();
            var b = mapper.Map<SimpleB>(a);
            b.Should().NotBeNull();
            b.Text.Should().Be(a.Text);
            b.Id.Should().NotBe(a.Id);
            b.Id.Should().Be(0);
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
    }
}
