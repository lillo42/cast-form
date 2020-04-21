using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using CastForm.Collection;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace CastForm.Test.Collection
{
    public class IListCollectionMappingTest
    {
        private readonly Fixture _fixture;
        private readonly IMap<string, string> _map;
        
        public IListCollectionMappingTest()
        {
            _fixture = new Fixture();
            _map = Substitute.For<IMap<string, string>>();
        }
        
        
        [Fact]
        public void List_From_List()
        {
            var list = _fixture.Create<List<string>>();

            foreach (var value in list)
            {
                _map.Map(value)
                    .Returns(value);
            }
            
            list.Add(list.First());
            
            var mapper = new IListCollectionMapping<string, string>(_map);

            var result = mapper.Map(list);

            result.Should().HaveCount(list.Count);
            result.Should().BeEquivalentTo(list);
        }
    }
}
