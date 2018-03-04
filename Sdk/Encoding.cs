using System.Security.Cryptography.X509Certificates;

namespace GPWebpayNet.Sdk
{
    /// <summary>
    /// Helper encoding class.
    /// </summary>
    public static class Encoding
    {
        /// <summary>
        /// The default encoding
        /// </summary>
        public const int DefaultEncoding = 1250;

        /// <summary>
        /// The default key storage flags
        /// </summary>
        public const X509KeyStorageFlags DefaultKeyStorageFlags = X509KeyStorageFlags.MachineKeySet |
                                                           X509KeyStorageFlags.PersistKeySet |
                                                           X509KeyStorageFlags.Exportable;
    }
}