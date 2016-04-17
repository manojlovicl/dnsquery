namespace Bdev.Net.Dns
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class NoResponseException : SystemException
    {
        public NoResponseException()
        {
        }

        public NoResponseException(Exception innerException) : base(null, innerException)
        {
        }

        protected NoResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public NoResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

