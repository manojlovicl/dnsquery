namespace Bdev.Net.Dns
{
    using System;

    [Serializable]
    public class NameServer : ResourceRecord
    {
        internal NameServer(Pointer pointer) : base(pointer)
        {
        }
    }
}

