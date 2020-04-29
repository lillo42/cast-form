using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace CastForm.Acceptance.Test.Collection
{
    public class MappingIAsyncEnumerable
    {
        private readonly Fixture _fixture;

        public MappingIAsyncEnumerable()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public async Task EnumerableToEnumerable()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleA, SimpleB>()
                .Build();

            var values = _fixture.Create<List<SimpleA>>();
            var a = GetSimpleA(values);
            var b = mapper.Map<IAsyncEnumerable<SimpleA>, IAsyncEnumerable<SimpleB>>(a);
            
            b.Should().NotBeNull();

            var bList = await ToListAsync(b);
            bList.Should().HaveCount(values.Count());
            bList.Should().BeEquivalentTo(values);
        }
        
        
        private static async Task<List<T>> ToListAsync<T>(IAsyncEnumerable<T> values)
        {
            var list = new List<T>();
            
            await foreach (var value in values)
            {
                list.Add(value);
            }

            return list;
        }

        private static async IAsyncEnumerable<SimpleA> GetSimpleA(IEnumerable<SimpleA> values)
        {
            foreach (var value in values)
            {
                await Task.Delay(1);
                yield return value;
            }
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
