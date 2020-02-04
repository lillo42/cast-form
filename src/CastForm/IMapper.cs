using System.Diagnostics.CodeAnalysis;

namespace CastForm
{
    public interface IMapper
    {
        TDestiny Map<TSource, TDestiny>(TSource source);
        TDestiny Map<TDestiny>(object source);
    }

    public interface IMap
    {

        object Map(object source);
    }

    public interface IMap<TSource, TDestiny> : IMap
    {
        [return: MaybeNull]
        object IMap.Map(object source)
            => Map((TSource)source);

        TDestiny Map(TSource source);
    }
}
