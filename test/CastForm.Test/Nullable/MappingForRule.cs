using System;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace CastForm.Test.Nullable
{
    public class MappingForRule
    {
        private readonly Fixture _fixture;

        public MappingForRule()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void For()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleA, SimpleB>()
                    .For(x => x.Id, x => x.Number)
                    .For(x =>x.Text, x => x.Value)
                .Build();

            var a = _fixture.Create<SimpleA>();
            var b = mapper.Map<SimpleB>(a);
            b.Should().NotBeNull();

            b.Number.Should().Be(a.Id);
            b.Value.Should().Be(a.Text);
        }

        public class SimpleA
        {
            public int Id { get; set; }
            public string Text { get; set; }
        }

        public class SimpleB
        {
            public int? Number { get; set; }
            public string Value { get; set; }
        }

        public class SimpleC
        {
            public string Number { get; set; }
            public string Value { get; set; }
            public bool IsEnable { get; set; }
        }
    }
}
