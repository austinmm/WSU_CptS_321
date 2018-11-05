using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    public class ExpTree
    {
        //Root of Expression Tree
        private Node root; 
        private Node Root { get { return this.root; } set { this.root = value; } }
        //Contains ref to all VarNodes in Expression Tree
       // private Dictionary<string, VarNode> varDict;

        //public ExpTree(string expression)
        //{
        //    //removes whitespace from expression
        //    expression = expression.Replace(" ", String.Empty);
        //    this.varDict = new Dictionary<string, VarNode>();
        //    this.root = this.ConstructTree(expression);
        //}

        public ExpTree(string expression, Dictionary<string, SpreadsheetCell> dependencies)
        {
            //removes whitespace from expression
            expression = expression.Replace(" ", String.Empty);
           // this.varDict = new Dictionary<string, VarNode>();
            this.root = this.ConstructTree(expression, dependencies);
        }

        //Creates the Expression tree structure
        private Node ConstructTree(string expression, Dictionary<string, SpreadsheetCell> dependencies)
        {
            Node newNode;
            //For opNodes
            for (int i = expression.Length - 1; i >= 0; i--)
            {
                char currChar = expression[i];
                if(currChar == '+'|| currChar == '-'
                    || currChar == '*' || currChar == '/')
                {
                    newNode = new OpNode($"{currChar}");
                    ((OpNode)newNode).Left = this.ConstructTree(expression.Substring(0, i), dependencies);
                    ((OpNode)newNode).Right = this.ConstructTree(expression.Substring(i + 1), dependencies);
                    return newNode;
                }
            }
            //For ValNodes or VarNodes
            try
            {
                double value = double.Parse(expression);
                newNode = new ValNode(value);
            }
            catch (Exception)
            {
                if (expression.Contains("("))
                {
                    expression = expression.Remove(0, 1);
                    Node insideNode = this.ConstructTree(expression, dependencies);
                    newNode = new ParNode("(", insideNode);
                }
                else if (expression.Contains(")"))
                {
                    expression = expression.Remove(expression.Length - 1, 1);
                    Node insideNode = this.ConstructTree(expression, dependencies);
                    newNode = new ParNode(")", insideNode);
                }
                else
                {
                    if (dependencies.TryGetValue(expression, out SpreadsheetCell cell))
                    {
                        newNode = new VarNode(cell);
                        //this.varDict.Add(expression, (VarNode)newNode);
                    }
                    else { newNode = new ValNode(0.0); }
                }
            }
            return newNode;
        }

        //Checks if root is empty
        public bool IsEmpty()
        {
            return this.root == null || String.IsNullOrWhiteSpace(this.ToString());
        }


        ////Sets the value(ValNode) of a VarNode in the VarDict
        //public void SetVar(string varName, double varValue)
        //{
        //    if (this.varDict.ContainsKey(varName))
        //    {
        //        this.varDict[varName].Value = new ValNode(varValue);
        //    }
        //}

        //Recursively calls the Expression Tree's ToString() overrided methods to print out tree contents
        public override string ToString()
        {
            return this.root.ToString();
        }

        //Evaluates the value of the expression
        public double Eval()
        {
            if (this.IsEmpty()) { return 0.0; }
            List<string>  expression = this.root.GetToken();
            Queue<string> outQueue = new Queue<string>();
            Stack<KeyValuePair<string, int>> opStack = new Stack<KeyValuePair<string, int>>();
            this.ShuntingYardAlg(expression, outQueue, opStack);
            return this.ReversePolish(outQueue);
        }

        //Converts Queue in Reverse Polish Notation into an actual double value
        private double ReversePolish(Queue<string> outQueue)
        {
            Stack<double> result = new Stack<double>();
            double n1 = 0.0, n2 = 0.0;
            //runs through every value in the Queue
            foreach(string val in outQueue)
            {
                //checks if val is an operator, and what type, or not
                switch (val)
                {
                    case OpType.Plus:
                        n2 = result.Pop();
                        n1 = result.Pop();
                        result.Push(n1 + n2);
                        break;
                    case OpType.Sub:
                        n2 = result.Pop();
                        n1 = result.Pop();
                        result.Push(n1 - n2); break;
                    case OpType.Mul:
                        n2 = result.Pop();
                        n1 = result.Pop();
                        result.Push(n1 * n2); break;
                    case OpType.Div:
                        n2 = result.Pop();
                        n1 = result.Pop();
                        result.Push(n1 / n2); break;
                    default:
                        result.Push(double.Parse(val));
                        break;
                }
            }
            //returns the final double value
            return result.Pop();
        }

        private void ShuntingYardAlg(List<string> expression, Queue<string> outQueue, Stack<KeyValuePair<string, int>> opStack)
        {
            //While there are tokens to be read, Read a token
            foreach (string token in expression)
            {
                try //If it's a number add it to queue
                {                  
                    int val = Int32.Parse(token);
                    outQueue.Enqueue(token);
                }
                catch (Exception) // If it's an operator or bracket
                {
                    //If it's a left bracket push it onto the stack
                    if (token == "(")
                    {
                        opStack.Push(new KeyValuePair<string, int>(token, 0));
                    }
                    //If it's a right bracket 
                    else if (token == ")")
                    {
                        //While there's not a left bracket at the top of the stack
                        while (opStack.Peek().Key != "(")
                        {
                            //Pop operators from the stack onto the output queue
                            outQueue.Enqueue(opStack.Pop().Key);
                        }
                        //Pop the left bracket from the stack and discard it
                        opStack.Pop(); //Left Bracket Discarded
                    }
                    else
                    {
                        int precedence = 0;
                        if (token == OpType.Plus || token == OpType.Sub) { precedence = 1; }
                        else { precedence = 2; }
                        KeyValuePair<string, int> newOp = new KeyValuePair<string, int>(token, precedence);
                        //While there's an operator on the top of the stack with greater precedence:
                        while (opStack.Count > 0 && newOp.Value < opStack.Peek().Value)
                        {
                            outQueue.Enqueue(opStack.Pop().Key);
                        }
                        opStack.Push(newOp);
                    }
                }
            }
            //While there are operators on the stack, pop them to the queue
            while (opStack.Count != 0)
            {
                outQueue.Enqueue(opStack.Pop().Key);
            }
        }
    }
}