using System;
using System.ComponentModel;

namespace CptS321
{
    internal class SpreadsheetCell : Cell
    {
        public SpreadsheetCell(int rowIndex, int columnIndex) 
            : base(rowIndex, columnIndex){}

        //Update inherited value variable
        public void setValue(string text)
        {
            this.value = text;
        }
    }

    public class Spreadsheet
    {
        public event PropertyChangedEventHandler CellPropertyChanged;

        //2D array of reference values to Abstract Cell class
        public Cell[,] CellArray;

        private readonly int columnCount;
        public int ColumnCount { get { return this.columnCount; } }

        private readonly int rowCount;
        public int RowCount { get { return this.rowCount; } }

        public Spreadsheet(int columns, int rows)
        {
            this.rowCount = rows;
            this.columnCount = columns;
            this.CellArray = new SpreadsheetCell[columns, rows];
            for(int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    this.CellArray[i,j] = new SpreadsheetCell(i,j);
                    this.CellArray[i, j].PropertyChanged += this.Spreadsheet_PropertyChanged;
                }
            }
        }

        public Cell GetCell(int column, int row)
        {
            try
            {
                Cell cell = this.CellArray[column, row];
                return cell;
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        private string CalculateValue(string text)
        {
            string value;
            //i.e. "=A2"
            while(!String.IsNullOrWhiteSpace(text) && text[0] == '=')
            {
                //i.e. "A2"
                value = text.Substring(1);
                int column = Convert.ToInt32(value[0]) - 65;//65 = ascii 'A'
                value = value.Substring(1);
                Int32.TryParse(value.ToString(), out int row);
                text = this.GetCell(column, row-1).Value;
            }
            value = text;
            return value;
        }
        //Invoked when a cell's data changes
        private void Spreadsheet_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SpreadsheetCell cell = sender as SpreadsheetCell; //Sets Cell type to subclass type 
            cell.setValue(CalculateValue(cell.Text)); //Updates the cells value if necessary
            if(this.CellPropertyChanged != null)
            {
                this.CellPropertyChanged(sender, e); //Passes reference to cell that changed
            }
        }

    }
}
