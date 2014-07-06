using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpBMD.Code;

namespace SharpBMD.Forms
{
    public partial class TextForm : Form
    {
        List<TextLines> textList = new List<TextLines>();

        public TextForm()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                dataGridView1.Rows.Clear();
                textList.Clear();

                using (FileStream FS = File.Open(openFileDialog1.FileName, FileMode.Open))
                {
                    if (FS.Length != 1800000 && FS.Length != 900000)
                        throw new FileLoadException("O tamanho do arquivo é diferente de 1800000 e 900000 bytes");

                    uint Reason = (uint)FS.Length / 3000;
                    byte[] ReadBytes = new byte[300 * (Reason / 300)];
                    for (uint i = 0; i < 3000; i++)
                    {
                        FS.Read(ReadBytes, 0, ReadBytes.Length);
                        TextLines text = new TextLines(ReadBytes);
                        text.Index = textList.Count + 1;
                        textList.Add(text);
                    }
                    if (Reason / 300 == 2) { lbEncoding.Text = "Unicode"; }
                    else { lbEncoding.Text = "ANSI"; }
                }

                for (int i = 0; i < textList.Count; i++)
                    dataGridView1.Rows.Add(textList[i].Index, textList[i].Text);
            }
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            btSalvar.Enabled = true;
            btSalvarComo.Enabled = true;
            btIr.Enabled = true;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            textList[e.RowIndex].Text = dataGridView1[e.ColumnIndex, e.RowIndex].Value as string;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(saveFileDialog1.FileName))
                SaveFile(openFileDialog1.FileName);
            else
                SaveFile(saveFileDialog1.FileName);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                SaveFile(saveFileDialog1.FileName);
        }

        private void SaveFile(string path)
        {
            using (FileStream FS = File.Open(path, FileMode.OpenOrCreate))
            {
                byte[] ConvertedBytes;
                Encoding encoding = comboBox1.SelectedIndex == 0 ? Encoding.Default : Encoding.Unicode;

                using (MemoryStream MS = new MemoryStream())
                {
                    byte[] buffer;
                    for (int i = 0; i < textList.Count; i++)
                    {
                        buffer = textList[i].ConvertEncoding(encoding);
                        MS.Write(buffer, 0, buffer.Length);
                    }
                    ConvertedBytes = MS.GetBuffer();
                }
                if (encoding == Encoding.Default)
                {
                    FS.SetLength(900000);
                    FS.Write(ConvertedBytes, 0, 900000);
                }
                else
                {
                    FS.SetLength(1800000);
                    FS.Write(ConvertedBytes, 0, 1800000);
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btIr_Click(object sender, EventArgs e)
        {
            if (tbIr.Text == string.Empty) { return; }
            int Value = Convert.ToInt32(tbIr.Text);
            if (Value > 0 && Value <= textList.Count)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = Value - 1;
                dataGridView1.Rows[Value - 1].Selected = true;
            }
            else
                MessageBox.Show("O número informado é inválido", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
