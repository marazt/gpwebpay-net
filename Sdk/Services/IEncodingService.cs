using System.Security.Cryptography.X509Certificates;

namespace GPWebpayNet.Sdk.Services
{
    public interface IEncodingService
    {
        string SignData(string message, string privateCertificateFile, string password, int encoding = Encoding.DefaultEncoding, X509KeyStorageFlags keyStorageFlags = Encoding.DefaultKeyStorageFlags);
        bool ValidateDigest(string digest, string message, string publicCertificateFile, string password, int encoding = Encoding.DefaultEncoding, X509KeyStorageFlags keyStorageFlags = Encoding.DefaultKeyStorageFlags);
    }
}