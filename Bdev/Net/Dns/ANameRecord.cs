namespace Bdev.Net.Dns
{
    using System;
    using System.Net;

    public class ANameRecord : RecordBase
    {
        internal System.Net.IPAddress _ipAddress;

        internal ANameRecord(Pointer pointer)
        {
            byte num = pointer.ReadByte();
            byte num2 = pointer.ReadByte();
            byte num3 = pointer.ReadByte();
            byte num4 = pointer.ReadByte();
            this._ipAddress = System.Net.IPAddress.Parse(string.Format("{0}.{1}.{2}.{3}", new object[] { num, num2, num3, num4 }));
        }

        public override string ToString()
        {
            return this._ipAddress.ToString();
        }

        public System.Net.IPAddress IPAddress
        {
            get
            {
                return this._ipAddress;
            }
        }
    }
}

