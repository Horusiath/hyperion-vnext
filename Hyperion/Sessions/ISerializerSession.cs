namespace Hyperion.Sessions
{
    public interface ISerializerSession
    {
        /// <summary>
        /// Remembers a reference and returns a <paramref name="referenceId"/> for provided instance.
        /// Returns false if reference payload should be stored along with <paramref name="referenceId"/>.
        /// Returns true if payload was stored in the past, so only <paramref name="referenceId"/> needs to be used.
        /// If values are not meant to be referenced (i.e. because type doesn't specify preserving object references
        /// or a <see cref="StatelessSession"/> is used) a <paramref name="referenceId"/> returned should be a negative value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="referenceId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool Remember<T>(T value, out int referenceId) where T : class;
    }
}