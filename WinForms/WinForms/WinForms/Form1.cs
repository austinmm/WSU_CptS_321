using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace WinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Random rand = new Random();
            List<int> numbers = new List<int>();
            for (int i = 0; i < 10000; i++)
            {
                numbers.Add(rand.Next(0, 20001));
            }
            //All times are in Milliseconds
            int mapCount = this.UnorderedMap(numbers, out double mapTime);
            int constMemCount = this.ConstantMemoryComplexity(numbers, out double constMemTime);
            int listCount = this.ListMethods(numbers, out double listTime);
            StringBuilder results = new StringBuilder();
            results.Append("Hash-Set Method:\r\n"
                + $"  ~ Unique Numbers: {mapCount}\r\n"
                + $"  ~ Time (Milliseconds): {mapTime}\r\n"
                + "  ~ Time Complexity: O(n)\r\n"
                + "  ~ Memory Complexity: O(n)\r\n\r\n");
            results.Append("Constant Storage Method:\r\n"
                + $"  ~ Unique Numbers: {constMemCount}\r\n"
                + $"  ~ Time (Milliseconds): {constMemTime}\r\n"
                + "  ~ Time Complexity: O(n^2)\r\n"
                + "  ~ Memory Complexity: O(1)\r\n\r\n");
            results.Append("Sorted Method:\r\n"
                + $"  ~ Unique Numbers: {listCount}\r\n"
                + $"  ~ Time (Milliseconds): {listTime}\r\n"
                + "  ~ Time Complexity: O(n)\r\n"
                + "  ~ Memory Complexity: O(1)");
            textBox1.Text = results.ToString();
        }

        //Time Complexity: O(n)
        //Memory Complexity: O(n)
        private int UnorderedMap(List<int> _numbers, out double _time)
        {
            //Object that will contain unique numbers in original list
            HashSet<int> map = new HashSet<int>();
            //Used to determine the time the algorithm took to process list in milliseconds
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (int num in _numbers)
            {
                if (!map.Contains(num))
                {
                    map.Add(num);
                }
            }
            stopwatch.Stop();
            _time = stopwatch.Elapsed.TotalMilliseconds;
            return map.Count;
        }

        //Time Complexity: O(n^2)
        //Memory Complexity: O(1)
        //Storage Complexity: O(1)
        private int ConstantMemoryComplexity(List<int> _numbers, out double _time)
        {
            //Used to count how many values in list are not unique
            int notUnique = 0;
            //Used to determine the time the algorithm took to process list in milliseconds
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //Retrieves the amount of numbers stored in list
            int size = _numbers.Count;
            for (int i = 0; i < size; i++)
            {
                //sets num to the ith index of the list
                int num = _numbers[i];
                //j begins 1 index greater then i
                for (int j = i + 1; j < size; j++)
                {
                    if (num == _numbers[j])
                    {
                        notUnique++;
                        break;
                    }
                }
            }
            stopwatch.Stop();
            _time = stopwatch.Elapsed.TotalMilliseconds;
            return size - notUnique;
        }

        //Time Complexity: O(n)
        //Memory Complexity: O(1)
        private int ListMethods(List<int> _numbers, out double _time)
        {
            //Used to determine the time the algorithm took to process list in milliseconds
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //Gets distinct elements and convert them into a list again
            _numbers = _numbers.Distinct().ToList();
            stopwatch.Stop();
            _time = stopwatch.Elapsed.TotalMilliseconds;
            return _numbers.Count;
        }
    }
}
