using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpBMD.Code.Item
{
    public class ItemInfo
    {
        public TypeItem Type { get; private set; }
        public int Index { get; set; }
        public string NameItem { get; set; }
        public bool ActiveItem { get; set; }
        public byte[] unknown { get; set; }

        public ItemInfo(byte[] Serialized, TypeItem typeItem, int IndexItem)
        {
            for (int i = 0; i < Serialized.Length; i++)
                Serialized[i] ^= Constants.MaskAnsi[i % 3]; //Deixar como ANSI

            if (Serialized.Length == 112) // Unicode
            {
                NameItem = Encoding.Unicode.GetString(Serialized, 0, 64);
                unknown = new byte[Serialized.Length - 64];
                Array.Copy(Serialized, 64, unknown, 0, unknown.Length);
            }
            else // ANSI
            {
                NameItem = Encoding.Default.GetString(Serialized, 0, 32);
                unknown = new byte[Serialized.Length - 32];
                Array.Copy(Serialized, 32, unknown, 0, unknown.Length);
            }

            this.ActiveItem = string.IsNullOrEmpty(NameItem);
            this.Type = typeItem;
            this.Index = IndexItem;
        }
    }
}
