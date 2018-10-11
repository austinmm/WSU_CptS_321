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

        //Value to be set/modified by subclass 
        protected string value;
        public string Value{ get { return this.value; }}

        public Cell(int rowIndex, int columnIndex)
        {
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
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
