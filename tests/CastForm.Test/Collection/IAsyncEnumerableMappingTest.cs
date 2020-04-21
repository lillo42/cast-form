using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using CastForm.Collection;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace CastForm.Test.Collection
{
    public class IAsyncEnumerableMappingTest
    {
        private readonly Fixture _fixture;
        private readonly IMap<string, string> _map;
        
        public IAsyncEnumerableMappingTest()
        {
            _fixture = new Fixture();
            _map = Substitute.For<IMap<string, string>>();
        }
        
        
        [Fact]
        public async Task IAsyncEnumerable_From_List()
        {
            var list = _fixture.Create<List<string>>();

            foreach (var value in list)
            {
                _map.Map(value)
                    .Returns(value);
            }
            
            var mapper = new IAsyncEnumerableMapping<string, string>(_map, new Counter());

            var result = mapper.Map(ToAsync(list));

            var counter = 0;
            await foreach (var value in result)
            {
                value.Should().Be(list[counter++]);
            }
        }


        private static async IAsyncEnumerable<string> ToAsync(IEnumerable<string> values)
        {
            foreach (var value in values)
            {
                await Task.Delay(100);
                yield return value;
            }
        }
    }
}
