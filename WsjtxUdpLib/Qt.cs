using M0LTE.WsjtxUdpLib.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace M0LTE.WsjtxUdpLib
{
    public static class Qt
    {
        public static bool ReverseByteOrder = BitConverter.IsLittleEndian;
        private static readonly byte[] QUInt32MaxValue = new byte[] { 0xff, 0xff, 0xff, 0xff };

        public static byte[] Concat(params byte[][] arrays)
        {
            var result = new List<byte>();

            foreach (byte[] array in arrays)
            {
                result.AddRange(array);
            }

            return result.ToArray();
        }

        public static byte[] Encode(UInt32 i)
        {
            var bytes = BitConverter.GetBytes(i);

            return ReverseByteOrder ? bytes.Reverse().ToArray() : bytes;
        }

        public static byte[] Encode(bool b)
        {
            return new byte[] { (byte)(b ? 0x01 : 0x00) };
        }

        public static byte[] Encode(string s)
        {
            if (s == null)
            {
                return QUInt32MaxValue;
            }

            var len = Encode((uint)s.Length);
            var utf8 = Encoding.UTF8.GetBytes(s);

            var bytes = Concat(len, utf8);

            return bytes;
        }

        private static readonly byte[] RgbQColorFormat = new byte[] { 0x01 };

        public static byte[] Encode(Colour c)
        {
            return Concat(RgbQColorFormat,
                new byte[] { 0xff, 0xff }, // alpha
                new byte[] { c.Red, 0x00 },
                new byte[] { c.Green, 0x00 },
                new byte[] { c.Blue, 0x00 },
                new byte[] { 0x00, 0x00 }); // padding
        }
    }
}
