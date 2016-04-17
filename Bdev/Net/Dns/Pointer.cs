namespace Bdev.Net.Dns
{
    using System;
    using System.Text;

    internal class Pointer
    {
        private byte[] _message;
        private int _position;

        public Pointer(byte[] message, int position)
        {
            this._message = message;
            this._position = position;
        }

        public Pointer Copy()
        {
            return new Pointer(this._message, this._position);
        }

        public static Pointer operator +(Pointer pointer, int offset)
        {
            return new Pointer(pointer._message, pointer._position + offset);
        }

        public byte Peek()
        {
            return this._message[this._position];
        }

        public byte ReadByte()
        {
            return this._message[this._position++];
        }

        public char ReadChar()
        {
            return (char) this.ReadByte();
        }

        public string ReadDomain()
        {
            StringBuilder builder = new StringBuilder();
            int num = 0;
            while ((num = this.ReadByte()) != 0)
            {
                if ((num & 0xc0) == 0xc0)
                {
                    Pointer pointer = this.Copy();
                    pointer.SetPosition(((num & 0x3f) << 8) | this.ReadByte());
                    builder.Append(pointer.ReadDomain());
                    return builder.ToString();
                }
                while (num > 0)
                {
                    builder.Append(this.ReadChar());
                    num--;
                }
                if (this.Peek() != 0)
                {
                    builder.Append('.');
                }
            }
            return builder.ToString();
        }

        public int ReadInt()
        {
            return ((((ushort) this.ReadShort()) << 0x10) | ((ushort) this.ReadShort()));
        }

        public short ReadShort()
        {
            return (short) ((this.ReadByte() << 8) | this.ReadByte());
        }

        public void SetPosition(int position)
        {
            this._position = position;
        }
    }
}

