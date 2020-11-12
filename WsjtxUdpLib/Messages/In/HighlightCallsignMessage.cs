namespace M0LTE.WsjtxUdpLib.Messages
{
    /*
     * Highlight Callsign In   13                     quint32
     *                         Id (unique key)        utf8
     *                         Callsign               utf8
     *                         Background Color       QColor
     *                         Foreground Color       QColor
     *                         Highlight last         bool
     *
     *      The server  may send  this message at  any time.   The message
     *      specifies  the background  and foreground  color that  will be
     *      used  to  highlight  the  specified callsign  in  the  decoded
     *      messages  printed  in the  Band  Activity  panel.  The  WSJT-X
     *      clients maintain a list of such instructions and apply them to
     *      all decoded  messages in the  band activity window.   To clear
     *      and  cancel  highlighting send  an  invalid  QColor value  for
     *      either or both  of the background and  foreground fields. When
     *      using  this mode  the  total number  of callsign  highlighting
     *      requests should be limited otherwise the performance of WSJT-X
     *      decoding may be  impacted. A rough rule of thumb  might be too
     *      limit the  number of active  highlighting requests to  no more
     *      than 100.
     *
     *      The "Highlight last"  field allows the sender  to request that
     *      all instances of  "Callsign" in the last  period only, instead
     *      of all instances in all periods, be highlighted.
     */

    /// <summary>
    /// This message specifies the background and foreground color that will be used to highlight the specified callsign
    /// in the decoded messages printed in the Band Activity panel. The WSJT-X clients maintain a list of such 
    /// instructions and apply them to all decoded messages in the band activity window. To clear and cancel highlighting
    /// send  an invalid  QColor value  for either or both of the background and foreground fields.
    /// </summary>
    public class HighlightCallsignMessage : WsjtxMessage, IWsjtxCommandMessageGenerator
    {
        public string Id { get; set; }
        /// <summary>
        /// The part of the message to highlight. Not necessarily just the callsign.
        /// </summary>
        public string Callsign { get; set; }
        public Colour BackgroundColour { get; set; }
        public Colour ForegroundColour { get; set; }
        /// <summary>
        /// The "Highlight last" field allows the sender to request that all instances of "Callsign" in the last period only, instead of all instances in all periods, be highlighted.
        /// </summary>
        public bool HighlightLast { get; set; }

        public byte[] GetBytes()
        {
            return Qt.Concat(
                MAGIC_NUMBER,
                SCHEMA_VERSION,
                Qt.Encode((uint)MessageType.HIGHLIGHT_CALLSIGN_MESSAGE_TYPE),
                Qt.Encode(Id),
                Qt.Encode(Callsign),
                Qt.Encode(BackgroundColour),
                Qt.Encode(ForegroundColour),
                Qt.Encode(HighlightLast));
        }
    }

    public class Colour
    {
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
    }
}
