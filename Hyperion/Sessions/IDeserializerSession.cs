using System.Buffers;

namespace Hyperion.Sessions
{
    public interface IDeserializerSession
    {
        /// <summary>
        /// Tries to recall a previously received <paramref name="value"/> by
        /// a given <paramref name="referenceId"/>. Returns true if this has
        /// succeed, false if payload needs to be read from scratch.
        /// </summary>
        /// <param name="referenceId"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool TryRecall<T>(int referenceId, out T value) where T : class;

        /// <summary>
        /// Stores a value in the context of current
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="referenceId"></param>
        /// <param name="value"></param>
        void Store<T>(int referenceId, T value) where T : class;

        /// <summary>
        /// Borrows a memory fragment to be used for serialization process.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        OwnedMemory<byte> Borrow(int length);
    }
}