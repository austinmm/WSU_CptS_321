using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /*
     * 1.)Each node in the tree will be in one of three categories:
        ● Node representing a constant numerical value
        ● Node representing a variable
        ● Node representing a binary operator        ● Note: No node should store any more data than it needs to. As just one example of
            what this means, the constant value and variable nodes never have children so
            their class declarations shouldn’t have references to children (nor should they be
            inheriting such declarations from a parent class)     * 2.) Support for Varables:        ● Support correct functionality of variables including multi-character values (like “A2”).
        ● Variables will start with an alphabet character, upper or lower-case, and be followed by
            any number of alphabet characters and numerical digits (0-9).
        ● A set of variables is stored per-expression, so creating a new expression will clear out
            the old set of variables.
        ● Have a default expression, something like “A1+B1+C1” would be fine, so that if setting
            variables is the first action that the user chooses then you have an expression object to
            work with.    */
    public class ExpTree
    {
        private Node root;
        private Node Root { get { return this.root; } set { this.root = value; } }
        private Dictionary<string, VarNode> varDict;

        public ExpTree(string expression)
        {
            this.varDict = new Dictionary<string, VarNode>();
            this.root = this.ConstructTree(expression);
        }
        private Node ConstructTree(string expression) { 
            for (int i = expression.Length - 1; i >= 0; i--)
            {
                char currChar = expression[i];
                switch (currChar)
                {
                    case '+':
                        OpNode opNode1 = new OpNode(currChar);
                        opNode1.Left = this.ConstructTree(expression.Substring(0, i));
                        opNode1.Right = this.ConstructTree(expression.Substring(i + 1));
                        return opNode1;
                    case '-':
                        OpNode opNode2 = new OpNode(currChar);
                        opNode2.Left = this.ConstructTree(expression.Substring(0, i));
                        opNode2.Right = this.ConstructTree(expression.Substring(i + 1));
                        return opNode2;
                    case '*':
                        OpNode opNode3 = new OpNode(currChar);
                        opNode3.Left = this.ConstructTree(expression.Substring(0, i));
                        opNode3.Right = this.ConstructTree(expression.Substring(i + 1));
                        return opNode3;
                    case '/':
                        OpNode opNode = new OpNode(currChar);
                        opNode.Left = this.ConstructTree(expression.Substring(0, i));
                        opNode.Right = this.ConstructTree(expression.Substring(i + 1));
                        return opNode;
                    default:
                        break;
                }
            }
            Node newNode; 
            try
            {
                double value = double.Parse(expression);
                newNode = new ValNode(value);
            }
            catch (Exception)
            {
                newNode = new VarNode(expression);
                this.varDict.Add(expression, (VarNode)newNode);
            }
            return newNode;
        }

        public bool IsEmpty()
        {
            return this.root == null;
        }

        public void SetVar(string varName, double varValue)
        {
            if (this.varDict.ContainsKey(varName))
            {
                this.varDict[varName].Value = new ValNode(varValue);
            }
        }

        public override string ToString()
        {
            return this.root.ToString();
        }

        public double Eval()
        {
            // Evaluates the expression to a double value
            return this.root.eval();
        }

    }
}
