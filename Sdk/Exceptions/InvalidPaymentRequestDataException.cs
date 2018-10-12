using System;

namespace GPWebpayNet.Sdk.Exceptions
{
    /// <summary>
    /// Invalid payment request data exception.
    /// </summary>
    /// <seealso cref="GPWebpayNet.Sdk.Exceptions.GPWebpayNetException" />
    public class InvalidPaymentRequestDataException : GPWebpayNetException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPaymentRequestDataException"/> class.
        /// </summary>
        public InvalidPaymentRequestDataException() : this(string.Empty, null)
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPaymentRequestDataException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public InvalidPaymentRequestDataException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}