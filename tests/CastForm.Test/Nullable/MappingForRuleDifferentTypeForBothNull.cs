using AutoFixture;
using FluentAssertions;
using Xunit;

namespace CastForm.Test.Nullable
{
    public class MappingForRuleDifferentTypeForBothNull
    {
        private readonly Fixture _fixture;

        public MappingForRuleDifferentTypeForBothNull()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void SourceNotNullableAndDestinyNullable()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleA, SimpleB>()
                    .For(x => x.Number, x => x.Id)
                    .For(x =>x.Value, x => x.Text)
                .Build();

            var a = _fixture.Create<SimpleA>();
            var b = mapper.Map<SimpleB>(a);
            b.Should().NotBeNull();

            b.Number.Should().Be(a.Id);
            b.Value.Should().Be(a.Text);
        }

        public class SimpleA
        {
            public int? Id { get; set; }
            public string Text { get; set; }
        }

        public class SimpleB
        {
            public long? Number { get; set; }
            public string Value { get; set; }
        }
    }
}
