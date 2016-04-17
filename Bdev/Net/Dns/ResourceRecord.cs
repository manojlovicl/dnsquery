namespace Bdev.Net.Dns
{
    using System;

    [Serializable]
    public class ResourceRecord
    {
        private readonly DnsClass _dnsClass;
        private readonly DnsType _dnsType;
        private readonly string _domain;
        private readonly RecordBase _record;
        private readonly int _Ttl;

        internal ResourceRecord(Pointer pointer)
        {
            this._domain = pointer.ReadDomain();
            this._dnsType = (DnsType) pointer.ReadShort();
            this._dnsClass = (DnsClass) pointer.ReadShort();
            this._Ttl = pointer.ReadInt();
            int num = pointer.ReadShort();
            switch (this._dnsType)
            {
                case DnsType.ANAME:
                    this._record = new ANameRecord(pointer);
                    break;

                case DnsType.NS:
                    this._record = new NSRecord(pointer);
                    break;

                case DnsType.SOA:
                    this._record = new SoaRecord(pointer);
                    break;

                case DnsType.MX:
                    this._record = new MXRecord(pointer);
                    break;

                default:
                    pointer += num;
                    break;
            }
        }

        public DnsClass Class
        {
            get
            {
                return this._dnsClass;
            }
        }

        public string Domain
        {
            get
            {
                return this._domain;
            }
        }

        public RecordBase Record
        {
            get
            {
                return this._record;
            }
        }

        public int Ttl
        {
            get
            {
                return this._Ttl;
            }
        }

        public DnsType Type
        {
            get
            {
                return this._dnsType;
            }
        }
    }
}

