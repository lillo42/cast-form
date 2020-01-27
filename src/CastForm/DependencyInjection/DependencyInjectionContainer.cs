using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CastForm.DependencyInjection
{
    internal class DependencyInjectionContainer : IDependencyInjectionContainer
    {
        private readonly ICollection<Type> _types = new List<Type>();

        public void RegisterType(Type type)
        {
            _types.Add(type);
        }

        public IMapper Resolver()
        {
            var resolved = new Dictionary<MapperKey, IMap>();
            Resolver(_types, resolved);
            return new Mapper(resolved);
        }


        private static void Resolver(IEnumerable<Type> mappers, Dictionary<MapperKey, IMap> resolved)
        {
            var toResolve = new List<Type>();
            foreach (var mapper in mappers)
            {
                var constructors = mapper.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
                var mapArgs = mapper.GetInterfaces()[0].GetGenericArguments();
                if (constructors.Length == 0)
                {
                    
                    resolved.Add(new MapperKey(mapArgs[0], mapArgs[1]), (IMap)Activator.CreateInstance(mapper));
                    continue;
                }

                var parameters = constructors[0].GetParameters();

                if (parameters.Length == 0)
                {
                    resolved.Add(new MapperKey(mapArgs[0], mapArgs[1]), (IMap)Activator.CreateInstance(mapper));
                    continue;
                }

                var dependencies = new List<IMap>();
                foreach (var parameter in parameters)
                {
                    var args = parameter.ParameterType.GetGenericArguments();
                    if(resolved.TryGetValue(new MapperKey(args[0], args[1]), out var dependency))
                    {
                        dependencies.Add(dependency);
                    }
                    else
                    {
                        toResolve.Add(mapper);
                    }
                }

                if (dependencies.Count == parameters.Length)
                {
                    resolved.Add(new MapperKey(mapArgs[0], mapArgs[1]), (IMap)Activator.CreateInstance(mapper, args: dependencies.ToArray()));
                }
            }

            if (toResolve.Any())
            {
                Resolver(toResolve, resolved);
            }
        }
    }
}
