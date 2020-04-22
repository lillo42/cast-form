using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace CastForm.Test
{
    public class MapperTest
    {
        private readonly Fixture _fixture;
        private readonly Mapper _mapper;
        private readonly Counter _counter;
        private readonly IServiceProvider _provider;

        public MapperTest()
        {
            _counter = new Counter();
            _fixture = new Fixture();
            
            _provider = Substitute.For<IServiceProvider>();
            
            _provider.GetService(typeof(Counter))
                .Returns(_counter);
            
            _mapper = new Mapper(_provider);
        }

        [Fact]
        public void MapGeneric_Should_Map()
        {
            var input = _fixture.Create<int>();
            var output = _fixture.Create<string>();
            
            var map = Substitute.For<IMap<int, string>>();

            map.Map(input)
                .Returns(output);

            _provider.GetService(typeof(IMap<int, string>))
                .Returns(map);

            _mapper.Map<int, string>(input)
                .Should().Be(output);

            map
                .Received(1)
                .Map(input);
            
            _provider
                .Received(1)
                .GetService(typeof(IMap<int, string>));
            
            _provider
                .Received(1)
                .GetService(typeof(Counter));
        }
        
        
        [Fact]
        public void Map_Should_Map()
        {
            var input = _fixture.Create<int>();
            var output = _fixture.Create<string>();
            
            var map = Substitute.For<IMap<int, string>>();

            map.Map((object)input)
                .Returns(output);

            _provider.GetService(typeof(IMap<int, string>))
                .Returns(map);

            _mapper.Map<string>(input)
                .Should().Be(output);

            map
                .Received(1)
                .Map((object)input);
            
            _provider
                .Received(1)
                .GetService(typeof(IMap<int, string>));
            
            _provider
                .Received(1)
                .GetService(typeof(Counter));
        }
        
        
        [Fact]
        public void MapIEnumerable_Should_Map()
        {
            var input = _fixture.Create<IEnumerable<int>>();
            var output = _fixture.Create<IEnumerable<string>>();
            
            var map = Substitute.For<IMap<IEnumerable<int> , IEnumerable<string>>>();

            map.Map((object)input)
                .Returns(output);

            _provider.GetService(typeof(IMap<IEnumerable<int> , IEnumerable<string>>))
                .Returns(map);

            _mapper.Map<IEnumerable<string>>(input)
                .Should().BeEquivalentTo(output);

            map
                .Received(1)
                .Map((object)input);
            
            _provider
                .Received(1)
                .GetService(typeof(IMap<IEnumerable<int>, IEnumerable<string>>));
            
            _provider
                .Received(1)
                .GetService(typeof(Counter));
        }
        
        
        [Fact]
        public async Task MapIAsyncEnumerable_Should_Map()
        {
            var input = _fixture.Create<List<int>>();
            var output = _fixture.Create<List<string>>();
            
            var map = Substitute.For<IMap<IAsyncEnumerable<int> , IAsyncEnumerable<string>>>();

            var async = ToAsync(input);
            
            map.Map((object)async)
                .Returns(ToAsync(output));

            _provider.GetService(typeof(IMap<IAsyncEnumerable<int> , IAsyncEnumerable<string>>))
                .Returns(map);

            var values = _mapper.Map<IAsyncEnumerable<string>>(async);
            
            var result = new List<string>();
            await foreach (var value in values)
            { 
                result.Add(value); 
            }

            result.Should().BeEquivalentTo(output);

            map
                .Received(1)
                .Map((object)async);
            
            _provider
                .Received(1)
                .GetService(typeof(IMap<IAsyncEnumerable<int>, IAsyncEnumerable<string>>));
            
            _provider
                .Received(1)
                .GetService(typeof(Counter));

            static async IAsyncEnumerable<T> ToAsync<T>(IEnumerable<T> values)
            {
                foreach (var value in values)
                {
                    await Task.Delay(1);
                    yield return value;
                }
            }
        }
        
    }
}
