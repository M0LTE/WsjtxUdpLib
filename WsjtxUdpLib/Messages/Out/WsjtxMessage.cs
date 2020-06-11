using System;
using System.Linq;
using System.Net;

namespace M0LTE.WsjtxUdpLib.Messages
{
    public abstract class WsjtxMessage
    {
        protected const int DECODE_MESSAGE_TYPE = 2;

        public static WsjtxMessage Parse(byte[] datagram)
        {
            if (!CheckMagicNumber(datagram))
            {
                return null;
            }

            int cur = 4; // length of magic number

            int schemaVersion = GetInt32(datagram, ref cur);
            int messageType = GetInt32(datagram, ref cur);

            if (schemaVersion == 2)
            {
                if (messageType == 0)
                {
                    return HeartbeatMessage.Parse(datagram);
                }
                else if (messageType == 1)
                {
                    return StatusMessage.Parse(datagram);
                }
                else if (messageType == DECODE_MESSAGE_TYPE)
                {
                    return DecodeMessage.Parse(datagram);
                }
                else if (messageType == 3)
                {
                    return ClearMessage.Parse(datagram);
                }
                else if (messageType == 5)
                {
                    return QsoLoggedMessage.Parse(datagram);
                }
                else if (messageType == 6)
                {
                    return CloseMessage.Parse(datagram);
                }
                else if (messageType == 10)
                {
                    return WsprDecodeMessage.Parse(datagram);
                }
                else if (messageType == 12)
                {
                    return LoggedAdifMessage.Parse(datagram);
                }
            }

            throw new NotImplementedException($"Schema version {schemaVersion}, message type {messageType}");
        }

        protected static int GetInt32(byte[] message, ref int cur)
        {
            int result = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(message, cur));
            cur += sizeof(int);
            return result;
        }

        protected static double GetDouble(byte[] message, ref int cur)
        {
            double result;
            if (BitConverter.IsLittleEndian)
            {
                // x64
                result = BitConverter.ToDouble(message.Skip(cur).Take(sizeof(double)).Reverse().ToArray(), 0);
            }
            else
            {
                // who knows what
                result = BitConverter.ToDouble(message, cur);
            }

            cur += sizeof(double);
            return result;
        }

        protected static bool GetBool(byte[] message, ref int cur)
        {
            bool result = message[cur] != 0;
            cur += sizeof(bool);
            return result;
        }

        protected static string GetString(byte[] message, ref int cur)
        {
            int numBytesInField = GetInt32(message, ref cur);

            char[] letters = new char[numBytesInField];
            for (int i = 0; i < numBytesInField; i++)
            {
                letters[i] = (char)message[cur + i];
            }

            cur += numBytesInField;

            return new string(letters);
        }

        protected static bool CheckMagicNumber(byte[] message) => message.Take(4).SequenceEqual(new byte[] { 0xad, 0xbc, 0xcb, 0xda });
    }
}
