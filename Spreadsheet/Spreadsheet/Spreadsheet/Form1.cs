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

        private void button1_Click(object sender, EventArgs e)
        {
            //this.ClearSpreadSheet();
            //this.GenerateRandomValues();
        }

        //private void ClearSpreadSheet()
        //{
        //    for (int i = 0; i < this.Columns; i++)
        //    {
        //        for (int j = 0; j < this.Rows; j++)
        //        {
        //            this.spreadsheet.CellArray[i,j].Text = "";
        //        }
        //    }
        //}

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            CptS321.Cell cell = this.spreadsheet.CellArray[e.ColumnIndex, e.RowIndex];
            this.dataGridView1[e.ColumnIndex, e.RowIndex].Value = cell.Value;
            this.textBox1.Text = $"{cell.Position}={cell.Value}";
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            object text = this.dataGridView1[e.ColumnIndex, e.RowIndex].Value;
            if(text != null)
            {
                CptS321.Cell cell = this.spreadsheet.CellArray[e.ColumnIndex, e.RowIndex];
                cell.Text = text.ToString();
                string value = String.IsNullOrWhiteSpace(cell.Errors) ? cell.ComputeValue().ToString() : cell.Errors;
                this.dataGridView1[e.ColumnIndex, e.RowIndex].Value = value;
            }
            this.textBox1.Text = String.Empty;
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex < this.Columns && e.ColumnIndex > -1)
                && (e.RowIndex < this.Rows && e.RowIndex > -1))
                {
                object text = this.dataGridView1[e.ColumnIndex, e.RowIndex].Value;
                if (text != null)
                {
                    CptS321.Cell cell = this.spreadsheet.CellArray[e.ColumnIndex, e.RowIndex];
                    string value = String.IsNullOrWhiteSpace(cell.Errors) ? cell.ComputeValue().ToString() : cell.Errors;
                    this.dataGridView1[e.ColumnIndex, e.RowIndex].Value = value;
                    this.textBox1.Text = $"{cell.Position}={cell.Value}";
                }
                else { this.textBox1.Text = String.Empty; }
            }
        }
    }
}
