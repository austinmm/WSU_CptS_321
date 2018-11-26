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

namespace Spreadsheet
{
    public partial class Form1 : Form
    {
        private CptS321.Spreadsheet spreadsheet;
        private int Rows = 50;
        private int Columns = 26;

        public Form1()
        {
            InitializeComponent();
            dataGridView1_Initialize();
        }
        private void dataGridView1_Initialize()
        {
            this.spreadsheet = new CptS321.Spreadsheet(this.Columns, this.Rows);
            this.spreadsheet.CellPropertyChanged += spreadsheet_CellPropertyChanged;
            this.Initialize_DataGrid();
        }

        //private void GenerateRandomValues()
        //{
        //    for (int j = 0; j < this.Rows; j++)
        //    {
        //        //Column B
        //        this.spreadsheet.CellArray[1, j].Text = $"This is cell B{j+1}";
        //        //Column A
        //        this.spreadsheet.CellArray[0, j].Text = $"=B{j+1}";
        //    }

        //    Random rand = new Random();
        //    for (int i = 0; i < 50; i++)
        //    {
        //        int column = rand.Next(26);
        //        int row = rand.Next(50);
        //        if(String.IsNullOrWhiteSpace(this.spreadsheet.CellArray[column, row].Text))
        //        {
        //            this.spreadsheet.CellArray[column, row].Text = "Hello World!";
        //        }
        //        else { i--; }
        //    }
        //}


        private void spreadsheet_CellPropertyChanged(object sender, EventArgs e)
        {
            CptS321.Cell cell = sender as CptS321.Cell;
            this.dataGridView1[cell.ColumnIndex, cell.RowIndex].Value = cell.Value;
        }

        private void Initialize_DataGrid()
        {
            for (int i = 0; i < this.Columns; i++)
            {
                string colName = Convert.ToChar(65 + i).ToString();
                this.dataGridView1.Columns.Add(colName, colName);
                this.dataGridView1.Columns[i].HeaderText = colName;
            }
            for (int i = 0; i < this.Rows; i++)
            {
                this.dataGridView1.Rows.Add(1);
                this.dataGridView1.Rows[i].HeaderCell.Value = $"{i+1}";
            }
        }

        private void ClearSpreadSheet()
        {
            this.dataGridView1_Initialize();
            for (int i = 0; i < this.Columns; i++)
            {
                for (int j = 0; j < this.Rows; j++)
                {
                    this.dataGridView1[i, j].Value = String.Empty;
                    this.dataGridView1[i, j].Style.BackColor = Color.White;
                }
            }
        }

        private void ComputeAllCells()
        {
            for (int i = 0; i < this.Columns; i++)
            {
                for (int j = 0; j < this.Rows; j++)
                {
                    if (this.spreadsheet.CellArray[i, j].IsValueSet())
                    {
                        this.dataGridView1[i, j].Value = this.spreadsheet.CellArray[i, j].ComputeValue();
                        this.SetCellBG(i,j);
                    }
                }
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            CptS321.Cell cell = this.spreadsheet.CellArray[e.ColumnIndex, e.RowIndex];
            this.dataGridView1[e.ColumnIndex, e.RowIndex].Value = cell.Value;
            this.toolStripTextBox1.Text = $"{cell.Position}={cell.Value}";
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            object text = this.dataGridView1[e.ColumnIndex, e.RowIndex].Value;
            if(text == null) { text = String.Empty; }
            CptS321.Cell cell = this.spreadsheet.CellArray[e.ColumnIndex, e.RowIndex];
            cell.Text = text.ToString();
            string value = String.IsNullOrWhiteSpace(cell.Errors) ? cell.ComputeValue().ToString() : cell.Errors;
            this.dataGridView1[e.ColumnIndex, e.RowIndex].Value = value;
            this.toolStripTextBox1.Text = String.Empty;
            this.ComputeAllCells();
        }
        private void SetCellBG(int column, int row)
        {
            if (this.dataGridView1[column, row].Style.BackColor.IsKnownColor)
            {
                try
                {
                    if (String.IsNullOrWhiteSpace(this.spreadsheet.CellArray[column, row].BGColor))
                    {
                        this.spreadsheet.CellArray[column, row].BGColor = ColorTranslator.ToHtml(this.dataGridView1[column, row].Style.BackColor);
                    }
                    else
                    {
                        string color = this.spreadsheet.CellArray[column, row].BGColor;
                        if (!color.Contains("#")) { color = $"#{color}"; }
                        this.dataGridView1[column, row].Style.BackColor = ColorTranslator.FromHtml(color);
                    }
                }
                catch (Exception) { }
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Helper Code from: https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.filedialog?view=netframework-4.7.2
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Removes all data from spreadsheet 
                    this.ClearSpreadSheet();
                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();
                    spreadsheet.Load(fileStream);
                    this.ComputeAllCells();
                    fileStream.Close();
                    fileStream.Dispose();
                }
                openFileDialog.Dispose();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Helper Code from: https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.filedialog?view=netframework-4.7.2
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    string filePath = openFileDialog.FileName;
                    //Read the contents of the file into a stream
                    FileStream fileStream = new FileStream(filePath, FileMode.Create);
                    spreadsheet.Save(fileStream);
                    fileStream.Close();
                    fileStream.Dispose();
                }
                openFileDialog.Dispose();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
 
        }
    }
}