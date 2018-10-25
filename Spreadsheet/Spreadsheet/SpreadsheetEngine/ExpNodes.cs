using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    public static class OpType {
        public const string Plus = "+";
        public const string Sub = "-";
        public const string Mul = "*";
        public const string Div = "/";
    };

    public abstract class Node
    {
        public abstract double eval();
        public abstract List<string> GetToken();
    }

    public class OpNode : Node, ICloneable
    {
        private Node left;
        public Node Left { get { return this.left; } set { this.left = value; } }
        private Node right;
        public Node Right { get { return this.right; } set { this.right = value; } }
        private string operation;
        public string Operation { get { return this.operation; } set { this.operation = value; } }

        public OpNode(string operation)
        {
            this.operation = operation;
        }

        public OpNode(string operation, Node left, Node right)
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
                case OpType.Plus:
                    return this.left.eval() + this.right.eval();
                case OpType.Sub:
                    return this.left.eval() - this.right.eval();
                case OpType.Mul:
                    return this.left.eval() * this.right.eval();
                case OpType.Div:
                    return this.left.eval() / this.right.eval();
                default:
                    throw new Exception("Invalid Operation Provided");
            }
        }

        public object Clone()
        {
            return new OpNode(this.operation, this.left, this.right);
        }

        public override List<string> GetToken()
        {
            List<string> value = new List<string>();
            value.AddRange(this.left.GetToken());
            value.Add($"{this.operation}");
            value.AddRange(this.right.GetToken());
            return value;
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
            if (this.value != null)
            {
                return this.value.eval();
            }
            return 0.0;
        }

        public object Clone()
        {
            return new VarNode(this.variable, this.value);
        }

        public override List<string> GetToken()
        {
            return this.value.GetToken();
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

        public override List<string> GetToken()
        {
            return new List<string>() { $"{this.value}" };
        }
    }

    //Helper Enum for ParNode
    public enum ParType { leftPar, rightPar };
    //For Parentheses strings
    public class ParNode : Node
    {
        //contents is either a valNode or VarNode Depending on what is the suffix or prefix of the Parentheses
        private Node contents;
        public Node Contents { get { return this.contents; } set { this.contents = value; } }
        private ParType parentheses;
        public ParType Parentheses { get { return this.parentheses; } set { this.parentheses = value; } }

        public ParNode(string parentheses, Node contents)
        {
            this.parentheses = String.Equals(parentheses, "(")? ParType.leftPar: ParType.rightPar;
            this.contents = contents;
        }

        public override string ToString()
        {
            switch(this.parentheses)
            {
                case ParType.leftPar:
                    return $"({this.contents.ToString()}";
                default:
                    return $"{this.contents.ToString()})";
            }
        }

        public override double eval()
        {
            if (this.contents != null)
            {
                return this.contents.eval();
            }
            return 0.0;
        }

        public override List<string> GetToken()
        {
            List<string> result = new List<string>();
            switch (this.parentheses)
            {
                case ParType.leftPar:
                    result.Add("(");
                    result.AddRange(this.contents.GetToken());
                    break;
                default:
                    result.AddRange(this.contents.GetToken());
                    result.Add(")");
                    break;
            }
            return result;
        }
    }
}
