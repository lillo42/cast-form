using System.Collections.Generic;
using CastForm.Collection;
using CastForm.RegisterServiceCollection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CastForm.Test.RegisterServiceCollection
{
    public class RegisterHashSetCollectionMappingTest
    {
        private readonly RegisterHashSetCollectionMapping _register;

        public RegisterHashSetCollectionMappingTest()
        {
            _register = new RegisterHashSetCollectionMapping();
        }

        [Fact]
        public void Register()
        {
            var collection  = new ServiceCollection();
            
            _register.Register(typeof(int), typeof(string), collection);

            collection.Should().Contain(
                x => x.ServiceType == typeof(IMap<IEnumerable<int>, HashSet<string>>) 
                     && x.ImplementationType == typeof(HashSetCollectionMapping<int,string>));
        }
    }
}
