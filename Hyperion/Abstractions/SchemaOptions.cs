using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Hyperion.Abstractions
{
    public struct SchemaOptions : IEquatable<SchemaOptions>
    {
        internal const int IMMUTABLE_MASK           = 1;
        internal const int VALUE_TYPE_MASK          = 1 << 1;
        internal const int SEALED_TYPE_MASK         = 1 << 2;
        internal const int PRESERVE_REFERENCES_MASK = 1 << 3;
        internal const int BLITTABLE_MASK           = 1 << 4;

        private readonly ushort typeId;
        private readonly int flags;

        public SchemaOptions(ushort typeId, int flags = 0)
        {
            this.typeId = typeId;
            this.flags = flags;
        }

        /// <summary>
        /// Type ID used to preserve unique type identifier across different contexts.
        /// </summary>
        public ushort TypeId => typeId;

        /// <summary>
        /// Determines if defined type is immutable.
        /// </summary>
        public bool IsImmutable => HasAllFlags(flags, IMMUTABLE_MASK);

        /// <summary>
        /// Determines if defined type can have any other subtypes derriving from it.
        /// By default value types and sealed classes are closed, other classes are open.
        /// </summary>
        public bool IsClosedType => HasAnyFlag(flags, SEALED_TYPE_MASK|VALUE_TYPE_MASK);

        /// <summary>
        /// Determines if defined type is value type (either struct or enum).
        /// </summary>
        public bool IsValueType => HasAllFlags(flags, VALUE_TYPE_MASK);

        /// <summary>
        /// Determines if defined type is sealed class.
        /// </summary>
        public bool IsSealed => HasAllFlags(flags, SEALED_TYPE_MASK);

        /// <summary>
        /// Determines if field is blittable. Blittable types are value types with
        /// consistent memory layout (they can consist only of primitive types
        /// or other blittable types).
        /// </summary>
        public bool IsBlittable => HasAllFlags(flags, BLITTABLE_MASK);

        /// <summary>
        /// Determines if provided type can be preserved in stateful session context.
        /// This can radically improve the payload and a performance of serialization
        /// process.
        ///
        /// Only immutable reference types (classes) can be open.
        /// </summary>
        public bool CanPreserveReferences => HasAllFlags(flags, PRESERVE_REFERENCES_MASK);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HasAllFlags(int flags, int mask) => (flags & mask) == mask;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HasAnyFlag(int flags, int mask) => (flags & mask) != 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int SetFlag(int flags, int mask) => (flags | mask);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int UnsetFlag(int flags, int mask) => (flags & ~mask);

        public SchemaOptions WithTypeId(ushort id) => new SchemaOptions(id, flags);

        public SchemaOptions AsImmutable(bool enable) => 
            new SchemaOptions(typeId, enable ? SetFlag(flags, IMMUTABLE_MASK) : UnsetFlag(flags, IMMUTABLE_MASK));

        public SchemaOptions AsValueType(bool enable) =>
            new SchemaOptions(typeId, enable ? SetFlag(flags, VALUE_TYPE_MASK) : UnsetFlag(flags, VALUE_TYPE_MASK));

        public SchemaOptions AsSealed(bool enable) =>
            new SchemaOptions(typeId, enable ? SetFlag(flags, SEALED_TYPE_MASK) : UnsetFlag(flags, SEALED_TYPE_MASK));

        public SchemaOptions AsBlittable(bool enable) =>
            new SchemaOptions(typeId, enable ? SetFlag(flags, BLITTABLE_MASK) : UnsetFlag(flags, BLITTABLE_MASK));

        public SchemaOptions PreserveObjectReferences(bool enable) =>
            new SchemaOptions(typeId, enable ? SetFlag(flags, PRESERVE_REFERENCES_MASK) : UnsetFlag(flags, PRESERVE_REFERENCES_MASK));

        public bool Equals(SchemaOptions other) => 
            typeId == other.typeId && flags == other.flags;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is SchemaOptions options && Equals(options);
        }

        public static bool operator ==(SchemaOptions x, SchemaOptions y) => x.Equals(y);
        public static bool operator !=(SchemaOptions x, SchemaOptions y) => !x.Equals(y);

        public override int GetHashCode()
        {
            unchecked
            {
                return (typeId.GetHashCode() * 397) ^ flags;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder($"SchemaOptions(id: {typeId}, ");

            if (IsImmutable) sb.Append(":immutable");
            if (IsBlittable) sb.Append(":unamanaged");
            if (IsSealed) sb.Append(":sealed");
            if (IsValueType) sb.Append(":value"); else sb.Append(":class");

            return sb.Append(")").ToString();
        }
    }
}