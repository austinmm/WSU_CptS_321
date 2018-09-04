using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSTtree
{
    class Node : IComparable, ICloneable
    {
        //Fields
        private int value;
        private Node left;
        private Node right;
        //Properties
        public int Value { get { return this.value; } set { this.value = value; } }
        public Node Left { get { return this.left; } set { this.left = value; } }
        public Node Right { get { return this.right; } set { this.right = value; } }

        public Node()
        {
            this.left = null;
            this.right = null;
            this.value = 0;
        }

        public Node(Node _left = null, Node _right = null, int _value = 0)
        {
            this.left = _left;
            this.right = _right;
            this.value = _value;
        }

        //Clones the current Node instance and returns a new deep copy of it
        public object Clone()
        {
            Node newNode = new Node(_left: this.left, _right: this.right, _value: this.value);
            return newNode;
        }

        //Checks if the current instance and the one passed in are equal
        public int CompareTo(object obj)
        {
            return this == ((Node)obj) ? 1 : 0;
        }

        public static bool operator< (Node _lhs, Node _rhs)
        {
            return _lhs.value < _rhs.value;
        }
        public static bool operator> (Node _lhs, Node _rhs)
        {
            return _lhs.value > _rhs.value;
        }
    }
}
