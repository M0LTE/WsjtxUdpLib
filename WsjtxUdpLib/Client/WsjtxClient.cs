﻿using M0LTE.WsjtxUdpLib.Messages;
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
        private readonly Action<WsjtxMessage> callback;

        public WsjtxClient(Action<WsjtxMessage> callback)
        {
            udpClient = new UdpClient(new IPEndPoint(IPAddress.Loopback, 2237));
            this.callback = callback;
            _ = Task.Run(UdpLoop);
        }

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
                    callback(msg);
                }
            }
        }

        public void Dispose()
        {
            udpClient.Dispose();
        }
    }
}