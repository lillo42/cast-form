using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace CastForm.Acceptance.Test.Collection
{
    public class MappingEnumerable
    {
        private readonly Fixture _fixture;

        public MappingEnumerable()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void EnumerableToEnumerable()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleA, SimpleB>()
                .Build();

            IEnumerable<SimpleA> a = _fixture.Create<List<SimpleA>>();
            var b = mapper.Map<IEnumerable<SimpleB>>(a);
            b.Should().NotBeNull();

            b = b.ToList();
            b.Should().HaveCount(a.Count());
            b.Should().BeEquivalentTo(a);
        }

        [Fact]
        public void ObjectWithEnumerable()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleA, SimpleB>()
                .AddMapper<SimpleC, SimpleD>()
                .Build();

            var c = _fixture.Create<SimpleC>();
            var d = mapper.Map<SimpleD>(c);
            d.Should().NotBeNull();
            d.Should().BeEquivalentTo(c);
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
            public int Id { get; set; }
            public string Text { get; set; }

            public IEnumerable<SimpleA> Collections { get; set; }
        }

        public class SimpleD
        {
            public int Id { get; set; }
            public string Text { get; set; }

            public IEnumerable<SimpleB> Collections { get; set; }
        }
    }
}
