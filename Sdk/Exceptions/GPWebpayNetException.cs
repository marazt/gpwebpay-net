using System;

namespace GPWebpayNet.Sdk.Exceptions
{
    /// <summary>
    /// Base GPWebpay exception.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class GPWebpayNetException: Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GPWebpayNetException"/> class.
        /// </summary>
        public GPWebpayNetException() : this(string.Empty, null)
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="GPWebpayNetException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public GPWebpayNetException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}