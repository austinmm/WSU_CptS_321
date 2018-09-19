using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
//Austin Marino-11507852
namespace NotepadApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void LoadText(TextReader sr)
        {
            //Make the richTextBox1's Text equal to all of the text in the OpenFileDialog's FileName(filename)
            richTextBox1.Text = sr.ReadToEnd();
        }
        private void loadFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Object used to obtain file information
            using (OpenFileDialog open = new OpenFileDialog())
            {
                //Set the Filename of the OpenFileDailog to empty
                open.FileName = "";
                //Prompts the user for the file location in their file explorer
                if (open.ShowDialog(this) == DialogResult.OK)
                {
                    //Declare filename as a String equal to the OpenFileDialog's FileName
                    string fileName = open.FileName;
                    //Obtains all text within file
                    TextReader txt = File.OpenText(fileName);
                    //writes all text to the textbox, overwritting any other text in it
                    this.LoadText(txt);
                    //closes the file
                    txt.Close();
                }
                else
                {
                    //Unable to open file and grab file path
                    return;
                }
            }
        }

        private void loadFibonacciNumbersfirst50ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FibonacciTextReader fib = new FibonacciTextReader(50);
            this.LoadText(fib);
        }

        private void loadFibonacciNumbersfirst100ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FibonacciTextReader fib = new FibonacciTextReader(100);
            this.LoadText(fib);
        }

        private void saveToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = "";
            //Object used to obtain file information
            using (OpenFileDialog open = new OpenFileDialog())
            {
                //Opens a file from the file explorer
                if (open.ShowDialog(this) == DialogResult.OK)
                {
                    //Declare filename as a String equal to the OpenFileDialog's FileName
                    fileName = open.FileName;
                }
                else
                {
                    //Unable to open file and grab file path
                    return;
                }
            }
            //Writes all text currently in the textbox to the file
            //It overwrites any text already existant in file
            string text = richTextBox1.Text;
            File.WriteAllText(fileName, text);
        }
    }
}
