using System;
using System.Reflection;

namespace Hyperion.Abstractions
{
    public sealed class FieldDef : IEquatable<FieldDef>, IComparable<FieldDef>
    {
        public FieldOptions Options { get; }
        public FieldInfo Info { get; }
        public Lazy<SchemaDef> FieldSchema { get; }

        public FieldDef(FieldOptions options, FieldInfo info)
        {
            Options = options;
            Info = info ?? throw new ArgumentNullException(nameof(info));
            FieldSchema = new Lazy<SchemaDef>(() => SchemaDef.OfType(info.FieldType));
        }

        public bool Equals(FieldDef other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Options.Equals(other.Options) && Equals(Info.Name, other.Info.Name);
        }

        public override bool Equals(object obj) => obj is FieldDef def && Equals(def);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Options.GetHashCode();
                hashCode = (hashCode * 397) ^ (Info.Name.GetHashCode());;
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"FieldDef({Options.Index}: {Info.FieldType} {Info.Name})";
        }

        public int CompareTo(FieldDef other) => this.Options.Index.CompareTo(other.Options.Index);
    }
}