using System;

namespace Hyperion.Abstractions
{
    public struct FieldOptions : IEquatable<FieldOptions>
    {
        public ushort Index { get; }

        public FieldOptions(ushort index)
        {
            Index = index;
        }

        public bool Equals(FieldOptions other)
        {
            return Index == other.Index;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is FieldOptions && Equals((FieldOptions) obj);
        }

        public override int GetHashCode()
        {
            return Index.GetHashCode();
        }
    }
}