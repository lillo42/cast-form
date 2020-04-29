using System.Collections.Generic;
using CastForm.Collection;
using CastForm.RegisterServiceCollection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CastForm.Test.RegisterServiceCollection
{
    public class RegisterICollectionMappingTest
    {
        private readonly RegisterICollectionMapping _register;
        
        public RegisterICollectionMappingTest()
        {
            _register = new RegisterICollectionMapping();
        }

        [Fact]
        public void Register()
        {
            var collection  = new ServiceCollection();
            
            _register.Register(typeof(int), typeof(string), collection);

            collection.Should().Contain(
                x => x.ServiceType == typeof(IMap<IEnumerable<int>, ICollection<string>>) 
                     && x.ImplementationType == typeof(ICollectionMapping<int,string>));
        }
    }
}
