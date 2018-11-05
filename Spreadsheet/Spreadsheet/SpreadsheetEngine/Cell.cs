using System;
using System.ComponentModel;

namespace CptS321
{
    //
    public abstract class Cell: INotifyPropertyChanged
    {
        //This Event Must be implamented for INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected readonly int rowIndex;
        public int RowIndex { get { return this.rowIndex; } }

        protected readonly int columnIndex;
        public int ColumnIndex { get { return this.columnIndex; } }

        //Triggered by PropertyChanged event
        protected string text;
        public string Text {
            get { return this.text; }
            set {
                if (!String.Equals(this.text, value)){
                    this.text = value;
                    RaisePropertyChanged("Text");
                }
            }
        }

        //Is set if any Errors Occur
        protected string errors;
        public string Errors { get { return this.errors; } }

        //The Column and Row position name
        protected string position;
        public string Position{ get { return this.position; } }

        //Value to be set/modified by subclass 
        protected string value;
        public string Value { get { return this.value; }}


        public Cell(int columnIndex, int rowIndex)
        {
            this.columnIndex = columnIndex;
            this.rowIndex = rowIndex;
            //i.e. column = 0, row = 0 : A1
            this.position = $"{Convert.ToChar(this.columnIndex + 65)}{this.rowIndex + 1}";
        }

        //Computes the value of the text within the cell
        public abstract double ComputeValue();
        //Checks if the cells value has been set
        public bool IsValueSet()
        {
            return this.value != null;
        }


        //https://github.com/jbe2277/waf/wiki/Implementing-and-usage-of-INotifyPropertyChanged
        protected void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        //Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(PropertyChangedEventArgs property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, property);
            }
        }
    }
}
