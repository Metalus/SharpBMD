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
        private const int ITEMS_TYPE = 16;
        private const int ITEMS_PERTYPE = 512;
        private const int ITEMS_TOTAL = ITEMS_TYPE * ITEMS_PERTYPE;

        public ItemForm()
        {
            InitializeComponent();
            //
            for (int i = 0; i < 50; i++)
            {
                string name = "unk" + i;
                dataGridView1.Columns.Add(name, name);
            }
            dataGridView1.Rows.Add(ITEMS_TOTAL);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using(FileStream FS = File.Open(openFileDialog1.FileName,FileMode.Open))
                {
                    if (FS.Length != 917508 && FS.Length != 688132)
                        throw new FileLoadException("O tamanho do arquivo é diferente de 917508 e 688132 bytes");

                    uint Reason = (uint)((FS.Length - 4) / ITEMS_TOTAL);
                    byte[] ReadBytes = new byte[Reason];

                    for (int i = 0; i < ITEMS_TOTAL; i++)
                    {
                        FS.Read(ReadBytes, 0, ReadBytes.Length);
                        int IndexItem = (ITEMS_PERTYPE + i) % ITEMS_PERTYPE;
                        int ClassItem = (i - IndexItem) / ITEMS_PERTYPE;
                        ItemInfo item = new ItemInfo(ReadBytes, (TypeItem)ClassItem, IndexItem);

                        DataGridViewRow row = dataGridView1.Rows[i];
                        row.Cells["CIndex"].Value = i;
                        row.Cells["CName"].Value = item.NameItem;
                        for(int j = 0; j < item.unknown.Length; j++)
                        {
                            row.Cells[j + 2].Value = Convert.ToInt32(item.unknown[j]).ToString();
                        }
                    }
                }
                comboBox1.Enabled = true;
            }
        }
       
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int IndexClass = comboBox1.SelectedIndex > 0 ? comboBox1.SelectedIndex * ITEMS_PERTYPE : 0;

            dataGridView1.FirstDisplayedScrollingRowIndex = IndexClass;

        }
    }
}
