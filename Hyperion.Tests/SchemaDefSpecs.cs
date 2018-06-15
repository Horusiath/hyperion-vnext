using System.Collections.Generic;
using System.Collections.Immutable;
using Hyperion.Abstractions;
using Xunit;

namespace Hyperion.Tests
{
    public class SchemaDefSpecs
    {
        #region test types

        [Schema(1)]
        class TestClass
        {
            public int A;
            public string B;
        }

        [Schema(2)]
        struct TestStruct
        {
            public int A;
            public string B;
        }

        [Schema(3)]
        sealed class TestSealedClass
        {
            public int A;
            public string B;
        }

        [Schema(4)]
        [Immutable]
        class TestImmutableClass
        {
            public int A;
            public string B;

            public TestImmutableClass(int a, string b)
            {
                A = a;
                B = b;
            }
        }

        [Schema(5)]
        sealed class TestSealedClassWithIgnoredField
        {
            public int A;

            [Ignore] public string B;
        }

        [Schema(6)]
        [Immutable]
        struct TestImmutableStruct
        {
            public int A;
            public string B;

            public TestImmutableStruct(int a, string b)
            {
                A = a;
                B = b;
            }
        }

        #endregion

        [Fact]
        public void SchemaDef_should_construct_schema_from_a_class()
        {
            var type = typeof(TestClass);

            var actual = SchemaDef.OfType(type);
            var expected = new SchemaDef(
                type: type,
                options: new SchemaOptions(1),
                fields: ImmutableSortedSet.CreateRange(new []
                {
                    new FieldDef(new FieldOptions(0), type.GetField("A")), 
                    new FieldDef(new FieldOptions(1), type.GetField("B")),
                }));

            Assert.Equal(actual, expected);
        }

        [Fact]
        public void SchemaDef_should_construct_schema_from_a_struct()
        {
            var type = typeof(TestStruct);

            var actual = SchemaDef.OfType(type);
            var expected = new SchemaDef(
                type: type,
                options: new SchemaOptions(2).AsValueType(true).AsSealed(true),
                fields: ImmutableSortedSet.CreateRange(new []
                {
                    new FieldDef(new FieldOptions(0), type.GetField("A")),
                    new FieldDef(new FieldOptions(1), type.GetField("B")),
                }));

            Assert.Equal(actual, expected);
        }

        [Fact]
        public void SchemaDef_should_construct_schema_from_a_sealed_class()
        {
            var type = typeof(TestSealedClass);

            var actual = SchemaDef.OfType(type);
            var expected = new SchemaDef(
                type: type,
                options: new SchemaOptions(3).AsSealed(true),
                fields: ImmutableSortedSet.CreateRange(new []
                {
                    new FieldDef(new FieldOptions(0), type.GetField("A")),
                    new FieldDef(new FieldOptions(1), type.GetField("B")),
                }));

            Assert.Equal(actual, expected);
        }

        [Fact]
        public void SchemaDef_should_construct_schema_from_an_immutable_class()
        {
            var type = typeof(TestImmutableClass);

            var actual = SchemaDef.OfType(type);
            var expected = new SchemaDef(
                type: type,
                options: new SchemaOptions(4)
                    .AsImmutable(true)
                    .PreserveObjectReferences(true),
                fields: ImmutableSortedSet.CreateRange(new []
                {
                    new FieldDef(new FieldOptions(0), type.GetField("A")),
                    new FieldDef(new FieldOptions(1), type.GetField("B")),
                }));

            Assert.Equal(actual, expected);
        }

        [Fact]
        public void SchemaDef_should_construct_schema_from_an_immutable_struct()
        {
            var type = typeof(TestImmutableStruct);

            var actual = SchemaDef.OfType(type);
            var expected = new SchemaDef(
                type: type,
                options: new SchemaOptions(6)
                    .AsImmutable(true)
                    .AsSealed(true)
                    .AsValueType(true),
                fields: ImmutableSortedSet.CreateRange(new []
                {
                    new FieldDef(new FieldOptions(0), type.GetField("A")),
                    new FieldDef(new FieldOptions(1), type.GetField("B")),
                }));

            Assert.Equal(actual, expected);
        }

        [Fact]
        public void SchemaDef_should_construct_schema_without_ignored_fields()
        {
            var type = typeof(TestSealedClassWithIgnoredField);

            var actual = SchemaDef.OfType(type);
            var expected = new SchemaDef(
                type: type,
                options: new SchemaOptions(5).AsSealed(true),
                fields: ImmutableSortedSet.CreateRange(new []
                {
                    new FieldDef(new FieldOptions(0), type.GetField("A")),
                }));

            Assert.Equal(actual, expected);
        }
    }
}