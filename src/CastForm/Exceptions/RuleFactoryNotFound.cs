using System.Reflection;

namespace CastForm.Exceptions
{
    /// <summary>
    /// Exception for when and rule match with source and destiny <see cref="PropertyInfo"/>
    /// </summary>
    public class RuleFactoryNotFound : CastFormException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceProperty"></param>
        /// <param name="destinyProperty"></param>
        public RuleFactoryNotFound(PropertyInfo sourceProperty, PropertyInfo destinyProperty)
            : base("Rule factory not found")
        {
            SourceProperty = sourceProperty;
            DestinyProperty = destinyProperty;
        }

        /// <summary>
        /// The Source property
        /// </summary>
        public PropertyInfo SourceProperty { get; }
        
        /// <summary>
        /// The Destiny property
        /// </summary>
        public PropertyInfo DestinyProperty { get; }
        
        
    }
}
