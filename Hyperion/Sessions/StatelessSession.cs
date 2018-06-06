using System;
using System.Buffers;

namespace Hyperion.Sessions
{
    public struct StatelessSession : ISerializerSession, IDeserializerSession
    {
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
        public OwnedMemory<byte> Borrow(int length)
        {
            var bytes = ArrayPool<byte>.Shared.Rent(length);
            throw new NotImplementedException();
        }
    }
}