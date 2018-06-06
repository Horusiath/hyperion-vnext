using System;
using System.IO;
using System.Runtime.CompilerServices;
using Hyperion.Abstractions;

namespace Hyperion
{
    /// <summary>
    /// An instance of binary stream reader, which uses fast approach to serialization:
    /// a data is stored as is in the most naive manner without applying possible
    /// compaction steps.
    /// </summary>
    /// <typeparam name="TStream"></typeparam>
    public struct StreamReader<TStream> : IReader where TStream : Stream
    {
        public const int StackallocSpanThreshold = 60;

        private TStream stream;

        public StreamReader(TStream stream)
        {
            if (stream == null) throw new ArgumentNullException();
            if (!stream.CanRead) throw new ArgumentException("Hyperion.Abstractions.StreamReader requires readable stream.");

            this.stream = stream;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sbyte ReadInt8()
        {
            return (sbyte)stream.ReadByte();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public short ReadInt16()
        {
            Span<byte> span = stackalloc byte[sizeof(short)];
            ReadBytes(span);
            return span.NonPortableCast<byte, short>()[0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt32()
        {
            Span<byte> span = stackalloc byte[sizeof(int)];
            ReadBytes(span);
            return span.NonPortableCast<byte, int>()[0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ReadInt64()
        {
            Span<byte> span = stackalloc byte[sizeof(long)];
            ReadBytes(span);
            return span.NonPortableCast<byte, long>()[0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadUInt8()
        {
            return (byte)stream.ReadByte();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort ReadUInt16()
        {
            Span<byte> span = stackalloc byte[sizeof(ushort)];
            ReadBytes(span);
            return span.NonPortableCast<byte, ushort>()[0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ReadUInt32()
        {
            Span<byte> span = stackalloc byte[sizeof(uint)];
            ReadBytes(span);
            return span.NonPortableCast<byte, uint>()[0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ReadUInt64()
        {
            Span<byte> span = stackalloc byte[sizeof(ulong)];
            ReadBytes(span);
            return span.NonPortableCast<byte, ulong>()[0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ReadSingle()
        {
            Span<byte> span = stackalloc byte[sizeof(float)];
            ReadBytes(span);
            return span.NonPortableCast<byte, float>()[0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double ReadDouble()
        {
            Span<byte> span = stackalloc byte[sizeof(double)];
            ReadBytes(span);
            return span.NonPortableCast<byte, double>()[0];
        }

        public decimal ReadDecimal()
        {
            Span<byte> span = stackalloc byte[sizeof(decimal)];
            ReadBytes(span);
            return span.NonPortableCast<byte, decimal>()[0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadBytes(Span<byte> span)
        {
            var readBytes = stream.Read(span);
            //TODO: check if readBytes is equal to span length and try to repeat read
        }
    }
}