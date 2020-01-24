using System;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace CastForm.Test
{
    public class MappingForRuleUsingConvert
    {
        private readonly Fixture _fixture;

        public MappingForRuleUsingConvert()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void ForDifferentType()
        {
            Convert.ToString(0);
            var mapper = new MapperBuilder()
                    .AddMapper<SimpleA, SimpleB>()
                .Build();

            var a = _fixture.Build<SimpleA>()
                .With(x=> x.Id, 1)
                .Create();
            var b = mapper.Map<SimpleB>(a);
            b.Should().NotBeNull();
            b.Id.Should().BeTrue();
            b.Text.Should().Be(Convert.ToInt32(a.Text));
            b.IsEnable.Should().Be(Convert.ToInt16(a.IsEnable));

        }

        public class SimpleA
        {
            public int Id { get; set; }
            public double Text { get; set; }
            public float IsEnable { get; set; }
        }

        public class SimpleB
        {
            public bool Id { get; set; }
            public int Text { get; set; }
            public short IsEnable { get; set; }
        }

        public class SimpleC
        {
            public string Number { get; set; }
            public string Value { get; set; }
            public bool IsEnable { get; set; }
        }
    }
}
