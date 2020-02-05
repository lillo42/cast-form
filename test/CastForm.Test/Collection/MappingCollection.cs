using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace CastForm.Test.Collection
{
    public class MappingCollection
    {
        private readonly Fixture _fixture;

        public MappingCollection()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void ListToCollection()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleA, SimpleB>()
                .Build();

            var a = _fixture.Create<List<SimpleA>>();
            var b = mapper.Map<ICollection<SimpleB>>(a);
            b.Should().NotBeNull();

            b = b.ToList();
            b.Should().HaveCount(a.Count);
            b.Should().BeEquivalentTo(a);
        }

        [Fact]
        public void ListToList()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleA, SimpleB>()
                .Build();

            var a = _fixture.Create<List<SimpleA>>();
            var b = mapper.Map<List<SimpleB>>(a);
            b.Should().NotBeNull();

            b = b.ToList();
            b.Should().HaveCount(a.Count);
            b.Should().BeEquivalentTo(a);
        }


        [Fact]
        public void ListToIList()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleA, SimpleB>()
                .Build();

            var a = _fixture.Create<List<SimpleA>>();
            var b = mapper.Map<IList<SimpleB>>(a);
            b.Should().NotBeNull();

            b = b.ToList();
            b.Should().HaveCount(a.Count);
            b.Should().BeEquivalentTo(a);
        }

        [Fact]
        public void ListToSet()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleA, SimpleB>()
                .Build();

            var a = _fixture.Create<List<SimpleA>>();
            var b = mapper.Map<ISet<SimpleB>>(a);
            b.Should().NotBeNull();
            b.Should().HaveCount(a.Count);
            b.Should().BeEquivalentTo(a);
        }

        [Fact]
        public void ListToHashSet()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleA, SimpleB>()
                .Build();

            var a = _fixture.Create<List<SimpleA>>();
            var b = mapper.Map<HashSet<SimpleB>>(a);
            b.Should().NotBeNull();
            b.Should().HaveCount(a.Count);
            b.Should().BeEquivalentTo(a);
        }

        [Fact]
        public void ObjectWithCollection()
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

            public ICollection<SimpleA> Collections { get; set; }
        }

        public class SimpleD
        {
            public int Id { get; set; }
            public string Text { get; set; }

            public ICollection<SimpleB> Collections { get; set; }
        }
    }
}
