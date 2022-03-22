using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Threading;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Drawing.Imaging;

namespace SPC
{
    public partial class Form1 : Form

    {
        DateTime logoutTime;
        double first_motor_move, second_motor_move, third_motor_move;
        double first_motor_moveTo, second_motor_moveTo, third_motor_moveTo;
        double first_motor_setMaxSpeed, second_motor_setMaxSpeed, third_motor_setMaxSpeed;
        double first_motor_setAcc, second_motor_setAcc, third_motor_setAcc;
        bool isCOMConnected = false;
        bool isswitch = true;

        delegate void DisplayS1(Byte[] buffer);
        delegate void DisplayS2(Byte[] buffer);

        VideoCapture capture;

        //Capture cap; //只適用於emgu3.0


        public Form1()
        {
            InitializeComponent();
            //Application.Idle += new EventHandler(Application_processing);只適用於emgu3.0
            button2.Enabled = false;
            button3.Enabled = false;
            tabControl1.Enabled = false;
            panel1.Visible = false;
            panel2.Visible = false;
        }
        //以下為方法二(只適用於emgu3.0的版本emgu4.0不支援
        /*
        private void Application_processing(object sender, EventArgs e)
        {
            
             Image<Bgr, byte> frame;
            frame = cap.QueryFrame().ToImage<Bgr, byte>();
            pictureBox1.Image = frame.ToBitmap();
            
        Image<Bgr, byte> frame = cap.QueryFrame().ToImage<Bgr, byte>();
            pictureBox1.Image = frame.ToBitmap();
        }
        */
        //以上為方法二
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new Size(645, 600);
            textBox2.Text = "";
            groupBox2.Visible = true;
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            int itemsFound = 0;
            portComboBox.Items.Clear();

            foreach (string port in SerialPort.GetPortNames())
            {
                portComboBox.Items.Add(port);
                itemsFound++;
                button1.Enabled = true;
            }

            if (itemsFound < 1)
            {
                MessageBox.Show("沒有任何端口被找到，重新掃描!", "訊息視窗");
                button2.Enabled = false;
            }
            else
            {
                button2.Enabled = true;
                portComboBox.SelectedIndex = 0;
            }



        }



        private void button2_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = portComboBox.Text;
            try

            {
                isCOMConnected = true;
                serialPort1.DtrEnable = true;
                serialPort1.BaudRate = 500000;

                serialPort1.Open();
                button2.Enabled = false;
                button2.Text = "已連線...";
                Object selectedItem = serialPort1.PortName;
                MessageBox.Show(selectedItem.ToString() + "端口已連線", "訊息視窗");
                button3.Enabled = true;
                button1.Enabled = false;
                tabControl1.Enabled = true;
            }

            catch (Exception err)

