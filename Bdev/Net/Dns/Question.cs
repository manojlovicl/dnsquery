namespace Bdev.Net.Dns
{
    using System;
    using System.Text.RegularExpressions;

    [Serializable]
    public class Question
    {
        private readonly DnsClass _dnsClass;
        private readonly DnsType _dnsType;
        private readonly string _domain;

        internal Question(Pointer pointer)
        {
            this._domain = pointer.ReadDomain();
            this._dnsType = (DnsType) pointer.ReadShort();
            this._dnsClass = (DnsClass) pointer.ReadShort();
        }

        public Question(string domain, DnsType dnsType, DnsClass dnsClass)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            if (!(((domain.Length != 0) && (domain.Length <= 0xff)) && Regex.IsMatch(domain, @"^[a-zA-Z0-9_-]{1,63}(\.[a-zA-Z0-9_-]{1,63})+$")))
            {
                throw new ArgumentException("The supplied domain name was not in the correct form", "domain");
            }
            if (!(Enum.IsDefined(typeof(DnsType), dnsType) && (dnsType != DnsType.None)))
            {
                throw new ArgumentOutOfRangeException("dnsType", "Not a valid value");
            }
            if (!(Enum.IsDefined(typeof(DnsClass), dnsClass) && (dnsClass != DnsClass.None)))
            {
                throw new ArgumentOutOfRangeException("dnsClass", "Not a valid value");
            }
            this._domain = domain;
            this._dnsType = dnsType;
            this._dnsClass = dnsClass;
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

        public DnsType Type
        {
            get
            {
                return this._dnsType;
            }
        }
    }
}

