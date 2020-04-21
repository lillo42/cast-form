using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using CastForm.Collection;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace CastForm.Test.Collection
{
    public class HashSetCollectionMappingTest
    {
        private readonly Fixture _fixture;
        private readonly IMap<string, string> _map;
        
        public HashSetCollectionMappingTest()
        {
            _fixture = new Fixture();
            _map = Substitute.For<IMap<string, string>>();
        }


        [Fact]
        public void HashSet_From_HashSet()
        {
            var hash = _fixture.Create<HashSet<string>>();

            foreach (var value in hash)
            {
                _map.Map(value)
                    .Returns(value);
            }
            
            var mapper = new HashSetCollectionMapping<string, string>(_map);

            var result = mapper.Map(hash);

            result.Should().BeEquivalentTo(hash);

            
            foreach (var value in hash)
            {
                _map
                    .Received(1)
                    .Map(value);
            }
        }
        
        
        [Fact]
        public void HashSet_From_List()
        {
            var list = _fixture.Create<List<string>>();

            foreach (var value in list)
            {
                _map.Map(value)
                    .Returns(value);
            }
            
            list.Add(list.First());
            
            var mapper = new HashSetCollectionMapping<string, string>(_map);

            var result = mapper.Map(list);

            result.Should().HaveCount(list.Count - 1);
            
            foreach (var value in list)
            {
                result.Contains(value).Should().BeTrue();
            }
        }
    }
}
