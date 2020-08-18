using M0LTE.WsjtxUdpLib.Client;
using M0LTE.WsjtxUdpLib.Messages;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace wsjtx_listener
{
    class Program
    {
        static void Main(string[] args)
        {
            using var client = new WsjtxClient(message =>
            {
                Console.WriteLine(message);
            });

            Thread.CurrentThread.Join();
        }
    }
}