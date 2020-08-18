using System;
using System.Globalization;
using System.Linq;
using System.Net;

namespace M0LTE.WsjtxUdpLib.Messages
{
    public enum MessageType
    {
        HEARTBEAT_MESSAGE_TYPE = 0,
        STATUS_MESSAGE_TYPE = 1,
        DECODE_MESSAGE_TYPE = 2,
        CLEAR_MESSAGE_TYPE = 3,
        QSO_LOGGED_MESSAGE_TYPE = 5,
        CLOSE_MESSAGE_TYPE = 6,
        WSPR_DECODE_MESSAGE_TYPE = 10,
        LOGGED_ADIF_MESSAGE_TYPE = 12,
    }

    public abstract class WsjtxMessage
    {
        protected const int MAGIC_NUMBER_LENGTH = 4;

        public static WsjtxMessage Parse(byte[] datagram)
        {
            if (!CheckMagicNumber(datagram))
            {
                return null;
            }

            int cur = MAGIC_NUMBER_LENGTH;

            int schemaVersion = DecodeQInt32(datagram, ref cur);
            var messageType = (MessageType)DecodeQInt32(datagram, ref cur);

            try
            {
                if (schemaVersion == 2)
                {
                    if (messageType == MessageType.HEARTBEAT_MESSAGE_TYPE)
                    {
                        return HeartbeatMessage.Parse(datagram);
                    }
                    else if (messageType == MessageType.STATUS_MESSAGE_TYPE)
                    {
                        return StatusMessage.Parse(datagram);
                    }
                    else if (messageType == MessageType.DECODE_MESSAGE_TYPE)
                    {
                        return DecodeMessage.Parse(datagram);
                    }
                    else if (messageType == MessageType.CLEAR_MESSAGE_TYPE)
                    {
                        return ClearMessage.Parse(datagram);
                    }
                    else if (messageType == MessageType.QSO_LOGGED_MESSAGE_TYPE)
                    {
                        return QsoLoggedMessage.Parse(datagram);
                    }
                    else if (messageType == MessageType.CLOSE_MESSAGE_TYPE)
                    {
                        return CloseMessage.Parse(datagram);
                    }
                    else if (messageType == MessageType.WSPR_DECODE_MESSAGE_TYPE)
                    {
                        return WsprDecodeMessage.Parse(datagram);
                    }
                    else if (messageType == MessageType.LOGGED_ADIF_MESSAGE_TYPE)
                    {
                        return LoggedAdifMessage.Parse(datagram);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ParseFailureException(messageType, datagram, ex);
            }

            throw new NotImplementedException($"Schema version {schemaVersion}, message type {messageType}");
        }

        private static double RoundToSignificantDigits(double d, int digits)
        {
            if (d == 0)
                return 0;

            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);
            return scale * Math.Round(d / scale, digits);
        }
        protected enum Align
        {
            Left, Right
        }

        protected static string Col(object o, int chars, Align alignment)
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

        protected static int DecodeQInt32(byte[] message, ref int cur)
        {
            var result = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(message, cur));
            cur += sizeof(int);
            return result;
        }

        protected static byte DecodeQUInt8(byte[] message, ref int cur)
        {
            var result = (byte)IPAddress.NetworkToHostOrder(message[cur]);
            cur += sizeof(byte);
            return result;
        }

        static bool reverseByteOrder = BitConverter.IsLittleEndian;

        protected static uint DecodeQUInt32(byte[] message, ref int cur)
        {
            byte[] digits = message.Skip(cur).Take(sizeof(uint)).ToArray();

            uint result = reverseByteOrder
                ? BitConverter.ToUInt32(message.Skip(cur).Take(sizeof(uint)).Reverse().ToArray(), 0)
                : BitConverter.ToUInt32(message, cur);

            cur += sizeof(uint);
            return result;
        }

