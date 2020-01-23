using System.Reflection;
using System.Reflection.Emit;

namespace CastForm
{
    public interface IRuleMapper
    {
        bool Match(PropertyInfo property);

        void Execute(ILGenerator il);
    }
}
