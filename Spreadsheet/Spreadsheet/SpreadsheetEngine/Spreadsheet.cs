using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace CptS321
{
    public class SpreadsheetCell : Cell
    {
        private Dictionary<string, SpreadsheetCell> dependencies;
        private ExpTree expression;
        public SpreadsheetCell(int columnIndex, int rowIndex)
            : base(columnIndex, rowIndex) { }

        //Update inherited value variable
        public void setValue(string text, string expression, Dictionary<string, SpreadsheetCell> dependencies)
        {
            if (String.Equals(expression, "#REF!") || String.Equals(expression, "Error: Circular Dependency"))
            {
                this.value = text;
                this.expression = null;
                this.errors = expression;
            }
            else if (!String.Equals(this.text, this.value))
            {
                this.dependencies = new Dictionary<string, SpreadsheetCell>(dependencies);
                this.value = text.Replace(" ", String.Empty);
                this.errors = String.Empty;
                this.expression = new ExpTree(expression, this.dependencies);
            }
        }

        //Update inherited value variable
        public override double ComputeValue()
        {
            if (this.IsValueSet() && this.expression != null)
            {
                return this.expression.Eval();
            }
            return 0.0;
        }
        public bool HasDependency(string key)
        {
            if (this.dependencies != null)
            {
                return this.dependencies.ContainsKey(key);
            }
            return false;
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
            this.columnCount = columns;
            this.rowCount = rows;
            this.CellArray = new SpreadsheetCell[columns, rows];
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    this.CellArray[i, j] = new SpreadsheetCell(i, j);
                    this.CellArray[i, j].PropertyChanged += this.Spreadsheet_PropertyChanged;
                }
            }
        }

        //Retrieves a cell at (column,row) index
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


        private void SetCellValue(SpreadsheetCell cell)
        {
            //i.e. A1=B1+2 or B3+6+C1
            string text = cell.Text;
            //i.e. B1+2 or B3+6+C1
            text = text.Replace(" ", String.Empty);
            if (text.Contains("=")) {
                text = text.Split('=')[1];
            }
            //Ensures the text in cell has actually changed
            if (!String.Equals(cell.Value, text))
            {
                //sets text to the actual text that has been changes in the cell since its last value
                string value = text;
                Dictionary<string, SpreadsheetCell> dependencies = this.FindDependencies(cell, RemoveOperators(text), ref text);
                cell.setValue(value, text, dependencies);
            }
        }

        //This locates any dependant cell names in the expression entered into the cells text
        //i.e. "A1,B1,3" => Dependent Cells include A1 & B1
        private Dictionary<string, SpreadsheetCell> FindDependencies(SpreadsheetCell cell, string expression, ref string text)
        {
            Dictionary<string, SpreadsheetCell> dependencies = new Dictionary<string, SpreadsheetCell>();
            //expression = "A1,5,B1" => expression.Split(',') = ["A1", "5", "B1"]
            foreach (string var in expression.Split(','))
            {
                //ensures that var is not an empty string
                if (!String.IsNullOrWhiteSpace(var))
                {
                    double value;
                    //if var is a real number then we ignore it
                    //if it is a cell name, such as "B1", then we execute the code below
                    if (!double.TryParse(var, out value))
                    {
                        try
                        {
                            //Converts a column name such as B to an int, i.e. "B" = 1
                            int column = Convert.ToInt32(var[0]) - 65;
                            //Converts a row number such as "1" to an int, i.e. "1" = 0
                            int row = Int32.Parse(var.Remove(0, 1));
                            //This retrives the cell located at the (column,row) values in the spreadsheet
                            SpreadsheetCell dependantCell = GetCell(column, row - 1) as SpreadsheetCell;
                            //Checks for circular Dependancy
                            if (dependantCell.HasDependency(cell.Position) || dependantCell == cell)
                            {
                                text = "Error: Circular Dependency";
                                return null;
                            }
                            //We then add this cell to the current cells depedency dictionary
                            dependencies.Add(dependantCell.Position, dependantCell);
                        }
                        catch (Exception)
                        {
                            text = "#REF!";
                            return null;
                        }
                    }
                }
            }
            return dependencies;
        }

        //Converts the string into a comma(',') delimited string of variables or values
        //i.e. "A1+3+B1" => "A1,3,B1"
        private string RemoveOperators(string text)
        {
            foreach (char op in new char[] { '+', '-', '*', '/', '(', ')' })
            {
                text = text.Replace(op, ',');
            }
            return text;
        }

        //Invoked when a cell's text changes
        private void Spreadsheet_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SpreadsheetCell cell = sender as SpreadsheetCell; //Sets Cell type to subclass type 
            //Sets cells values if they need changing
            this.SetCellValue(cell);
            if (this.CellPropertyChanged != null)
            {
                this.CellPropertyChanged(sender, e); //Passes reference to cell that changed
            }
        }
        public void Load(Stream fileStream)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            using (XmlReader reader = XmlReader.Create(fileStream, settings))
            {
                string name = String.Empty;
                string attribute;
                SpreadsheetCell cell = null;
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                           name = reader.Name.ToLower();
                           if(String.Equals(name, "cell") && reader.HasAttributes){
                                attribute = reader.GetAttribute("name");
                                //Converts a column name such as B to an int, i.e. "B" = 1
                                int column = Convert.ToInt32(attribute[0]) - 65;
                                //Converts a row number such as "1" to an int, i.e. "1" = 0
                                int row = Int32.Parse(attribute.Remove(0, 1))-1;
                                cell = this.GetCell(column, row) as SpreadsheetCell;
                            }
                            break;
                        case XmlNodeType.Text:
                            if (cell != null)
                            {
                                switch (name)
                                {
                                    case "cell":
                                        break;
                                    case "bgcolor":
                                        cell.BGColor = reader.Value;
                                        break;
                                    case "text":
                                        cell.Text = reader.Value;
                                        break;
                                    default: break;
                                }
                            }
                            break;
                        case XmlNodeType.EndElement:
                            if(String.Equals("cell", reader.Name))
                            {
                                cell = null;
                            }
                            break;
                        default: break;
                    }
                }
                reader.Close();
                reader.Dispose();
            }
        }

        public void Save(Stream fileStream)
        {
            //Contains entire xml document
            XDocument srcTree = new XDocument();
            //Contains a list of xml elements to be later inserted into the srcTree
            List<XElement> cells = new List<XElement>();
            //Runs through the entire spreadsheet
            foreach (SpreadsheetCell speadCell in this.CellArray)
            {
                //If the cell's value has been set
                if (speadCell.IsValueSet())
                {
                    //Text value of the cell
                    XElement cellText = new XElement("text", "=" + speadCell.Value);
                    //Bachground color of the cell
                    XElement cellColor = new XElement("bgcolor", speadCell.BGColor);
                    //Inserts the BG color and text elements as the children of the cell element with a name attribute
                    XElement cell = new XElement("cell", new XElement[] { cellText, cellColor });
                    cell.SetAttributeValue("name", speadCell.Position);
                    //adds new cell to cell element list
                    cells.Add(cell);
                }
            }
            //adds cell/element list to xml document 
            srcTree.Add(new XElement("spreadsheet", cells));
            //writes to user selected file
            srcTree.Save(fileStream);
        }
    }
}