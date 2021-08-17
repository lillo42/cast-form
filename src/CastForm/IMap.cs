namespace CastForm
{
    /// <summary>
    /// The map.
    /// </summary>
    public interface IMap
    {
        /// <summary>
        /// Map the <see cref="source"/> to object destiny
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The destiny</returns>
        object? Map(object source);
    }

    /// <summary>
    /// Map <typeparamref name="TDestiny"/> to <typeparamref name="TSource"/>.
    /// </summary>
    /// <typeparam name="TDestiny">The source.</typeparam>
    /// <typeparam name="TSource">The destiny.</typeparam>
    public interface Map<out TDestiny, in TSource> : IMap
    {
        
        /// <summary>
        /// Map <typeparamref name="TDestiny"/> to <typeparamref name="TSource"/>.
        /// </summary>
        /// <param name="source">The <typeparamref name="TSource"/>.</param>
        /// <returns>The new instance of <typeparamref name="TDestiny"/>.</returns>
        TDestiny Map(TSource source);

        object? IMap.Map(object source)
            => Map((TSource)source);
    }
}
