using System;

namespace GPWebpayNet.Sdk.Exceptions
{
    /// <summary>
    /// Invalid digest exception.
    /// </summary>
    /// <seealso cref="GPWebpayNet.Sdk.Exceptions.GPWebpayNetException" />
    public class DigestValidationException : GPWebpayNetException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DigestValidationException"/> class.
        /// </summary>
        public DigestValidationException() : this(string.Empty, null)
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DigestValidationException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public DigestValidationException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}