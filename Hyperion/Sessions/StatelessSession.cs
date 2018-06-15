using System;
using System.Buffers;

namespace Hyperion.Sessions
{
    public struct StatelessSession : ISerializerSession, IDeserializerSession
    {
        private static readonly ArrayPool<byte> BufferPool = ArrayPool<byte>.Shared;

        /// <inheritdoc cref="ISerializerSession"/>
        public bool Remember<T>(T value, out int referenceId) where T : class
        {
            referenceId = -1;
            return false;
        }

        /// <inheritdoc cref="IDeserializerSession"/>
        public bool TryRecall<T>(int referenceId, out T value) where T : class
        {
            value = null;
            return false;
        }

        public void Store<T>(int referenceId, T value) where T : class { }
        public byte[] Borrow(int minLength) => BufferPool.Rent(minLength);
        public void Return(byte[] borrowed) => BufferPool.Return(borrowed);
    }
}