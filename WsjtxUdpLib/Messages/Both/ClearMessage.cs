﻿using M0LTE.WsjtxUdpLib.Messages.Out;
using System;

namespace M0LTE.WsjtxUdpLib.Messages.Both
{
    /*
     * Clear         Out/In    3                      quint32
     *                         Id (unique key)        utf8
     *                         Window                 quint8 (In only) (not in JTDX)
     *
     *      This message is  send when all prior "Decode"  messages in the
     *      "Band Activity"  window have been discarded  and therefore are
     *      no long available for actioning  with a "Reply" message. It is
     *      sent when the user erases  the "Band activity" window and when
     *      WSJT-X  closes down  normally. The  server should  discard all
     *      decode messages upon receipt of this message.
     *
     *      It may  also be  sent to  a WSJT-X instance  in which  case it
     *      clears one or  both of the "Band Activity"  and "Rx Frequency"
     *      windows.  The Window  argument  can be  one  of the  following
     *      values:
     *
     *         0  - clear the "Band Activity" window (default)
     *         1  - clear the "Rx Frequency" window
     *         2  - clear both "Band Activity" and "Rx Frequency" windows
     */

    public class ClearMessage : WsjtxMessage, IWsjtxCommandMessageGenerator
    {
        public int SchemaVersion { get; set; }
        public string Id { get; set; }
        public byte? Window { get; set; }
        

        public static new WsjtxMessage Parse(byte[] datagram)
        {
            if (!CheckMagicNumber(datagram))
            {
                return null;
            }

            var message = new ClearMessage();

            int cur = MAGIC_NUMBER_LENGTH;

            message.SchemaVersion = (int)DecodeQUInt32(datagram, ref cur);

            var messageType = (MessageType)DecodeQUInt32(datagram, ref cur);

            if (messageType != MessageType.CLEAR_MESSAGE_TYPE)
            {
                return null;
            }

            message.Id = DecodeString(datagram, ref cur);

            if (datagram.Length > cur)
            {
                message.Window = DecodeQUInt8(datagram, ref cur);
            }

            return message;
        }

        public byte[] GetBytes() => throw new NotImplementedException();

        public override string ToString() => $"Clear id:{Id}";
    }
}
