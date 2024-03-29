﻿using M0LTE.WsjtxUdpLib.Messages.Out;
using System;

namespace M0LTE.WsjtxUdpLib.Messages
{
    public class HeartbeatMessage : WsjtxMessage, IWsjtxCommandMessageGenerator
    {
        /*
         * Message       Direction Value                  Type
         * ------------- --------- ---------------------- -----------
         * Heartbeat     Out/In    0                      quint32
         *                         Id (unique key)        utf8
         *                         Maximum schema number  quint32
         *                         version                utf8
         *                         revision               utf8
         *
         *    The heartbeat  message shall be  sent on a periodic  basis every
         *    NetworkMessage::pulse   seconds   (see    below),   the   WSJT-X
         *    application  does  that  using the  MessageClient  class.   This
         *    message is intended to be used by servers to detect the presence
         *    of a  client and also  the unexpected disappearance of  a client
         *    and  by clients  to learn  the schema  negotiated by  the server
         *    after it receives  the initial heartbeat message  from a client.
         *    The message_aggregator reference server does just that using the
         *    MessageServer class. Upon  initial startup a client  must send a
         *    heartbeat message as soon as  is practical, this message is used
         *    to negotiate the maximum schema  number common to the client and
         *    server. Note  that the  server may  not be  able to  support the
         *    client's  requested maximum  schema  number, in  which case  the
         *    first  message received  from the  server will  specify a  lower
         *    schema number (never a higher one  as that is not allowed). If a
         *    server replies  with a lower  schema number then no  higher than
         *    that number shall be used for all further outgoing messages from
         *    either clients or the server itself.
         *
         *    Note: the  "Maximum schema number"  field was introduced  at the
         *    same time as schema 3, therefore servers and clients must assume
         *    schema 2 is the highest schema number supported if the Heartbeat
         *    message does not contain the "Maximum schema number" field.
         */
        public static new WsjtxMessage Parse(byte[] message)
        {
            if (!CheckMagicNumber(message))
            {
                return null;
            }

            var heartbeatMessage = new HeartbeatMessage();

            int cur = MAGIC_NUMBER_LENGTH;
            heartbeatMessage.SchemaVersion = (int)DecodeQUInt32(message, ref cur);

            var messageType = (MessageType)DecodeQUInt32(message, ref cur);

            if (messageType != MessageType.HEARTBEAT_MESSAGE_TYPE)
            {
                return null;
            }

            heartbeatMessage.Id = DecodeString(message, ref cur);
            heartbeatMessage.MaxSchemaNumber = (int)DecodeQUInt32(message, ref cur);
            heartbeatMessage.Version = DecodeString(message, ref cur);
            if (message.Length > cur) // JTDX
            {
                heartbeatMessage.Revision = DecodeString(message, ref cur);
            }

            return heartbeatMessage;
        }

        public int SchemaVersion { get; set; }
        public string Id { get; set; }
        public int MaxSchemaNumber { get; set; }
        public string Version { get; set; }
        public string Revision { get; set; }

        public override string ToString() => $"Heartbeat id:{Id} maxSchemaNumber:{MaxSchemaNumber} version:{Version}{(Revision == null ? "" : $"revision:{Revision}")}";

        public byte[] GetBytes() => throw new NotImplementedException();
    }
}