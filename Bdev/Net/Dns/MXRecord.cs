namespace Bdev.Net.Dns
{
    using System;

    [Serializable]
    public class MXRecord : RecordBase, IComparable
    {
        private readonly string _domainName;
        private readonly int _preference;

        internal MXRecord(Pointer pointer)
        {
            this._preference = pointer.ReadShort();
            this._domainName = pointer.ReadDomain();
        }

        public int CompareTo(object obj)
        {
            MXRecord record = (MXRecord) obj;
            if (record._preference < this._preference)
            {
                return 1;
            }
            if (record._preference > this._preference)
            {
                return -1;
            }
            return -record._domainName.CompareTo(this._domainName);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(base.GetType() == obj.GetType()))
            {
                return false;
            }
            MXRecord record = (MXRecord) obj;
            if (record._preference != this._preference)
            {
                return false;
            }
            if (record._domainName != this._domainName)
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return this._preference;
        }

        public static bool operator ==(MXRecord record1, MXRecord record2)
        {
            if (record1 == null)
            {
                throw new ArgumentNullException("record1");
            }
            return record1.Equals(record2);
        }

        public static bool operator !=(MXRecord record1, MXRecord record2)
        {
            return !(record1 == record2);
        }

        public override string ToString()
        {
            return string.Format("Mail Server = {0}, Preference = {1}", this._domainName, this._preference.ToString());
        }

        public string DomainName
        {
            get
            {
                return this._domainName;
            }
        }

        public int Preference
        {
            get
            {
                return this._preference;
            }
        }
    }
}

