namespace Bdev.Net.Dns
{
    using System;
    using System.Collections;
    using System.Net;
    using System.Net.Sockets;

    public sealed class Resolver
    {
        private const int _dnsPort = 0x35;
        private const int _udpRetryAttempts = 2;
        private static int _uniqueId;

        private Resolver()
        {
        }

        public static Response Lookup(Request request, IPAddress dnsServer)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            if (dnsServer == null)
            {
                throw new ArgumentNullException("dnsServer");
            }
            IPEndPoint server = new IPEndPoint(dnsServer, 0x35);
            byte[] message = request.GetMessage();
            return new Response(UdpTransfer(server, message));
        }

        public static MXRecord[] MXLookup(string domain, IPAddress dnsServer)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            if (dnsServer == null)
            {
                throw new ArgumentNullException("dnsServer");
            }
            Request request = new Request();
            request.AddQuestion(new Question(domain, DnsType.MX, DnsClass.IN));
            Response response = Lookup(request, dnsServer);
            if (response == null)
            {
                return null;
            }
            ArrayList list = new ArrayList();
            foreach (Answer answer in response.Answers)
            {
                if (answer.Record.GetType() == typeof(MXRecord))
                {
                    list.Add(answer.Record);
                }
            }
            MXRecord[] array = new MXRecord[list.Count];
            list.CopyTo(array);
            Array.Sort<MXRecord>(array);
            return array;
        }

        private static byte[] UdpTransfer(IPEndPoint server, byte[] requestMessage)
        {
            int num = 0;
            while (num <= 2)
            {
                requestMessage[0] = (byte) (_uniqueId >> 8);
                requestMessage[1] = (byte) _uniqueId;
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 0x3e8);
                socket.SendTo(requestMessage, requestMessage.Length, SocketFlags.None, server);
                byte[] buffer = new byte[0x200];
                try
                {
                    socket.Receive(buffer);
                    if ((buffer[0] == requestMessage[0]) && (buffer[1] == requestMessage[1]))
                    {
                        return buffer;
                    }
                }
                catch (SocketException)
                {
                    num++;
                }
                finally
                {
                    _uniqueId++;
                    socket.Close();
                }
            }
            throw new NoResponseException();
        }
    }
}

