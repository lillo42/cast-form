using System.Collections.Generic;
using CastForm.Collection;
using CastForm.RegisterServiceCollection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CastForm.Test.RegisterServiceCollection
{
    public class RegisterListCollectionMappingTest
    {
        private readonly RegisterListCollectionMapping _register;
        
        public RegisterListCollectionMappingTest()
        {
            _register = new RegisterListCollectionMapping();
        }

        [Fact]
        public void Register()
        {
            var collection  = new ServiceCollection();
            
            _register.Register(typeof(int), typeof(string), collection);

            collection.Should().Contain(
                x => x.ServiceType == typeof(IMap<IEnumerable<int>, List<string>>) 
                     && x.ImplementationType == typeof(ListCollectionMapping<int,string>));
        }
    }
}
