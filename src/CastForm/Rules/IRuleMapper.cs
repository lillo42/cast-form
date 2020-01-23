using System.Reflection;

namespace CastForm
{
    public interface IRuleMapper
    {
        bool Match(PropertyInfo property);
    }
}
