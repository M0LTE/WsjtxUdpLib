using M0LTE.WsjtxUdpLib;
using M0LTE.WsjtxUdpLib.Messages;
using System;
using Xunit;

namespace WsjtxUdpLibTests
{
    public class EncodeMessageTests
    {
        [Fact]
        public void EncodeTrueBool()
        {
            var bytes = Qt.Encode(true);
            Assert.Equal(new byte[] { 0x01 }, bytes);
        }

        [Fact]
        public void EncodeFalseBool()
        {
            var bytes = Qt.Encode(false);
            Assert.Equal(new byte[] { 0x00 }, bytes);
        }

        [Fact]
        public void EncodeString()
        {
            var bytes = Qt.Encode("hi");
            Assert.Equal(new byte[] { 0, 0, 0, 2, (byte)'h', (byte)'i' }, bytes);
        }

        [Fact]
        public void EncodeUInt32Max()
        {
            var bytes = Qt.Encode(UInt32.MaxValue);
            Assert.Equal(new byte[] { 0xff, 0xff, 0xff, 0xff }, bytes);
        }

        [Fact]
        public void EncodeUInt32One()
        {
            var bytes = Qt.Encode(1);
            Assert.Equal(new byte[] { 0x00, 0x00, 0x00, 0x01 }, bytes);
        }

        [Fact]
        public void EncodeColour()
        {
            /*
                qint8   s = color.cspec;
                quint16 a = color.ct.argb.alpha;
                quint16 r = color.ct.argb.red;
                quint16 g = color.ct.argb.green;
                quint16 b = color.ct.argb.blue;
                quint16 p = color.ct.argb.pad;
             */

            var bytes = Qt.Encode(new Colour { Red = 0xff, Green = 0x00, Blue = 0x00 });
            Assert.Equal(new byte[] {
                0x01,       // spec, RGB
                0xff, 0xff, // alpha, 100%
                0xff, 0x00, // red, 100%
                0x00, 0x00, // green, 0%
                0x00, 0x00, // blue, 0%
                0x00, 0x00, // pad
            }, bytes);
        }

        [Fact]
        public void EncodeMessage()
        {
            var msg = new HighlightCallsignMessage
            {
                Id = "test",
                Callsign = "J",
                BackgroundColour = new Colour { Red = 0xff, Green = 0x00, Blue = 0x00 },
                ForegroundColour = new Colour { Red = 0x00, Green = 0x00, Blue = 0xff },
                HighlightLast = false
            };

            var dg = msg.GetBytes();

            Assert.Equal(new byte[] {
                0,0,0,13, // message type 13
                0,0,0,4, (byte)'t',(byte)'e',(byte)'s',(byte)'t', // Id
                0,0,0,1, (byte)'J',  // callsign

                // background
                0x01,       // spec, RGB
                0xff, 0xff, // alpha, 100%
                0xff, 0x00, // red, 100%
                0x00, 0x00, // green, 0%
                0x00, 0x00, // blue, 0%
                0x00, 0x00, // pad

                // colour
                0x01,       // spec, RGB
                0xff, 0xff, // alpha, 100%
                0x00, 0x00, // red, 0%
                0x00, 0x00, // green, 0%
                0xff, 0x00, // blue, ff%
                0x00, 0x00, // pad

                0x00 // highlight last
            }, dg);
        }
    }
}