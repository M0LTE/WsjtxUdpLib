using M0LTE.WsjtxUdpLib.Client;
using M0LTE.WsjtxUdpLib.Messages;
using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;

namespace wsjtx_listener
{
    class Program
    {
        static void Main(string[] args)
        {
            using var client = new WsjtxClient(Console.WriteLine, IPAddress.Parse("239.1.2.3"), multicast: true, debug: true);

            Thread.CurrentThread.Join();
        }
    }
}