namespace M0LTE.WsjtxUdpLib.Messages
{
    /*
     * WSPRDecode    Out       10                     quint32
     *                         Id (unique key)        utf8
     *                         New                    bool
     *                         Time                   QTime
     *                         snr                    qint32
     *                         Delta time (S)         float (serialized as double)
     *                         Frequency (Hz)         quint64
     *                         Drift (Hz)             qint32
     *                         Callsign               utf8
     *                         Grid                   utf8
     *                         Power (dBm)            qint32
     *                         Off air                bool
     *
     *      The decode message is sent when  a new decode is completed, in
     *      this case the 'New' field is true. It is also used in response
     *      to  a "Replay"  message where  each  old decode  in the  "Band
     *      activity" window, that  has not been erased, is  sent in order
     *      as  a one  of  these  messages with  the  'New'  field set  to
     *      false.  See   the  "Replay"  message  below   for  details  of
     *      usage. The off air field indicates that the decode was decoded
     *      from a played back recording.
     */

    public class WsprDecodeMessage : WsjtxMessage
    {
        public static new WsjtxMessage Parse(byte[] message)
        {
            return new WsprDecodeMessage();
        }
    }
}
