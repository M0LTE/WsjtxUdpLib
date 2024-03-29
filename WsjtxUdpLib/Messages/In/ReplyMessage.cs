﻿using System;

namespace M0LTE.WsjtxUdpLib.Messages
{
    /*
     * Reply         In        4                      quint32
     *                         Id (target unique key) utf8
     *                         Time                   QTime
     *                         snr                    qint32
     *                         Delta time (S)         float (serialized as double)
     *                         Delta frequency (Hz)   quint32
     *                         Mode                   utf8
     *                         Message                utf8
     *                         
     *                         WSJT-X only:
     *                         Low confidence         bool
     *                         Modifiers              quint8
     *
     *      In order for a server  to provide a useful cooperative service
     *      to WSJT-X it  is possible for it to initiate  a QSO by sending
     *      this message to a client. WSJT-X filters this message and only
     *      acts upon it  if the message exactly describes  a prior decode
     *      and that decode  is a CQ or QRZ message.   The action taken is
     *      exactly equivalent to the user  double clicking the message in
     *      the "Band activity" window. The  intent of this message is for
     *      servers to be able to provide an advanced look up of potential
     *      QSO partners, for example determining if they have been worked
     *      before  or if  working them  may advance  some objective  like
     *      award progress.  The  intention is not to  provide a secondary
     *      user  interface for  WSJT-X,  it is  expected  that after  QSO
     *      initiation the rest  of the QSO is carried  out manually using
     *      the normal WSJT-X user interface.
     *
     *      The  Modifiers   field  allows  the  equivalent   of  keyboard
     *      modifiers to be sent "as if" those modifier keys where pressed
     *      while  double-clicking  the  specified  decoded  message.  The
     *      modifier values (hexadecimal) are as follows:
     *
     *          no modifier     0x00
     *          SHIFT           0x02
     *          CTRL            0x04  CMD on Mac
     *          ALT             0x08
     *          META            0x10  Windows key on MS Windows
     *          KEYPAD          0x20  Keypad or arrows
     *          Group switch    0x40  X11 only
     */

    public class ReplyMessage : IWsjtxCommandMessageGenerator
    {
        public byte[] GetBytes() => throw new NotImplementedException();
    }
}
