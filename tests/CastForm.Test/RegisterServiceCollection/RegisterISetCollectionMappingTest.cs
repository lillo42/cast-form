using System.Collections.Generic;
using CastForm.Collection;
using CastForm.RegisterServiceCollection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CastForm.Test.RegisterServiceCollection
{
    public class RegisterISetCollectionMappingTest
    {
        private readonly RegisterISetCollectionMapping _register;
        
        public RegisterISetCollectionMappingTest()
        {
            _register = new RegisterISetCollectionMapping();
        }

        [Fact]
        public void Register()
        {
            var collection  = new ServiceCollection();
            
            _register.Register(typeof(int), typeof(string), collection);

            collection.Should().Contain(
                x => x.ServiceType == typeof(IMap<IEnumerable<int>, ISet<string>>) 
                     && x.ImplementationType == typeof(ISetCollectionMapping<int,string>));
        }
    }
}
