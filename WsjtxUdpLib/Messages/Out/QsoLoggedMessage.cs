namespace M0LTE.WsjtxUdpLib.Messages
{
    /*
     * QSO Logged    Out       5                      quint32
     *                         Id (unique key)        utf8
     *                         Date & Time Off        QDateTime
     *                         DX call                utf8
     *                         DX grid                utf8
     *                         Tx frequency (Hz)      quint64
     *                         Mode                   utf8
     *                         Report sent            utf8
     *                         Report received        utf8
     *                         Tx power               utf8
     *                         Comments               utf8
     *                         Name                   utf8
     *                         Date & Time On         QDateTime
     *                         Operator call          utf8
     *                         My call                utf8
     *                         My grid                utf8
     *                         Exchange sent          utf8
     *                         Exchange received      utf8
     *
     *      The  QSO logged  message is  sent  to the  server(s) when  the
     *      WSJT-X user accepts the "Log  QSO" dialog by clicking the "OK"
     *      button.
     */

    public class QsoLoggedMessage : WsjtxMessage
    {
        public static new WsjtxMessage Parse(byte[] message)
        {
            return new QsoLoggedMessage();
        }
    }
}
