using System;

namespace CastForm
{
    /// <summary>
    /// Map source to destiny
    /// </summary>
    public interface IMapper
    {
        /// <summary>
        /// Execute a mapping from source to destiny
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestiny">Destination type to create</typeparam>
        /// <param name="source">Source object to map</param>
        /// <returns>Mapped object</returns>
        TDestiny Map<TSource, TDestiny>(TSource source)
            where TSource: notnull;

        /// <summary>
        /// Execute a mapping from source to destiny
        /// </summary>
        /// <typeparam name="TDestiny">Destination type to create</typeparam>
        /// <param name="source">Source object to map</param>
        /// <returns>Mapped object</returns>
        TDestiny Map<TDestiny>(object source);
    }

    /// <summary>
    /// Map source to destiny 
    /// </summary>
    public interface IMap
    {
        /// <summary>
        /// Execute a mapping from source to destiny
        /// </summary>
        /// <param name="source">Source object to map</param>
        /// <returns>Mapped object</returns>
        object Map(object source);
    }


    /// <summary>
    /// Map source to destiny 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestiny"></typeparam>
    public interface IMap<TSource, TDestiny> : IMap
        where TSource: notnull
    {
        object IMap.Map(object source)
        {
            if (source is TSource cast)
            {
                return Map(cast)!;
            }

            throw new InvalidCastException($"Error to cast {typeof(TSource).FullName} to {typeof(TDestiny).FullName}");
        } 

        /// <summary>
        /// Execute a mapping from source to destiny
        /// </summary>
        /// <param name="source">Source object to map</param>
        /// <returns>Mapped object</returns>
        TDestiny Map(TSource source);
    }
}
