using System;

namespace M0LTE.WsjtxUdpLib.Messages
{
    /*
     * JTDX only:
     * 
     * SetTxDeltaFreq  In      50                     quint32
     *                         Id (unique key)        utf8
     *                         TX delta frequency     quint32
     *
     *      Setting TX delta frequency in JTDX.  Received  value  will  be
     *      checked against widegraph frequency range,  it will be ignored
     *      if it does not fit there.
     */

    public class SetTxDeltaFreqMessage : IWsjtxCommandMessageGenerator
    {
        public byte[] GetBytes() => throw new NotImplementedException();
    }
}
