using System.Security.Cryptography.X509Certificates;

namespace GPWebpayNet.Sdk
{
    public static class Encoding
    {
        public const int DefaultEncoding = 1250;

        public const X509KeyStorageFlags DefaultKeyStorageFlags = X509KeyStorageFlags.MachineKeySet |
                                                           X509KeyStorageFlags.PersistKeySet |
                                                           X509KeyStorageFlags.Exportable;
    }
}