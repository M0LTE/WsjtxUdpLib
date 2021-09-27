using System;

namespace M0LTE.WsjtxUdpLib.Messages
{
    /*
     * JTDX only:
     * 
     * TriggerCQ    In         51                     quint32
     *                         Id (unique key)        utf8
     *                         Direction              utf8
     *                         Tx period              bool
     *                         Send                   bool
     *
     *      The  triggerCQ   message  is  dedicated  to  set CQ direction,
     *      TX period  and  optionally  trigger  CQ  message  transmission 
     *      in  JTDX  from  external  software    through    the   network
     *      connection.  Directional  CQ  is  also  being  supported where 
     *      direction is two-char combination in the range AA..ZZ.
     *      TX period is equivalent to TX first in the Status UDP message,
     *      where  'true'  value  shall  correspond  to  'TX 00/30' second 
     *      in FT8 mode and 'TX even' minute in JT65/JT9/T10 modes.
     *
     *      If the "Send" flag is set  then  CQ message  will be generated 
     *      and Enable Tx button will be switched on.
     */

    public class TriggerCqMessage : IWsjtxCommandMessageGenerator
    {
        public byte[] GetBytes() => throw new NotImplementedException();
    }
}
