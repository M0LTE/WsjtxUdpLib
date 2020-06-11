namespace M0LTE.WsjtxUdpLib.Messages
{
    /*
     * Logged ADIF    Out      12                     quint32
     *                         Id (unique key)        utf8
     *                         ADIF text              utf8
     *
     *      The  logged ADIF  message is  sent to  the server(s)  when the
     *      WSJT-X user accepts the "Log  QSO" dialog by clicking the "OK"
     *      button. The  "ADIF text" field  consists of a valid  ADIF file
     *      such that  the WSJT-X  UDP header information  is encapsulated
     *      into a valid ADIF header. E.g.:
     *
     *          <magic-number><schema-number><type><id><32-bit-count>  # binary encoded fields
     *          # the remainder is the contents of the ADIF text field
     *          <adif_ver:5>3.0.7
     *          <programid:6>WSJT-X
     *          <EOH>
     *          ADIF log data fields ...<EOR>
     *
     *      Note that  receiving applications can treat  the whole message
     *      as a valid ADIF file with one record without special parsing.
     */

    public class LoggedAdifMessage : WsjtxMessage
    {
        public static new WsjtxMessage Parse(byte[] message)
        {
            return null;
        }
    }
}
