namespace Bdev.Net.Dns
{
    using System;
    using System.Collections;

    public class Request
    {
        private Bdev.Net.Dns.Opcode _opCode = Bdev.Net.Dns.Opcode.StandardQuery;
        private ArrayList _questions = new ArrayList();
        private bool _recursionDesired = true;

        private static void AddDomain(ArrayList data, string domainName)
        {
            int startIndex = 0;
            int num2 = 0;
            while (startIndex < domainName.Length)
            {
                num2 = domainName.IndexOf('.', startIndex) - startIndex;
                if (num2 < 0)
                {
                    num2 = domainName.Length - startIndex;
                }
                data.Add((byte) num2);
                while (num2-- > 0)
                {
                    data.Add((byte) domainName[startIndex++]);
                }
                startIndex++;
            }
            data.Add((byte) 0);
        }

        public void AddQuestion(Question question)
        {
            if (question == null)
            {
                throw new ArgumentNullException("question");
            }
            this._questions.Add(question);
        }

        public byte[] GetMessage()
        {
            ArrayList data = new ArrayList {
                (byte) 0,
                (byte) 0,
                (byte) ((((byte) this._opCode) << 3) | (this._recursionDesired ? 1 : 0)),
                (byte) 0,
                (byte) (this._questions.Count >> 8),
                (byte) this._questions.Count,
                (byte) 0,
                (byte) 0,
                (byte) 0,
                (byte) 0,
                (byte) 0,
                (byte) 0
            };
            foreach (Question question in this._questions)
            {
                AddDomain(data, question.Domain);
                data.Add((byte) 0);
                data.Add((byte) question.Type);
                data.Add((byte) 0);
                data.Add((byte) question.Class);
            }
            byte[] array = new byte[data.Count];
            data.CopyTo(array);
            return array;
        }

        public Bdev.Net.Dns.Opcode Opcode
        {
            get
            {
                return this._opCode;
            }
            set
            {
                this._opCode = value;
            }
        }

        public bool RecursionDesired
        {
            get
            {
                return this._recursionDesired;
            }
            set
            {
                this._recursionDesired = value;
            }
        }
    }
}

