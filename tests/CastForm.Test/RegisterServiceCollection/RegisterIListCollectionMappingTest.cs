using System.Collections.Generic;
using CastForm.Collection;
using CastForm.RegisterServiceCollection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CastForm.Test.RegisterServiceCollection
{
    public class RegisterIListCollectionMappingTest
    {
        private readonly RegisterIListCollectionMapping _register;
        
        public RegisterIListCollectionMappingTest()
        {
            _register = new RegisterIListCollectionMapping();
        }

        [Fact]
        public void Register()
        {
            var collection  = new ServiceCollection();
            
            _register.Register(typeof(int), typeof(string), collection);

            collection.Should().Contain(
                x => x.ServiceType == typeof(IMap<IEnumerable<int>, IList<string>>) 
                     && x.ImplementationType == typeof(IListCollectionMapping<int,string>));
        }
    }
}
