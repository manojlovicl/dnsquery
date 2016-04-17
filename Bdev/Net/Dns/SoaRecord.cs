namespace Bdev.Net.Dns
{
    using System;

    public class SoaRecord : RecordBase
    {
        private readonly int _defaultTtl;
        private readonly int _expire;
        private readonly string _primaryNameServer;
        private readonly int _refresh;
        private readonly string _responsibleMailAddress;
        private readonly int _retry;
        private readonly int _serial;

        internal SoaRecord(Pointer pointer)
        {
            this._primaryNameServer = pointer.ReadDomain();
            this._responsibleMailAddress = pointer.ReadDomain();
            this._serial = pointer.ReadInt();
            this._refresh = pointer.ReadInt();
            this._retry = pointer.ReadInt();
            this._expire = pointer.ReadInt();
            this._defaultTtl = pointer.ReadInt();
        }

        public override string ToString()
        {
            return string.Format("primary name server = {0}\nresponsible mail addr = {1}\nserial  = {2}\nrefresh = {3}\nretry   = {4}\nexpire  = {5}\ndefault TTL = {6}", new object[] { this._primaryNameServer, this._responsibleMailAddress, this._serial.ToString(), this._refresh.ToString(), this._retry.ToString(), this._expire.ToString(), this._defaultTtl.ToString() });
        }

        public int DefaultTtl
        {
            get
            {
                return this._defaultTtl;
            }
        }

        public int Expire
        {
            get
            {
                return this._expire;
            }
        }

        public string PrimaryNameServer
        {
            get
            {
                return this._primaryNameServer;
            }
        }

        public int Refresh
        {
            get
            {
                return this._refresh;
            }
        }

        public string ResponsibleMailAddress
        {
            get
            {
                return this._responsibleMailAddress;
            }
        }

        public int Retry
        {
            get
            {
                return this._retry;
            }
        }

        public int Serial
        {
            get
            {
                return this._serial;
            }
        }
    }
}

