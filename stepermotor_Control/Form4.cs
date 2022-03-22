using System;
using System.Windows.Forms;

namespace SPC
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
            if (!(pass.Text == "ZEZE"))
                MessageBox.Show("密碼錯誤!");
            else
            {
                MessageBox.Show("密碼正確!");
            }
        }

        private void pass_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }
    }
}
