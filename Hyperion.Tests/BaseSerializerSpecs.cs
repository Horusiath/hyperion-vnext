using System;
using System.IO;
using FsCheck.Xunit;
using Xunit;

namespace Hyperion.Tests
{
    public abstract class BaseSerializerSpecs<T> where T : IEquatable<T>
    {
        protected readonly Serializer Serializer;

        protected BaseSerializerSpecs()
        {
            Serializer = new Serializer();
        }

        [Property]
        public void Should_be_able_to_deserialize_serialized_values(T value)
        {
            var actual = Roundtrip(value);
            Assert.Equal(value, actual);
        }

        public T Roundtrip(T value)
        {
            using (var stream = new MemoryStream())
            {
                Serializer.StreamSerialize(value, stream);
                stream.Position = 0;
                return Serializer.StreamDeserialize<T, MemoryStream>(stream);
            }
        }
    }
}