namespace Bdev.Net.Dns
{
    using System;

    [Serializable]
    public class AdditionalRecord : ResourceRecord
    {
        internal AdditionalRecord(Pointer pointer) : base(pointer)
        {
        }
    }
}