        protected static uint? DecodeNullableQUInt32(byte[] message, ref int cur)
        {
            try
            {
                if (IsQUInt32MaxValue(message, cur))
                {
                    return null;
                }

                uint result = reverseByteOrder
                    ? BitConverter.ToUInt32(message.Skip(cur).Take(sizeof(uint)).Reverse().ToArray(), 0)
                    : BitConverter.ToUInt32(message, cur);

                return result;
            }
            finally
            {
                cur += sizeof(uint);
            }
        }

        protected static UInt64 DecodeQUInt64(byte[] message, ref int cur)
        {
            var result = reverseByteOrder
                ? BitConverter.ToUInt64(message.Skip(cur).Take(sizeof(UInt64)).Reverse().ToArray(), 0)
                : BitConverter.ToUInt64(message, cur);

            cur += sizeof(UInt64);
            return result;
        }

        protected static double DecodeDouble(byte[] message, ref int cur)
        {
            double result;
            if (reverseByteOrder)
            {
                // x64
                result = BitConverter.ToDouble(message.Skip(cur).Take(sizeof(double)).Reverse().ToArray(), 0);
            }
            else
            {
                // who knows what platform
                result = BitConverter.ToDouble(message, cur);
            }

            cur += sizeof(double);
            return result;
        }

        protected static bool DecodeBool(byte[] message, ref int cur)
        {
            bool result = message[cur] != 0;
            cur += sizeof(bool);
            return result;
        }

        protected static TimeSpan DecodeQTime(byte[] message, ref int cur)
        {
            return TimeSpan.FromMilliseconds(DecodeQInt32(message, ref cur));
        }

        protected static string DecodeString(byte[] message, ref int cur)
        {
            if (IsQUInt32MaxValue(message, cur))
            {
                cur += sizeof(UInt32);
                return null;
            }

            var numBytesInField = DecodeQUInt32(message, ref cur);

            char[] letters = new char[numBytesInField];
            for (int i = 0; i < numBytesInField; i++)
            {
                letters[i] = (char)message[cur + i];
            }

            cur += (int)numBytesInField;

            return new string(letters);
        }

        protected static DateTime DecodeQDateTime(byte[] message, ref int cur)
        {
            /*
             *       QDateTime:
             *           QDate      qint64    Julian day number
             *           QTime      quint32   Milli-seconds since midnight
             *           timespec   quint8    0=local, 1=UTC, 2=Offset from UTC
             *                                                 (seconds)
             *                                3=time zone
             *           offset     qint32    only present if timespec=2
             *           timezone   several-fields only present if timespec=3
             *
             *      we will avoid using QDateTime fields with time zones for simplicity.
             */

            long julianDay = DecodeQInt32(message, ref cur);
            var sinceMidnight = DecodeQTime(message, ref cur);
            byte timespec = DecodeQUInt8(message, ref cur);
            int offset = DecodeQInt32(message, ref cur);

            DateTimeKind kind;
            if (timespec == 0) kind = DateTimeKind.Local;
            else if (timespec == 1) kind = DateTimeKind.Utc;
            else if (timespec == 2) kind = DateTimeKind.Utc;
            else if (timespec == 3) throw new NotSupportedException("timespec=3");
            else throw new NotImplementedException($"timespec={timespec}");

            var date = new DateTime(-4714, 11, 24, 0, 0, 0, kind).AddDays(julianDay);
            
            var result = date.Add(sinceMidnight).AddSeconds(offset);

            cur += sizeof(long) + sizeof(uint) + sizeof(byte) + sizeof(int);

            return result;
        }

        protected static bool IsQUInt32MaxValue(byte[] message, int cur)
            => message[cur] == 0xff && message[cur + 1] == 0xff && message[cur + 2] == 0xff && message[cur + 3] == 0xff;

        protected static bool CheckMagicNumber(byte[] message) => message.Take(4).SequenceEqual(new byte[] { 0xad, 0xbc, 0xcb, 0xda });
    }
}