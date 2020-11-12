using M0LTE.WsjtxUdpLib.Messages;
using M0LTE.WsjtxUdpLib.Messages.Out;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace M0LTE.WsjtxUdpLib.Client
{
    public sealed class WsjtxClient : IDisposable
    {
        private readonly UdpClient udpClient;
        private readonly Action<WsjtxMessage, IPEndPoint> callback;
        private readonly bool debug;

        public WsjtxClient(Action<WsjtxMessage, IPEndPoint> callback, IPAddress ipAddress, int port = 2237, bool multicast = false, bool debug = false)
        {
            if (multicast)
            {
                udpClient = new UdpClient();
                udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, port));
                udpClient.JoinMulticastGroup(ipAddress);
            }
            else
            {
                udpClient = new UdpClient(new IPEndPoint(ipAddress, port));
            }

            this.callback = callback;
            this.debug = debug;
            _ = Task.Run(UdpLoop);
        }

        //Random random = new Random();

        private void UdpLoop()
        {
            var from = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                byte[] datagram = udpClient.Receive(ref from);

                WsjtxMessage msg;

                try
                {
                    msg = WsjtxMessage.Parse(datagram);
                }
                catch (ParseFailureException ex)
                {
                    File.WriteAllBytes($"{ex.MessageType}.couldnotparse.bin", ex.Datagram);
                    Console.WriteLine($"Parse failure for {ex.MessageType}: {ex.InnerException.Message}");
                    continue;
                }

                if (msg != null)
                {
                    if (debug)
                    {
                        WriteToDisk(msg);
                    }
                    /*
                    if (msg is DecodeMessage dm)
                    {
                        var highlightMessage = new HighlightCallsignMessage
                        {
                            Id = "test",
                            Callsign = dm.Message,
                            BackgroundColour = new Colour { Red = 0xff, Green = 0xff, Blue = 0xff },
                            ForegroundColour = new Colour { Red = (byte)random.Next(256), Green = (byte)random.Next(256), Blue = (byte)random.Next(256) },
                            HighlightLast = true
                        };

                        var dg = highlightMessage.GetBytes();
                        udpClient.Send(dg, dg.Length, from);
                    }*/

                    callback(msg, from);
                }
            }
        }

        private void WriteToDisk(WsjtxMessage msg)
        {
            try
            {
                File.WriteAllBytes(msg.GetType().Name, msg.Datagram);
            }
            catch (Exception) { }
        }

        public void Dispose()
        {
            udpClient.Dispose();
        }
    }
}