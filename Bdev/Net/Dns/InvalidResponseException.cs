namespace Bdev.Net.Dns
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidResponseException : SystemException
    {
        public InvalidResponseException()
        {
        }

        public InvalidResponseException(Exception innerException) : base(null, innerException)
        {
        }

        protected InvalidResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InvalidResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

