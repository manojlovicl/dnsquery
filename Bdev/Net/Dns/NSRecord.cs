namespace Bdev.Net.Dns
{
    using System;

    public class NSRecord : RecordBase
    {
        private readonly string _domainName;

        internal NSRecord(Pointer pointer)
        {
            this._domainName = pointer.ReadDomain();
        }

        public override string ToString()
        {
            return this._domainName;
        }

        public string DomainName
        {
            get
            {
                return this._domainName;
            }
        }
    }
}

