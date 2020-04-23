using System.Reflection;
using CastForm.Exceptions;
using CastForm.Generator;

namespace CastForm.Rules
{
    internal static class ForRuleFactory
    {
        public static IRuleMapper CreateRule(PropertyInfo destiny, PropertyInfo source, IHashCodeFactoryGenerator hashCodeFactory)
        {
            foreach (var factory in Registers.RuleFactories)
            {
                if (factory.CanCreateRule(source, destiny))
                {
                    return factory.CreateRule(source, destiny, hashCodeFactory);
                }
            }
            
            throw new RuleFactoryNotFound(source, destiny);
        }
    }
}
