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
        public static new WsjtxMessage Parse(byte[] message) => null;

        public byte[] GetBytes() => throw new NotImplementedException();
    }
}
