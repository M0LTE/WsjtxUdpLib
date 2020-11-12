using M0LTE.WsjtxUdpLib.Client;
using System;
using System.Net;
using System.Threading;

namespace wsjtx_listener
{
    class Program
    {
        static void Main(string[] args)
        {
            using var client = new WsjtxClient((msg, from) =>
            {
                Console.WriteLine(msg);
            }, IPAddress.Parse("239.255.0.1"), multicast: true, debug: true);

            Thread.CurrentThread.Join();
        }
    }
}