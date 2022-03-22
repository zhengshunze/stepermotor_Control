using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SPC
{

    public partial class Form2 : Form
    {

        int curr_x, curr_y;
        bool isWndMove;
        DateTime loginTime;
        string UserName;


        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
                   int left,
                   int top,
                   int right,
                   int bottom,
                   int width,
                   int height
               );

        [DllImport("winmm.dll", EntryPoint = "sndPlaySoundA")]
        public static extern long sndPlaySound(String SoundName, int Flags);





        public Form2()
        {
            InitializeComponent();

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            pictureBox4.Hide();
            pictureBox5.Hide();
          

            // Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 12, 12));


        }

        private void button1_Click(object sender, EventArgs e)
        {
            //|| txtUserName.Text == "admin"

            if ((txtUserName.Text == "") && txtpassword.Text == "")
            {
                loginTime = DateTime.Now;
                UserName = txtUserName.Text;
                LogMsg();
                Object selUserName = txtUserName.Text;
                sndPlaySound("C:/succ.wav", 1);
                MessageBox.Show(selUserName.ToString() + "您已成功登入!");
                new Form1().Show();
                this.Hide();
            }
            else
            {
                sndPlaySound("C:/error.wav", 1);
                MessageBox.Show("您輸入的帳號或者密碼不正確，請重新輸入!");
                txtUserName.Clear();
                txtpassword.Clear();
                txtUserName.Focus();
            }

        }

        private void LogMsg()
        {
            StreamWriter sw = new StreamWriter(@"C:\log.txt", true);
            sw.WriteLine($"使用者名稱:{UserName}\n登入時間:{loginTime.ToString()}");
            sw.Close();
        }


        private void label5_Click(object sender, EventArgs e)
        {
            txtUserName.Clear();
            txtpassword.Clear();
            txtUserName.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtpassword_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {

                this.button1_Click(sender, e);

            }

            if (e.Control && e.KeyCode == Keys.V)
            {
                e.Handled = true;
            }
        }
        /*
                private void txtUserName_MouseLeave(object sender, EventArgs e)
                {
                    // txtUserName.Focus();
                }

                private void txtpassword_MouseMove(object sender, MouseEventArgs e)
                {
                    //txtpassword.Focus();
                }
        */
        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.isWndMove)
                this.Location = new Point(this.Left + e.X - this.curr_x, this.Top + e.Y - this.curr_y);
        }

        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            this.isWndMove = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.isWndMove)
                this.Location = new Point(this.Left + e.X - this.curr_x, this.Top + e.Y - this.curr_y);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            this.isWndMove = false;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.curr_x = e.X;
                this.curr_y = e.Y;
                this.isWndMove = true;
            }
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.curr_x = e.X;
                this.curr_y = e.Y;
                this.isWndMove = true;
            }
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.isWndMove)
                this.Location = new Point(this.Left + e.X - this.curr_x, this.Top + e.Y - this.curr_y);
        }

        private void label1_MouseUp(object sender, MouseEventArgs e)
        {
            this.isWndMove = false;
        }

        private void txtUserName_MouseClick(object sender, MouseEventArgs e)
        {
            sndPlaySound("C:/loading_account.wav", 1);

        }

        private void txtpassword_MouseClick(object sender, MouseEventArgs e)
        {
            sndPlaySound("C:/loading_password.wav", 1);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void txtUserName_MouseEnter(object sender, EventArgs e)
        {

        }

        private void txtUserName_MouseHover(object sender, EventArgs e)
        {
            pictureBox2.Visible = true;

        }

        private void txtUserName_MouseEnter_1(object sender, EventArgs e)
        {
            pictureBox2.Show();

        }

        private void txtUserName_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.Hide();

        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            pictureBox2.Show();

        }


        private void txtpassword_MouseHover(object sender, EventArgs e)
        {
            pictureBox3.Visible = true;

        }

        private void txtpassword_MouseEnter(object sender, EventArgs e)
        {
            pictureBox3.Show();
        }

        private void txtpassword_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.Hide();
        }

        private void txtpassword_TextChanged(object sender, EventArgs e)
        {
            pictureBox3.Show();
        }

        private void txtpassword_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void txtUserName_TextChanged_1(object sender, EventArgs e)
        {
            pictureBox4.Show();
        }

        private void txtUserName_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox4.Show();


        }
        private void txtUserName_MouseLeave_1(object sender, EventArgs e)
        {
            pictureBox4.Hide();
        }

        private void txtUserName_TextChanged_2(object sender, EventArgs e)
        {
            pictureBox4.Show();
        }

        private void txtpassword_TextChanged_1(object sender, EventArgs e)
        {
            pictureBox5.Show();
        }

        private void txtpassword_MouseLeave_1(object sender, EventArgs e)
        {
            pictureBox5.Hide();
        }

        private void txtpassword_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox5.Show();
        }



        private void pictureBox6_MouseDown(object sender, MouseEventArgs e)
        {





        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtpassword.PasswordChar = '\0';

                 checkBox1.BackgroundImage = Properties.Resources.eye2;
            }
            else
            {
                txtpassword.PasswordChar = '*';
                checkBox1.BackgroundImage = Properties.Resources.eye;
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                this.curr_x = e.X;
                this.curr_y = e.Y;
                this.isWndMove = true;
            }
        }


    }

}