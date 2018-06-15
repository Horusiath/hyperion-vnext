using System;

namespace Hyperion
{
    public abstract class HyperionAttribute : Attribute
    {
    }

    /// <summary>
    /// Assigned to serializable type. It marks that type with specific informations
    /// about how to serialize instances of specific type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct|AttributeTargets.Enum)]
    public sealed class SchemaAttribute : HyperionAttribute
    {
        /// <summary>
        /// A unique identifier of given type. Attached as part of payload manifest.
        /// If it's not provided, it will be generated as a hash of a given fully qualified type name with assembly.
        /// </summary>
        public ushort? Id { get; }

        public SchemaAttribute() { }

        public SchemaAttribute(ushort id)
        {
            Id = id;
        }
    }

    /// <summary>
    /// Tells hyperion to ignore a given field or property during serialization/deserialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class IgnoreAttribute : HyperionAttribute
    {
    }

    /// <summary>
    /// Marks a target type as immutable. This informs hyperion, that the instances of
    /// a given type once created will never change. Therefore it will allow hyperion
    /// to apply some specific optimizations during their serialization/deserialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct)]
    public sealed class ImmutableAttribute : HyperionAttribute
    {
    }
}