using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpBMD.Code
{
    public class TextLines
    {
        public int Index { get; set; }
        public string Text { get; set; }

        public byte[] ConvertEncoding(Encoding encoding)
        {
            byte[] TextEncoded = encoding.GetBytes(Text);
            byte[] FinalTextEncoded;
            if (encoding == Encoding.Default) // ANSI
            {
                FinalTextEncoded = new byte[300];
                TextEncoded.CopyTo(FinalTextEncoded, 0);

                for (int i = 0; i < FinalTextEncoded.Length; i++)
                    FinalTextEncoded[i] ^= Constants.MaskAnsi[i % 3];
            }
            else //Unicode
            {
                FinalTextEncoded = new byte[600];
                TextEncoded.CopyTo(FinalTextEncoded, 0);

                for (int i = 0; i < FinalTextEncoded.Length; i++)
                    FinalTextEncoded[i] ^= Constants.MaskUnicode[i % 6];
            }

            return FinalTextEncoded;
        }

        public TextLines(byte[] Serialized)
        {
            if (Serialized.Length == 600) //Unicode
            {
                for (int i = 0; i < Serialized.Length; i++)
                    Serialized[i] ^= Constants.MaskUnicode[i % 6]; 

                Text = Encoding.Unicode.GetString(Serialized); 
            }

            else //ANSI
            {
                for (int i = 0; i < Serialized.Length; i++)
                    Serialized[i] ^= Constants.MaskAnsi[i % 3]; 

                Text = Encoding.Default.GetString(Serialized);
            }
        }
    }
}
