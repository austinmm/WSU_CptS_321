using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpTreeTest
{
    class Program
    {


        static void Main(string[] args)
        {
            //Assume expressions will NOT have any parentheses
            //Assume expressions will only have a single type of operator, but can have any number
            //... of instances of that operator in the expression            //Support operators +,-,*, and /            //Parse the expression that the user enters and build the appropriate tree in memory
            CptS321.ExpTree expTree = null;
            Menu(expTree);

        }

        static void Menu(CptS321.ExpTree expTree)
        {
            bool canExit = false;
            do
            {
                string currExprssion = expTree == null ? "" : expTree.ToString();
                Console.WriteLine($"Menu (Current Expresssion: {currExprssion})");
                int option = GetMenuChoice();
                switch (option)
                {
                    //Enter a New Expression
                    case 1:
                        expTree = EnterNewExpression();
                        break;
                    //Set a Variable Value
                    case 2:
                        SetVariableValue(expTree);
                        break;
                    //Evaluate Tree
                    case 3:
                        Console.WriteLine(expTree.Eval());
                        break;
                    //Quit
                    case 4:
                        canExit = true;
                        break;
                    //Other
                    default: break;
                }
            } while (!canExit);
        }

        public static CptS321.ExpTree EnterNewExpression()
        {
            Console.Write("Enter New Expression: ");
            string expression = Console.ReadLine();
            return new CptS321.ExpTree(expression);
        }

        public static void SetVariableValue(CptS321.ExpTree expTree)
        {
            if(expTree != null && !expTree.IsEmpty())
            {
                Console.Write("Enter Variable Name:");
                string name = Console.ReadLine();
                Console.Write("Enter Variable Value:");
                int value = Int32.Parse(Console.ReadLine());
                expTree.SetVar(name, value);
            }
            else
            {
                Console.WriteLine("Expression Tree cannot be modified because it has not been assigned an expression yet...");
            }
        }

        static int GetMenuChoice()
        {
            Console.WriteLine("1.) Enter a New Expression");
            Console.WriteLine("2.) Set a Variable Value");
            Console.WriteLine("3.) Evaluate Tree");
            Console.WriteLine("4.) Quit");
            Console.Write("Choice: ");
            return Int32.Parse(Console.ReadLine());
        }
    }
}
