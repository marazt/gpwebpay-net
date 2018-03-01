using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using GPWebpayNet.Sdk.Exceptions;
using Microsoft.Extensions.Logging;

namespace GPWebpayNet.Sdk.Services
{
    public class EncodingService : IEncodingService
    {
        private readonly ILogger logger;

        public EncodingService()
        {
            System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);  
        }

        public EncodingService(ILogger<EncodingService> logger) : this()
        {
            this.logger = logger;
        }

        public string SignData(string message, string certificateFile, string certificatePassword, int encoding = Encoding.DefaultEncoding, X509KeyStorageFlags keyStorageFlags = Encoding.DefaultKeyStorageFlags)
        {
            try
            {
                var msgData = System.Text.Encoding.GetEncoding(encoding).GetBytes(message);
                var cert = new X509Certificate2(certificateFile, certificatePassword, keyStorageFlags);

                byte[] hash;
                using (var rsa = cert.GetRSAPrivateKey())
                {
                    if (rsa == null)
                    {
                        throw new SignDataException("No private key found", null);
                    }

                    hash = rsa.SignData(msgData, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
                }

                return Convert.ToBase64String(hash);
            }
            catch (SignDataException)
            {
                throw;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error while signing data: {ex.Message}\n{ex.StackTrace}");
                this.logger.LogError($"Message: {message}");
                this.logger.LogError($"Private ceritificate: {certificateFile}");
                throw new SignDataException("Error while signing data", ex);
            }
        }

        public bool ValidateDigest(string digest, string message, string certificateFile, string certificatePassword, int encoding = Encoding.DefaultEncoding, X509KeyStorageFlags keyStorageFlags = Encoding.DefaultKeyStorageFlags)
        {
            try
            {
                var byteDigest = Convert.FromBase64String(digest);
                var cert = new X509Certificate2(certificateFile, certificatePassword, keyStorageFlags);
                var data = System.Text.Encoding.GetEncoding(encoding).GetBytes(message);
                var sha = SHA1.Create();
                var hashResult = sha.ComputeHash(data);

                using (var rsa = cert.GetRSAPublicKey())
                {
                    if (rsa == null)
                    {
                        throw new DigestValidationException("No pulic key found", null);
                    }
                    return rsa.VerifyHash(hashResult, byteDigest, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
                }
            }
            catch (DigestValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error while validating digest: {ex.Message}\n{ex.StackTrace}");
                this.logger.LogError($"Digest: {digest}");
                this.logger.LogError($"Message: {message}");
                this.logger.LogError($"Public ceritificate: {certificateFile}");
                throw new DigestValidationException("Error while validating digest", ex);
            }
        }
    }
}