using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Test_SREC
{
    public partial class Form1 : Form
    {
        private StringBuilder sRecordBuilder = new StringBuilder();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (float.TryParse(textBox1.Text, out float A) &&
            float.TryParse(textBox2.Text, out float B) &&
            float.TryParse(textBox3.Text, out float C) &&
            float.TryParse(textBox4.Text, out float D) &&
            float.TryParse(textBox5.Text, out float X))
            {
                float result = (float)Math.Pow(X + A, B) * C + D;
                textBox6.Text = result.ToString();

                sRecordBuilder.Clear();

                string sRecordContent = string.Format("S1{0:X2}0000", 1); 
                sRecordContent += $"{BitConverter.GetBytes(A)[0]:X2}{BitConverter.GetBytes(A)[1]:X2}";
                sRecordContent += $"{BitConverter.GetBytes(B)[0]:X2}{BitConverter.GetBytes(B)[1]:X2}";
                sRecordContent += $"{BitConverter.GetBytes(C)[0]:X2}{BitConverter.GetBytes(C)[1]:X2}";
                sRecordContent += $"{BitConverter.GetBytes(D)[0]:X2}{BitConverter.GetBytes(D)[1]:X2}";
                sRecordContent += $"{BitConverter.GetBytes(X)[0]:X2}{BitConverter.GetBytes(X)[1]:X2}";
                sRecordContent += $"{BitConverter.GetBytes(result)[0]:X2}{BitConverter.GetBytes(result)[1]:X2}";

             
                byte checksum = 0;
                for (int i = 0; i < sRecordContent.Length / 2; i++)
                {
                    string hexByte = sRecordContent.Substring(i * 2, 2);
                    if (byte.TryParse(hexByte, System.Globalization.NumberStyles.HexNumber, null, out byte byteValue))
                    {
                        checksum += byteValue;
                    }
                }
                checksum = (byte)(~checksum);

                sRecordContent += $"{checksum:X2}";

                sRecordBuilder.AppendLine(sRecordContent);

                string fileName = "output.srec";

                if (!File.Exists(fileName))
                {
                    File.WriteAllText(fileName, sRecordBuilder.ToString(), Encoding.ASCII);
                    label6.Show();
                    label6.Text = $"SRecord file '{fileName}' created.";
                }
                else
                {
                    File.AppendAllText(fileName, sRecordContent + Environment.NewLine, Encoding.ASCII);
                    label6.Show();
                    label6.Text = $"SRecord appended to '{fileName}'.";
                }

                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
            }
            else
            {
                MessageBox.Show("Invalid input. Please enter valid numeric values.");
            }
        }
        private float CalculateResult(float A, float B, float C, float D, float X)
        {
            return (float)Math.Pow(X + A, B) * C + D;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label6.Hide();
            if (float.TryParse(textBox1.Text, out float A) &&
         float.TryParse(textBox2.Text, out float B) &&
         float.TryParse(textBox3.Text, out float C) &&
         float.TryParse(textBox4.Text, out float D) &&
         float.TryParse(textBox5.Text, out float X))
            {
                float result = CalculateResult(A, B, C, D, X);
                textBox6.Text= result.ToString();
            }
            else
            {
                MessageBox.Show("Invalid input. Please enter valid numeric values.");
            }

        }
    }
}
