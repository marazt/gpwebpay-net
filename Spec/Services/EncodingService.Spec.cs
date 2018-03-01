using System;
using FluentAssertions;
using GPWebpayNet.Sdk.Exceptions;
using GPWebpayNet.Sdk.Services;
using Xunit;

namespace GPWebpayNet.Sdk.Spec.Services
{
    public class EncodingServiceSpec : ABaseTest
    {
        [Fact]
        public void Should_sign_and_validate_valid_digest()
        {
            // Arrange
            const string message = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.";
            const string privateCertificateFile = "certs/client.pfx";
            const string publicCertificateFile = "certs/client_pub.pem";
            const string privateCertificatePassword = "test";
            const string publicCertificatePassword = null;

            var loggerMock = GetLoggerMock<EncodingService>();
            var testee = new EncodingService(loggerMock.Object);
            var digest = testee.SignData(message, privateCertificateFile, privateCertificatePassword);
            
            // Act
            var result = testee.ValidateDigest(digest, message, publicCertificateFile, publicCertificatePassword);

            // Assert
            result.Should().BeTrue();
        }
        
        [Fact]
        public void Should_sign_and_validate_invalid_digest()
        {
            // Arrage
            const string message = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.";
            const string badMessage = "Lorem ipsum dolor sit amet.";
            const string privateCertificateFile = "certs/client.pfx";
            const string publicCertificateFile = "certs/client_pub.pem";
            const string privateCertificatePassword = "test";
            const string publicCertificatePassword = null;

            var loggerMock = GetLoggerMock<EncodingService>();
            var testee = new EncodingService(loggerMock.Object);
            var digest = testee.SignData(message, privateCertificateFile, privateCertificatePassword);
            
            // Act
            var result = testee.ValidateDigest(digest, badMessage, publicCertificateFile, publicCertificatePassword);

            // Assert
            result.Should().BeFalse();
        }
        
        [Fact]
        public void Should_throw_SignDataException_while_sign_data_when_private_key_is_not_found()
        {
            // Arrange
            const string message = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.";
            const string privateCertificateFile = "certs/non_existing.pfx";
            const string privateCertificatePassword = "test";

            var loggerMock = GetLoggerMock<EncodingService>();
            var testee = new EncodingService(loggerMock.Object);
            
            // Act
            // Assert
            Action action = () => testee.SignData(message, privateCertificateFile, privateCertificatePassword);
            action
                .Should().Throw<SignDataException>()
                .WithMessage("Error while signing data");
        }
        
        [Fact]
        public void Should_throw_SignDataException_while_sign_data_when_invalid_password_set()
        {
            // Arrange
            const string message = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.";
            const string privateCertificateFile = "certs/client.pfx";
            const string privateCertificatePassword = "bad_password";
            var loggerMock = GetLoggerMock<EncodingService>();
            var testee = new EncodingService(loggerMock.Object);
            
            // Act
            // Assert
            Action action = () => testee.SignData(message, privateCertificateFile, privateCertificatePassword);
            action
                .Should().Throw<SignDataException>()
                .WithMessage("Error while signing data");
        }
        
        [Fact]
        public void Should_throw_DigestValidationException_while_validate_digest_when_public_key_is_not_found()
        {
            // Arrange
            const string message = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.";
            const string publicCertificateFile = "certs/non_existing.pem";
            const string publicCertificatePassword = null;
            const string digest = "somedata";

            var loggerMock = GetLoggerMock<EncodingService>();
            var testee = new EncodingService(loggerMock.Object);
            
            // Act
            // Assert
            Action action = () => testee.ValidateDigest(digest, message, publicCertificateFile, publicCertificatePassword);
            action
                .Should().Throw<DigestValidationException>()
                .WithMessage("Error while validating digest");
        }
    }
}