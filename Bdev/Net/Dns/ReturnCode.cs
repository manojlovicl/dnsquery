namespace Bdev.Net.Dns
{
    using System;

    public enum ReturnCode
    {
        Success,
        FormatError,
        ServerFailure,
        NameError,
        NotImplemented,
        Refused,
        Other
    }
}

