using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSTtree
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test Input: 62 11 23 43 56 78 98 34 54 21 99 88 77 22 33 44 55 21 11
            List<string> errors = new List<string>();
            Console.Write("Please Enter a list of integers ranging from 0-100 (i.e. \"1 3 5 87 9\"): ");
            string str = Console.ReadLine();
            BST tree = new BST();
            SplitandInsert(str, tree, ref errors);
            Console.WriteLine($"BST Count: {tree.Count}");         
            Console.WriteLine($"BST Depth: {BST.Depth(tree.Root)}");
            Console.WriteLine($"BST Minimum level: {BST.MinimumLevels(tree.Root, tree.Count)}");
        }

        static void SplitandInsert(string str, BST tree, ref List<string> errors)
        {
            List<string> strList =  str.Split(' ').ToList<string>();
            foreach (var element in strList)
            {
                try
                {
                    tree.Insert(Int32.Parse(element));
                }
                catch (Exception ex)
                {
                    errors.Add(ex.Message);
                    if (ex.InnerException != null)
                    {
                        errors.Add(ex.InnerException.Message);
                    }
                }
            }
        }
    }
}