            {

                MessageBox.Show(err.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {


            int BTR = (sender as SerialPort).BytesToRead;
            if (BTR > 0)
            {
                Byte[] buffer = new Byte[BTR];
                Int32 length = (sender as SerialPort).Read(buffer, 0, buffer.Length);
                Array.Resize(ref buffer, length);
                DisplayS1 d1 = new DisplayS1(DisplayTextS1);
                this.Invoke(d1, new Object[] { buffer });



            }
            try
            {
                this.IncomingData = this.serialPort1.ReadLine();
                this.Invoke(new Action(this.DataProcessing));
            }
            catch
            {

            }



        }



        string IncomingData;

        private void DataProcessing()
        {

            if (this.isCOMConnected)
            {
                try
                {
                    // this.textBox2.Text = this.textBox2.Text + this.IncomingData.ToString() + Environment.NewLine;


                    string str = this.IncomingData.Substring(0, 1);

                    if (str != null)
                    {

                        if (str == "C")

                        {

                            label6.Text = this.IncomingData.Substring(1).ToString();


                        }

                        if (str == "P")

                        {
                            label15.Text = this.IncomingData.Substring(1).ToString();


                        }

                        if (str == "T")

                        {
                            label17.Text = this.IncomingData.Substring(1).ToString();

                        }


                    }
                }
                catch
                {

                }
                /*
                if (IncomingData.Length > 4)
                {
                    string[] arr = IncomingData.Split('\n');
                    foreach (var item in arr)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            arr[0] = label6.Text;
                            arr[1] = label15.Text;
                            arr[2] = label17.Text;
                        }
                    }
                }
                */
            }


        }

        private void DisplayTextS1(byte[] buffer)
        {
            string result = System.Text.Encoding.UTF8.GetString(buffer);
            textBox2.AppendText(result);


        }

        string data;
        private void DisplayTextS2(byte[] buffer)
        {
            string result = System.Text.Encoding.UTF8.GetString(buffer);

            /*
            data = data + result;
            string[] arr = data.Split('\n');
            foreach (var item in arr)
                if (item.Trim().StartsWith("C"))
                    label6.Text = arr[0];
                else if (item.Trim().StartsWith("P"))
                    label15.Text = arr[1];
                else if (item.Trim().StartsWith("T"))
                    label17.Text = arr[2];
            */
        }

        private void portComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Object selectedItem = serialPort1.PortName;

            serialPort1.Close();
            button2.Enabled = true;
            MessageBox.Show(selectedItem.ToString() + "端口已斷線", "訊息視窗");
            button2.Text = "連線";
            portComboBox.Enabled = true;
            button1.Enabled = true;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }



        private void button5_Click(object sender, EventArgs e)
        {
            if (this.isCOMConnected)
            {

                // this.serialPort1.Write("d1");
                //Thread.Sleep(100);
                this.serialPort1.Write("x" + textBox4.Text.ToString());
                Thread.Sleep(1000);
                this.serialPort1.Write("a" + textBox5.Text.ToString());
                Thread.Sleep(1000);
                this.serialPort1.Write("v" + textBox6.Text.ToString());

            }

        }



        private void LogMsg()
        {
            StreamWriter sw = new StreamWriter(@"C:\log.txt", true);//set the log path
            sw.WriteLine($"登出時間:{logoutTime.ToString()}");//log message
            sw.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {

            try
            {
                serialPort1.Write(textBox3.Text);


            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            label15.Text = "00000";
            label6.Text = "00000";
            label17.Text = "00000";
            textBox1.Text = "00000";
            textBox16.Text = "00000";
            textBox17.Text = "00000";


        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                button6.PerformClick();
                // textBox3.Clear();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (this.isCOMConnected)
            {

                this.serialPort1.Write("S");







            }
        }


        private void button14_Click(object sender, EventArgs e)
        {
            if (this.isCOMConnected)
            {
                this.ParseTextBoxes();
                this.serialPort1.Write("a" + this.first_motor_setAcc.ToString());
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (this.isCOMConnected)
            {
                this.ParseTextBoxes();
                this.serialPort1.Write("y" + " 0 " + " " + this.second_motor_setMaxSpeed.ToString());
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (this.isCOMConnected)
            {
                this.ParseTextBoxes();
                this.serialPort1.Write("b" + "0" + " " + this.second_motor_setAcc.ToString() + " " + "0");
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (this.isCOMConnected)
            {
                this.ParseTextBoxes();
                this.serialPort1.Write("z" + "0 " + "0 " + this.third_motor_setMaxSpeed.ToString());
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (this.isCOMConnected)
            {
                this.ParseTextBoxes();
                this.serialPort1.Write("c" + "0" + " " + "0" + " " + this.third_motor_setAcc.ToString());
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {


            DialogResult Result = MessageBox.Show("確定關閉此程式嗎？", "訊息視窗", MessageBoxButtons.OKCancel);

            if (Result == DialogResult.OK)
            {
                serialPort1.Close();
                isCOMConnected = false;
                new Form2().Show();

                logoutTime = DateTime.Now;
                LogMsg();

            }
            else
            {
                e.Cancel = true;
            }

        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (this.isCOMConnected)
            {
                this.button21.PerformClick();
                Thread.Sleep(1000);
                this.button20.PerformClick();
                Thread.Sleep(1000);
                this.button25.PerformClick();







                /*
                this.serialPort1.Write("d3");
                Thread.Sleep(1000);
                this.serialPort1.Write("z " + "0 " + "0 " + textBox15.Text.ToString());
                Thread.Sleep(1000);
                this.serialPort1.Write("c " + "0 " + "0 " + textBox14.Text.ToString());
                Thread.Sleep(1000);
                this.serialPort1.Write("t " + "0 " + "0 " + textBox13.Text.ToString());
                */


                // this.serialPort1.Write("x" + " " + textBox15.Text.ToString() + " " + "0" + " " + "0" + "a" + textBox14.Text.ToString() + " " + "0" + " " + "0" + "d3");
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            if (this.isCOMConnected)
            {

                this.button16.PerformClick();
                Thread.Sleep(1000);
                this.button15.PerformClick();
                Thread.Sleep(1000);
                this.button18.PerformClick();

                /*
                this.serialPort1.Write("d2");
                Thread.Sleep(1000);
                this.serialPort1.Write("y" + " 0 " + " " + textBox11.Text.ToString());
                Thread.Sleep(1000);
                this.serialPort1.Write("b" + "0" + " " + textBox10.Text.ToString());
                Thread.Sleep(1000);
                this.serialPort1.Write("e" + "0" + " " + textBox9.Text.ToString());
                */
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.isCOMConnected)
            {
                this.serialPort1.Write("1");
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            if (this.isCOMConnected)
            {
                this.serialPort1.Write("2");
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            if (this.isCOMConnected)
            {
                this.serialPort1.Write("3");
            }
        }

        private void button1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }


        private void button31_Click(object sender, EventArgs e)
        {
            Object selectedItem = serialPort1.PortName;

            serialPort1.Close();
            button2.Enabled = true;
            MessageBox.Show(selectedItem.ToString() + "端口已斷線", "訊息視窗");
            button2.Text = "連線";
            portComboBox.Enabled = true;
            button1.Enabled = true;
        }

        private void button34_Click(object sender, EventArgs e)
        {
            Object selectedItem = serialPort1.PortName;

            serialPort1.Close();
            button2.Enabled = true;
            MessageBox.Show(selectedItem.ToString() + "端口已斷線", "訊息視窗");
            button2.Text = "連線";
            portComboBox.Enabled = true;
            button1.Enabled = true;
        }

        private void button35_Click(object sender, EventArgs e)
        {

        }

        private void button36_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (this.isCOMConnected)
            {

                this.serialPort1.Write("n" + textBox4.Text.ToString() + " " + textBox11.Text.ToString() + " " + textBox15.Text.ToString());
                //Thread.Sleep(1000);
                this.serialPort1.Write("q" + textBox5.Text.ToString() + " " + textBox10.Text.ToString() + " " + textBox14.Text.ToString());
                //Thread.Sleep(1000);
                this.serialPort1.Write("h" + textBox6.Text.ToString() + " " + textBox9.Text.ToString() + " " + textBox13.Text.ToString());
                // Thread.Sleep(1000);
                if (checkBox1.Checked)
                {
                    serialPort1.Write("R");
                }



            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.isCOMConnected)
                {
                    this.ParseTextBoxes();
                    this.serialPort1.Write("U");
                    label6.Text = "00000";
                    textBox2.Text = "";
                }
            }
            catch
            {

            }

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            if (this.isCOMConnected)
            {
                try
                {
                    this.ParseTextBoxes();
                    this.serialPort1.Write("D");
                    label15.Text = "00000";
                    textBox2.Text = "";
                }
                catch
                {

                }
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            if (this.isCOMConnected)
            {
                try
                {
                    this.ParseTextBoxes();
                    this.serialPort1.Write("B");
                    label17.Text = "00000";
                    textBox2.Text = "";
                }
                catch
                {

                }
            }
        }



        private void button31_Click_1(object sender, EventArgs e)
        {
            try { this.serialPort1.Write("C"); } catch { };

        }

        private void button30_Click(object sender, EventArgs e)
        {
            try { this.serialPort1.Write("P"); } catch { };
        }

        private void button29_Click(object sender, EventArgs e)
        {
            try { this.serialPort1.Write("T"); } catch { };
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            textBox2.Select(textBox2.TextLength, 0);
            textBox2.ScrollToCaret();

            int Line = textBox2.Lines.Length;

            if (Line > 4)
            {
                int start = textBox2.GetFirstCharIndexFromLine(0);

                int end = textBox2.GetFirstCharIndexFromLine(1);

                textBox2.Select(start, end);

                textBox2.SelectedText = "";

            }
        }

        private void Form1_FormClosed_1(object sender, FormClosedEventArgs e)
        {
            this.serialPort1.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string blah = textBox2.Text;
            //textBox1.Clear();
            var lst = blah.ToUpper().Split('\n').ToList();
            foreach (var item in lst)
                if (item.Trim().StartsWith("C"))
                    textBox1.AppendText($"{item.Remove(0, 1)}\n");
                else if (item.Trim().StartsWith("P"))
                    textBox16.AppendText($"{item.Remove(0, 1)}\n");
                else if (item.Trim().StartsWith("T"))
                    textBox17.AppendText($"{item.Remove(0, 1)}\n");

            textBox2.Select(textBox2.TextLength, 0);
            textBox2.ScrollToCaret();

            int Line = textBox2.Lines.Length;

            if (Line > 4)
            {
                int start = textBox2.GetFirstCharIndexFromLine(0);

                int end = textBox2.GetFirstCharIndexFromLine(1);

                textBox2.Select(start, end);

                textBox2.SelectedText = "";

            }
        }




        private void textBox1_TextChanged_2(object sender, EventArgs e)
        {
            textBox1.Select(textBox2.TextLength, 0);


            int Line = textBox1.Lines.Length;

            if (Line > 2)
            {
                int start = textBox1.GetFirstCharIndexFromLine(0);

                int end = textBox1.GetFirstCharIndexFromLine(1);

                textBox1.Select(start, end);

                textBox1.SelectedText = "";

            }
        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox16_TextChanged_1(object sender, EventArgs e)
        {
            textBox16.Select(textBox2.TextLength, 0);
            //textBox16.ScrollToCaret();

            int Line = textBox16.Lines.Length;

            if (Line > 2)
            {
                int start = textBox16.GetFirstCharIndexFromLine(0);

                int end = textBox16.GetFirstCharIndexFromLine(1);

                textBox16.Select(start, end);

                textBox16.SelectedText = "";

            }
        }

        private void textBox17_TextChanged_1(object sender, EventArgs e)
        {
            textBox17.Select(textBox2.TextLength, 0);


            int Line = textBox17.Lines.Length;

            if (Line > 2)
            {
                int start = textBox17.GetFirstCharIndexFromLine(0);

                int end = textBox17.GetFirstCharIndexFromLine(1);

                textBox17.Select(start, end);

                textBox17.SelectedText = "";

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                this.groupBox2.Visible = true;
                if (isCOMConnected)
                {
                    serialPort1.Write("R");
                }
               
            }
            else
            {
                this.groupBox2.Visible = false;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label20.Text = trackBar1.Value.ToString();


        }


        private void button4_Click_1(object sender, EventArgs e)
        {
            if (panel1.Visible == false)
            {
                if (panel2.Visible == false)
                {
                    Form4 form = new Form4();
                    DialogResult res = form.ShowDialog();
                    if (res == DialogResult.Yes)
                    {
                        if (form.pass.Text == "ZEZE")
                        {
                            panel1.Visible = true;
                            this.Size = new Size(645, 800);
                            button4.Text = "關閉設定及紀錄";

                        }
                    }
                }
                else
                {
                    Form4 form = new Form4();
                    DialogResult res = form.ShowDialog();
                    if (res == DialogResult.Yes)
                    {
                        if (form.pass.Text == "ZEZE")
                        {
                            panel1.Visible = true;
                            this.Size = new Size(1300, 800);
                            button4.Text = "關閉設定及紀錄";
                        }

                    }

                }

            }
            else
            {
                if (panel2.Visible == false)
                {
                    panel1.Visible = false;
                    this.Size = new Size(645, 600);
                    button4.Text = "開啟設定及紀錄";
                }
                else
                {

                    panel1.Visible = false;
                    this.Size = new Size(1300, 600);
                    button4.Text = "開啟設定及紀錄";
                }


            }
        }

        private void button26_Click_1(object sender, EventArgs e)
        {
            if (panel2.Visible == false)
            {

                if (panel1.Visible == false)
                {

                    panel2.Visible = true;
                    this.Size = new Size(1300, 600);
                    button26.Text = "關閉攝像頭";

                }
                else
                {

                    panel2.Visible = true;
                    this.Size = new Size(1302, 800);
                    button26.Text = "關閉攝像頭";

                }
            }
            else
            {
                if (panel1.Visible == false)
                {
                    panel2.Visible = false;
                    this.Size = new Size(645, 600);
                    button26.Text = "開啟攝像頭";
                }
                else
                {
                    panel2.Visible = false;
                    this.Size = new Size(645, 800);
                    button26.Text = "開啟攝像頭";
                }
            }
        }


        private void button27_Click_1(object sender, EventArgs e)
        {


            if (isswitch)
            {
                capture = new Emgu.CV.VideoCapture(0);
                capture.ImageGrabbed += Capture_ImageGrabbed;
                capture.Start();
                button27.Text = "關閉畫面";
                button27.BackColor = Color.Red;
            }
            else
            {
                capture.Dispose();
                button27.Text = "開啟畫面";
                button27.BackColor = Color.FromArgb(100, 149, 237);
                Thread.Sleep(1000);
            }
            isswitch = !isswitch;

        }

        private void Capture_ImageGrabbed(object sender, EventArgs e)
        {
            try
            {
                Mat m = new Mat();
                capture.Retrieve(m);
                pictureBox1.Image = m.ToImage<Bgr, byte>().Bitmap;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }

        private void button32_Click(object sender, EventArgs e)
        {

        }

        private void button33_Click(object sender, EventArgs e)
        {


            capture.Dispose();


        }

        private void button32_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!isswitch)
                {
                    if (capture != null)
                    {

                        SaveFileDialog sav = new SaveFileDialog();
                        sav.Filter = "JPEG files(*.Jpeg)|*.Jpeg";
                        capture.Stop();
                        timer1.Stop();

                        if (DialogResult.OK == sav.ShowDialog())
                        {

                            if (pictureBox1.Image != null)
                            {

                                if (sav.FileName != "")
                                {
                                    pictureBox1.Image.Save(sav.FileName, ImageFormat.Jpeg);
                                }
                            }
                        }

                        capture.Start();
                        timer1.Start();
                    }
                    else
                    {
                        MessageBox.Show("請先開啟攝像頭!");
                    }
                }
                else
                {
                    MessageBox.Show("請先開啟攝像頭!");
                }
            }
            catch
            {
                //NothingToDo
            }
        }



        private void button34_Click_1(object sender, EventArgs e)
        {
          


            /*
                var grayImage = new Image<Gray, byte>((Bitmap)pictureBox1.Image);
                double threshval = (double)this.numericUpDown1.Value;
                var threshImge = grayImage.CopyBlank();
                CvInvoke.Threshold(grayImage, threshImge, threshval, 255, ThresholdType.Otsu);
                pictureBox2.Image = threshImge.ToBitmap();
            */
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button36_Click_1(object sender, EventArgs e)
        {
            pictureBox2.Image = pictureBox1.Image;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {


        }
        bool doloop = true;
        private void button34_MouseDown(object sender, MouseEventArgs e)
        {
            if (doloop)
            {
                timer1.Interval = 30;
                timer1.Tick += doloopthing;
                timer1.Start();
                button34.Text = "停止二值化處理";
            }
            else
            {
                timer1.Stop();
                button34.Text = "開始二值化處理";
                Thread.Sleep(1000);

            }
            doloop = !doloop;
        }

        private void doloopthing(object sender, EventArgs e)
        {
            if (capture != null)
            {
                if (!doloop)
                {
                    try
                    {
                        var grayImage = new Image<Gray, byte>((Bitmap)pictureBox1.Image);
                        // double threshval = (double)this.numericUpDown1.Value;
                        var threshImge = grayImage.CopyBlank();
                        CvInvoke.Threshold(grayImage, threshImge, 0, 255, ThresholdType.Otsu);
                         pictureBox2.Image = threshImge.ToBitmap();
                        

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.InnerException.Message);
                    }
                }


            }

        }

        private void button34_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void button35_Click_1(object sender, EventArgs e)
        {

            SaveFileDialog sav = new SaveFileDialog();
            sav.Filter = "JPEG files(*.Jpeg)|*.Jpeg";

            try
            {
                if (DialogResult.OK == sav.ShowDialog())

                    if (pictureBox2.Image != null)
                    {
                        capture.Stop();
                        timer1.Stop();
                        if (sav.FileName != "")
                        {

                            pictureBox2.Image.Save(sav.FileName, ImageFormat.Jpeg);

                        }
                        capture.Start();
                        timer1.Start();
                    }

            }
            catch
            {

            }
        }

        private void button33_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog Opfile = new OpenFileDialog();
            Opfile.Filter = "JPG files(*.Jpg)|*.Jpg";
            if (DialogResult.OK == Opfile.ShowDialog())
            {
                this.pictureBox1.Image = new Bitmap(Opfile.FileName);
              
            }
        }

        private void button37_Click(object sender, EventArgs e)
        {

        }

        private void button38_Click(object sender, EventArgs e)
        {
         
        }

        //以下為方法二
        /*
        bool _streaming;
        private void button33_Click_1(object sender, EventArgs e)
        {
            if (!_streaming)
            {
                Application.Idle += streaming;

            }
            else
            {
                Application.Idle -= streaming;

            }
            _streaming = !_streaming;
        }

        private void streaming(object sender, EventArgs e)
        {
            var img = capture.QueryFrame().ToImage<Bgr, byte>();
            var bmp = img.Bitmap;
            pictureBox1.Image = bmp;
        }
        */
        //以上為方法二

        private void button11_Click(object sender, EventArgs e)
        {
            if (this.isCOMConnected)
            {

                this.textBox2.Clear();
                this.ParseTextBoxes();
                this.serialPort1.Write("v" + this.first_motor_moveTo.ToString());

                button31.PerformClick();
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {

            if (this.isCOMConnected)
            {
                this.textBox2.Clear();
                this.ParseTextBoxes();
                this.serialPort1.Write("t" + "0" + " " + "0" + " " + this.third_motor_moveTo.ToString());
                serialPort1.Write("T");

            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (this.isCOMConnected)
            {
                this.ParseTextBoxes();
                this.serialPort1.Write("m" + "0" + " " + "0" + " " + this.third_motor_move.ToString());
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (this.isCOMConnected)
            {
                this.ParseTextBoxes();
                this.serialPort1.Write("x" + this.first_motor_setMaxSpeed.ToString());
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (this.isCOMConnected)
            {
                this.textBox2.Clear();
                this.ParseTextBoxes();
                this.serialPort1.Write("e" + "0" + " " + this.second_motor_moveTo.ToString());

                serialPort1.Write("P");
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (this.isCOMConnected)
            {
                this.ParseTextBoxes();
                this.serialPort1.Write("m" + "0" + " " + this.second_motor_move.ToString() + " " + "0");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (this.isCOMConnected)
            {
                this.ParseTextBoxes();
                this.serialPort1.Write("m" + this.first_motor_move.ToString());
            }
        }

        private void ParseTextBoxes()
        {
            double.TryParse(this.textBox6.Text, out this.first_motor_moveTo);
            double.TryParse(this.textBox9.Text, out this.second_motor_moveTo);
            double.TryParse(this.textBox13.Text, out this.third_motor_moveTo);

            double.TryParse(this.textBox7.Text, out this.first_motor_move);
            double.TryParse(this.textBox8.Text, out this.second_motor_move);
            double.TryParse(this.textBox12.Text, out this.third_motor_move);

            double.TryParse(this.textBox4.Text, out this.first_motor_setMaxSpeed);
            double.TryParse(this.textBox11.Text, out this.second_motor_setMaxSpeed);
            double.TryParse(this.textBox15.Text, out this.third_motor_setMaxSpeed);
            double.TryParse(this.textBox5.Text, out this.first_motor_setAcc);
            double.TryParse(this.textBox10.Text, out this.second_motor_setAcc);
            double.TryParse(this.textBox14.Text, out this.third_motor_setAcc);
        }
    }
}
