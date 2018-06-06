using System;
using System.IO;
using Hyperion.Codecs;
using Hyperion.Sessions;

namespace Hyperion
{

    public static class SerializerExtensions
    {
        public static void StreamSerialize<T, TStream>(this Serializer serializer, T value, TStream stream)
            where TStream : Stream =>
            serializer.Serialize(value, new StreamWriter<TStream>(stream));

        public static void StreamSerialize<T, TStream, TSession>(this Serializer serializer, T value, TStream stream, TSession session)
            where TStream : Stream
            where TSession : ISerializerSession =>
            serializer.Serialize(value, new StreamWriter<TStream>(stream), session);

        public static T StreamDeserialize<T, TStream>(this Serializer serializer, TStream stream)
            where TStream : Stream =>
            serializer.Deserialize<T, StreamReader<TStream>>(new StreamReader<TStream>(stream));

        public static T StreamDeserialize<T, TStream, TSession>(this Serializer serializer, TStream stream, TSession session)
            where TStream : Stream
            where TSession : IDeserializerSession =>
            serializer.Deserialize<T, StreamReader<TStream>, TSession>(new StreamReader<TStream>(stream), session);
    }
}