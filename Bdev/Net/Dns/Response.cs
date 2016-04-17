namespace Bdev.Net.Dns
{
    using System;

    public class Response
    {
        private readonly AdditionalRecord[] _additionalRecords;
        private readonly Answer[] _answers;
        private readonly bool _authoritativeAnswer;
        private readonly NameServer[] _nameServers;
        private readonly Question[] _questions;
        private readonly bool _recursionAvailable;
        private readonly Bdev.Net.Dns.ReturnCode _returnCode;
        private readonly bool _truncated;

        internal Response(byte[] message)
        {
            int num4;
            byte num = message[2];
            byte num2 = message[3];
            int num3 = num2 & 15;
            if (num3 > 6)
            {
                num3 = 6;
            }
            this._returnCode = (Bdev.Net.Dns.ReturnCode) num3;
            this._authoritativeAnswer = (num & 4) != 0;
            this._recursionAvailable = (num2 & 0x80) != 0;
            this._truncated = (num & 2) != 0;
            this._questions = new Question[GetShort(message, 4)];
            this._answers = new Answer[GetShort(message, 6)];
            this._nameServers = new NameServer[GetShort(message, 8)];
            this._additionalRecords = new AdditionalRecord[GetShort(message, 10)];
            Pointer pointer = new Pointer(message, 12);
            for (num4 = 0; num4 < this._questions.Length; num4++)
            {
                try
                {
                    this._questions[num4] = new Question(pointer);
                }
                catch (Exception exception)
                {
                    throw new InvalidResponseException(exception);
                }
            }
            for (num4 = 0; num4 < this._answers.Length; num4++)
            {
                this._answers[num4] = new Answer(pointer);
            }
            for (num4 = 0; num4 < this._nameServers.Length; num4++)
            {
                this._nameServers[num4] = new NameServer(pointer);
            }
            for (num4 = 0; num4 < this._additionalRecords.Length; num4++)
            {
                this._additionalRecords[num4] = new AdditionalRecord(pointer);
            }
        }

        private static short GetShort(byte[] message, int position)
        {
            return (short) ((message[position] << 8) | message[position + 1]);
        }

        public AdditionalRecord[] AdditionalRecords
        {
            get
            {
                return this._additionalRecords;
            }
        }

        public Answer[] Answers
        {
            get
            {
                return this._answers;
            }
        }

        public bool AuthoritativeAnswer
        {
            get
            {
                return this._authoritativeAnswer;
            }
        }

        public bool MessageTruncated
        {
            get
            {
                return this._truncated;
            }
        }

        public NameServer[] NameServers
        {
            get
            {
                return this._nameServers;
            }
        }

        public Question[] Questions
        {
            get
            {
                return this._questions;
            }
        }

        public bool RecursionAvailable
        {
            get
            {
                return this._recursionAvailable;
            }
        }

        public Bdev.Net.Dns.ReturnCode ReturnCode
        {
            get
            {
                return this._returnCode;
            }
        }
    }
}

