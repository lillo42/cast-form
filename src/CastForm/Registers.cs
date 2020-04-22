using System.Collections.Generic;
using System.Linq;
using CastForm.RegisterServiceCollection;

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
        }
        
        public static IEnumerable<IRegisterServiceCollectionType> RegisterTypes => s_registers.Union(s_defaultRegister);

        public static void Add(IRegisterServiceCollectionType registerServiceCollectionType)
        {
            s_registers.Add(registerServiceCollectionType);
        }
    }
}
