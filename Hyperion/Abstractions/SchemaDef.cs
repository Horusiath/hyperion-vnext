using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hyperion.Abstractions
{
    /// <summary>
    /// Schema definition describes details about the particular type to be serialized.
    /// </summary>
    public sealed class SchemaDef : IEquatable<SchemaDef>
    {
        private static readonly BindingFlags FieldBindings = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public static SchemaDef OfType(Type type)
        {
            var typeId = GetTypeId(type);
            var options = GetTypeFlags(type, new SchemaOptions(typeId));
            var fields = GetFieldDefs(type);

            return new SchemaDef(type, options, ImmutableSortedSet.CreateRange(fields));
        }

        private static IEnumerable<FieldDef> GetFieldDefs(Type type)
        {
            ushort index = 0;
            foreach (var field in type.GetFields(FieldBindings))
            {
                var ignore = field.GetCustomAttribute<IgnoreAttribute>();
                if (ignore == null)
                {
                    var fieldDef = new FieldDef(new FieldOptions(index), field);
                    index++;
                    yield return fieldDef;
                }
            }
        }

        private static SchemaOptions GetTypeFlags(Type type, SchemaOptions options)
        {
            if (type.IsValueType)
                options = options.AsValueType(true);

            if (type.IsSealed)
                options = options.AsSealed(true);

            foreach (var attribute in type.GetCustomAttributes())
            {
                switch (attribute)
                {
                    case ImmutableAttribute _:
                        options = options.AsImmutable(true);
                        if (type.IsClass)
                            options = options.PreserveObjectReferences(true);
                        break;
                }
            }

            return options;
        }

        private static ushort GetTypeId(Type type)
        {
            var schema = type.GetCustomAttribute<SchemaAttribute>();
            if (schema != null && schema.Id.HasValue)
                return schema.Id.Value;
            else
            {
                throw new NotImplementedException();
            }
        }

        public Type Type { get; }
        public SchemaOptions Options { get; }
        public ImmutableSortedSet<FieldDef> Fields { get; }
        public FieldDef this[int index] => Fields.First(x => x.Options.Index == index);
        public FieldDef this[string name] => Fields.First(x => x.Info.Name == name);

        public SchemaDef(Type type, SchemaOptions options, ImmutableSortedSet<FieldDef> fields)
        {
            Options = options;
            Type = type;
            Fields = fields;
        }

        public bool Equals(SchemaDef other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (Type != other.Type) return false;
            if (!Options.Equals(other.Options)) return false;
            if (Fields.Count != other.Fields.Count) return false;

            for (int i = 0; i < Fields.Count; i++)
            {
                if (!Fields[i].Equals(other.Fields[i])) return false;
            }
            
            return true;
        }

        public override bool Equals(object obj) => 
            obj is SchemaDef def && Equals(def);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Type != null ? Type.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Options.GetHashCode();
                foreach (var fieldDef in Fields)
                {
                    hashCode = (hashCode * 397) ^ fieldDef.GetHashCode();
                }
                return hashCode;
            }
        }

        public override string ToString() => 
            $"SchemaDef(type: {Type.FullName}, options:{Options}, fields:[\n\t{string.Join("\n\t ", Fields)}\n])";
    }
}