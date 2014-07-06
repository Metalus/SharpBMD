using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpBMD.Code.Item;

namespace SharpBMD.Forms
{
    public partial class ItemForm : Form
    {
        ItemInfo[,] Items;

        public ItemForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Items = new ItemInfo[16, 512];
                using(FileStream FS = File.Open(openFileDialog1.FileName,FileMode.Open))
                {
                    if (FS.Length != 917508 && FS.Length != 688132)
                        throw new FileLoadException("O tamanho do arquivo é diferente de 1800000 e 900000 bytes");

                    uint Reason = (uint)((FS.Length - 4) / 8192);
                    byte[] ReadBytes = new byte[Reason];
                    for (int i = 0; i < 8192; i++)
                    {
                        FS.Read(ReadBytes, 0, ReadBytes.Length);
                        int IndexItem = ( 512 + i) % 512;
                        int ClassItem = (i-IndexItem) / 512;
                        ItemInfo item = new ItemInfo(ReadBytes, (TypeItem)ClassItem, IndexItem);
                        Items[ClassItem, IndexItem] = item;
                    }
                }
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int IndexClass = comboBox1.SelectedIndex;
            int IndexItem = comboBox2.SelectedIndex;
            byte[] unknown = Items[IndexClass, IndexItem].unknown;
            StringBuilder rich = new StringBuilder();
            for (int i = 0; i < unknown.Length; i++)
            {
                rich.Append(Convert.ToInt32(unknown[i]).ToString());
                if (i != unknown.Length - 1)
                    rich.Append(" - ");
            }

            richTextBox1.Text = rich.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            int IndexClass = comboBox1.SelectedIndex > 0 ? comboBox1.SelectedIndex : 0;

            for (int i = 0; i < 512; i++)
                comboBox2.Items.Add(Items[IndexClass, i].NameItem);
        }
    }
}
