using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpBMD.Code
{
    public static class Constants
    {
        public static byte[] MaskAnsi = new byte[] { 0xFC, 0xCF, 0xAB };
        public static byte[] MaskUnicode = new byte[] { 0x56, 0xCD, 0x93, 0x1D, 0x81, 0x5B }; // Apenas para Text.BMD
    }
}
