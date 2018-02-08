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
            const string privateCertificateFile = "certs/test.pfx";
            const string publicCertificateFile = "certs/test.pfx";
            const string password = "test";

            var loggerMock = GetLoggerMock<EncodingService>();
            var testee = new EncodingService(loggerMock.Object);
            var digest = testee.SignData(message, privateCertificateFile, password);
            
            // Act
            var result = testee.ValidateDigest(digest, message, publicCertificateFile, password);

            // Assert
            result.Should().BeTrue();
        }
        
        [Fact]
        public void Should_sign_and_validate_invalid_digest()
        {
            // Arrage
            const string message = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.";
            const string badMessage = "Lorem ipsum dolor sit amet.";
            const string privateCertificateFile = "certs/test.pfx";
            const string publicCertificateFile = "certs/test.pfx";
            const string password = "test";

            var loggerMock = GetLoggerMock<EncodingService>();
            var testee = new EncodingService(loggerMock.Object);
            var digest = testee.SignData(message, privateCertificateFile, password);
            
            // Act
            var result = testee.ValidateDigest(digest, badMessage, publicCertificateFile, password);

            // Assert
            result.Should().BeFalse();
        }
        
        [Fact]
        public void Should_throw_SignDataException_while_sign_data_when_private_key_is_not_found()
        {
            // Arrange
            const string message = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.";
            const string privateCertificateFile = "certs/test_key.pem";
            const string password = "test";

            var loggerMock = GetLoggerMock<EncodingService>();
            var testee = new EncodingService(loggerMock.Object);
            
            // Act
            // Assert
            Action action = () => testee.SignData(message, privateCertificateFile, password);
            action
                .Should().Throw<SignDataException>()
                .WithMessage("Error while signing data");
        }
        
        [Fact]
        public void Should_throw_SignDataException_while_sign_data_when_invalid_password_set()
        {
            // Arrange
            const string message = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.";
            const string privateCertificateFile = "certs/test.pfx";
            const string password = "badpassword";

            var loggerMock = GetLoggerMock<EncodingService>();
            var testee = new EncodingService(loggerMock.Object);
            
            // Act
            // Assert
            Action action = () => testee.SignData(message, privateCertificateFile, password);
            action
                .Should().Throw<SignDataException>()
                .WithMessage("Error while signing data");
        }
        
        [Fact]
        public void Should_throw_DigestValidationException_while_validate_digest_when_public_key_is_not_found()
        {
            // Arrange
            const string message = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.";
            const string publicCertificateFile = "certs/test_cert.cer";
            const string password = "test";
            const string digest = "somedata";

            var loggerMock = GetLoggerMock<EncodingService>();
            var testee = new EncodingService(loggerMock.Object);
            
            // Act
            // Assert
            Action action = () => testee.ValidateDigest(digest, message, publicCertificateFile, password);
            action
                .Should().Throw<DigestValidationException>()
                .WithMessage("Error while validating digest");
        }
    }
}