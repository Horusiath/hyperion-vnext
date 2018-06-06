using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using Hyperion.Abstractions;
using Hyperion.Codecs;
using Hyperion.Compilation;
using Hyperion.Sessions;

namespace Hyperion
{
    public sealed class Serializer
    {
        private readonly SerializerSettings settings;
        private readonly ConcurrentDictionary<Type, ICodec> codecs;

        public Serializer() : this(SerializerSettings.Default)
        {
        }

        public Serializer(SerializerSettings settings)
        {
            this.settings = settings;
            this.codecs = new ConcurrentDictionary<Type, ICodec>();

            InitializeCodecs(ReflectionHelpers.LoadedAssemblies());
        }

        private void InitializeCodecs(Assembly[] assemblies)
        {
            var tCodec = typeof(ICodec);
            var found = (
                from asm in assemblies
                from type in asm.GetTypes()
                where tCodec.IsAssignableFrom(type)
                select type)
                .ToArray();
            
            foreach (var type in found)
            {
                if (!type.IsGenericTypeDefinition && type.IsValueType)
                {
                    var value = (ICodec)Activator.CreateInstance(type);
                    var targetType = value.TargetType;

                    codecs.TryAdd(targetType, value);
                }
            }
        }

        public void Serialize<T, TWriter>(T value, TWriter writer)
            where TWriter : struct, IWriter
        {
            Serialize(value, writer, new StatelessSession());
        }
        
        public void Serialize<T, TWriter, TSession>(T value, TWriter writer, TSession session)
            where TWriter : struct, IWriter
            where TSession : ISerializerSession
        {
            var type = typeof(T);
            var codec = (ICodec<T>)codecs.GetOrAdd(type, CreateCodec);
            codec.Write(writer, value, session);
        }

        public T Deserialize<T, TReader>(TReader reader)
            where TReader : struct, IReader
        {
            return Deserialize<T, TReader, StatelessSession>(reader, new StatelessSession());
        }

        public T Deserialize<T, TReader, TSession>(TReader reader, TSession session)
            where TReader : struct, IReader
            where TSession : IDeserializerSession
        {
            var type = typeof(T);
            var codec = (ICodec<T>)codecs.GetOrAdd(type, CreateCodec);
            codec.Read(reader, out var result, session);
            return result;
        }

        private ICodec CreateCodec(Type type)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class SerializerSettings
    {
        public static SerializerSettings Default { get; } = new SerializerSettings();
    }
}