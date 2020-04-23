using System.Collections.Generic;
using System.Linq;
using CastForm.RegisterServiceCollection;
using CastForm.Rules.Factories;

namespace CastForm
{
    internal static class Registers
    {
        private static readonly List<IRegisterServiceCollectionType> s_registers = new List<IRegisterServiceCollectionType>();
        private static readonly List<IRegisterServiceCollectionType> s_defaultRegister = new List<IRegisterServiceCollectionType>();

        static Registers()
        {
            s_defaultRegister.Add(new RegisterLazyEnumerableMapping());
            s_defaultRegister.Add(new RegisterIAsyncEnumerableMapping());
            s_defaultRegister.Add(new RegisterHashSetCollectionMapping());
            s_defaultRegister.Add(new RegisterICollectionMapping());
            s_defaultRegister.Add(new RegisterIListCollectionMapping());
            s_defaultRegister.Add(new RegisterISetCollectionMapping());
            s_defaultRegister.Add(new RegisterListCollectionMapping());
            
            s_defaultFactories.Add(new SameTypeRuleFactory());
            s_defaultFactories.Add(new DifferentNetTypeRuleFactory());
            s_defaultFactories.Add(new NullableRuleForNetType());
            s_defaultFactories.Add(new NullableRuleForDifferentTypeFactory());
            s_defaultFactories.Add(new DifferentTypeRuleFactory());
        }
        
        public static IEnumerable<IRegisterServiceCollectionType> RegisterTypes => s_registers.Union(s_defaultRegister);

        public static void Add(IRegisterServiceCollectionType registerServiceCollectionType) 
            => s_registers.Add(registerServiceCollectionType);

        private static readonly List<IRuleFactory> s_factories = new List<IRuleFactory>();
        private static readonly List<IRuleFactory> s_defaultFactories = new List<IRuleFactory>();
        public static IEnumerable<IRuleFactory> RuleFactories => s_factories.Union(s_defaultFactories);
        public static void Add(IRuleFactory factory) 
            => s_factories.Add(factory);
    }
}
