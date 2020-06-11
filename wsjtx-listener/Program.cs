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
                if (message is DecodeMessage decoded)
                {
                    var sb = new StringBuilder();
                    sb.Append($"{Col(decoded.SinceMidnight, 8, Align.Left)} ");
                    sb.Append($"{Col(decoded.Snr, 3, Align.Right)} ");
                    sb.Append($"{Col(decoded.DeltaFrequency, 4, Align.Right)} ");
                    sb.Append($"{Col(decoded.DeltaTime, 4, Align.Right)} ");

                    sb.Append($"{Col(decoded.Mode, 1, Align.Left)} ");
                    sb.Append($"{(decoded.LowConfidence ? "LC" : "  ")} ");
                    sb.Append($"{Col(decoded.Message, 20, Align.Left)} ");

                    if (decoded.SchemaVersion != 2)
                    {
                        Debugger.Break();
                    }

                    if (decoded.Id != "WSJT-X")
                    {
                        Debugger.Break();
                    }

                    Console.WriteLine(sb);
                }
            });

            Thread.CurrentThread.Join();
        }

        private static double RoundToSignificantDigits(double d, int digits)
        {
            if (d == 0)
                return 0;

            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);
            return scale * Math.Round(d / scale, digits);
        }

        private static string Col(object o, int chars, Align alignment)
        {
            if (o == null)
            {
                return new string(' ', chars);
            }

            if (o is double d)
            {
                string str = RoundToSignificantDigits(d, chars - 1).ToString();
                if (!str.Contains("."))
                {
                    str += ".0";
                }
                return Col(str, chars, alignment);
            }

            string output = o.ToString();

            if (output.Length > chars)
            {
                if (alignment == Align.Left)
                {
                    return output.Substring(0, chars);
                }
                else
                {
                    return output.Substring(output.Length - chars, chars);
                }
            }
            else if (output.Length == chars)
            {
                return output;
            }
            else
            {
                if (alignment == Align.Left)
                {
                    return output + new string(' ', chars - output.Length);
                }
                else
                {
                    return new string(' ', chars - output.Length) + output;
                }
            }
        }
    }

    enum Align
    {
        Left, Right
    }
}