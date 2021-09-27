using M0LTE.WsjtxUdpLib.Messages.Out;
using System;

namespace M0LTE.WsjtxUdpLib.Messages
{
    /*
     * Close         Out/In    6                      quint32
     *                         Id (unique key)        utf8
     *
     *      Close is  sent by  a client immediately  prior to  it shutting
     *      down gracefully. When sent by  a server it requests the target
     *      client to close down gracefully.
     */

    public class CloseMessage : WsjtxMessage, IWsjtxCommandMessageGenerator
    {
        public int SchemaVersion { get; set; }
        public string Id { get; set; }

        public static new WsjtxMessage Parse(byte[] datagram)
        {
            if (!CheckMagicNumber(datagram))
            {
                return null;
            }

            var message = new CloseMessage();

            int cur = MAGIC_NUMBER_LENGTH;

            message.SchemaVersion = (int)DecodeQUInt32(datagram, ref cur);

            var messageType = (MessageType)DecodeQUInt32(datagram, ref cur);

            if (messageType != MessageType.CLOSE_MESSAGE_TYPE)
            {
                return null;
            }

            message.Id = DecodeString(datagram, ref cur);

            return message;
        }

        public byte[] GetBytes() => throw new NotImplementedException();

        public override string ToString() => $"Close id:{Id}";
    }
}
