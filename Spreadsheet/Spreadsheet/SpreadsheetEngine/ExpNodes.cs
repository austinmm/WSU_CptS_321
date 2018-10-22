using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    public abstract class Node
    {
        public abstract double eval();
    }

    public class OpNode : Node, ICloneable
    {
        private Node left;
        private Node right;
        private char operation;
        public Node Left { get { return this.left; } set { this.left = value; } }
        public Node Right { get { return this.right; } set { this.right = value; } }
        public char Operation { get { return this.operation; } set { this.operation = value; } }

        public OpNode(char operation)
        {
            this.operation = operation;
        }

        public OpNode(char operation, Node left, Node right)
        {
            this.operation = operation;
            this.left = left;
            this.right = right;
        }

        public override string ToString()
        {
            return $"{this.left.ToString()}{this.operation}{this.right.ToString()}";
        }

        public override double eval()
        {
            switch (this.operation)
            {
                case '+':
                    return this.left.eval() + this.right.eval();
                case '-':
                    return this.left.eval() - this.right.eval();
                case '*':
                    return this.left.eval() * this.right.eval();
                case '/':
                    return this.left.eval() / this.right.eval();
                default:
                    throw new Exception("Invalid Operation Provided");
            }
        }

        public object Clone()
        {
            return new OpNode(this.operation, this.left, this.right);
        }
    }

    public class VarNode : Node, ICloneable
    {
        private ValNode value;
        private string variable;
        public ValNode Value { get { return this.value; } set { this.value = value; } }
        public string Variable { get { return this.variable; } set { this.variable = value; } }

        public VarNode(string variable, double value = 0.0)
        {
            this.variable = variable;
            this.value = new ValNode(value);
        }

        public VarNode(string variable, ValNode value)
        {
            this.variable = variable;
            this.value = value;
        }

        public override string ToString()
        {
            return $"{this.variable}";
        }

        public override double eval()
        {
            return this.value.eval();
        }

        public object Clone()
        {
            return new VarNode(this.variable, this.value);
        }
    }

    public class ValNode : Node, ICloneable
    {
        private double value;
        public double Value { get { return this.value; } set { this.value = value; } }

        public ValNode(double value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return $"{this.value}";
        }

        public override double eval()
        {
            return this.value;
        }

        public object Clone()
        {
            return new ValNode(this.value);
        }
    }
}
