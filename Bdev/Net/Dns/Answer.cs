namespace Bdev.Net.Dns
{
    using System;

    [Serializable]
    public class Answer : ResourceRecord
    {
        internal Answer(Pointer pointer) : base(pointer)
        {
        }
    }
}

