using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spreadsheet
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.textBox1.Text = "Spreadsheet\r\nVersion 4.0\r\nCpts 321 Spreadsheet\r\nAustin marino - austin.marino@wsu.edu";
            //<img alt='Creative Commons License' style='border-width:0' src='https://i.creativecommons.org/l/by/4.0/80x15.png' />
            this.webBrowser1.DocumentText = "<div align='center' style='font-size:1.3em;'>&copy;&ensp;2018-<strong>&#x221e;</strong> Austin Marino</div>";
        }
    }
}
