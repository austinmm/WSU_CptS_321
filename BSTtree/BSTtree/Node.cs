using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSTtree
{
    class Node<T>: ICloneable, IComparable<T>
    {
        //Fields
        private T value;
        private Node<T> left;
        private Node<T> right;
        //Properties
        public T Value { get { return this.value; } set { this.value = value; } }
        public Node<T> Left { get { return this.left; } set { this.left = value; } }
        public Node<T> Right { get { return this.right; } set { this.right = value; } }

        public Node()
        {
            this.left = null;
            this.right = null;
            this.value = default(T);
        }

        public Node(Node<T> _left = null, Node<T> _right = null, T _value = default(T))
        {
            this.left = _left;
            this.right = _right;
            this.value = _value;
        }

        //Clones the current Node instance and returns a new deep copy of it
        public object Clone()
        {
            Node<T> newNode = new Node<T>(_left: this.left, _right: this.right, _value: this.value);
            return newNode;
        }

        //Checks if the current instance and the one passed in are equal
        public int CompareTo(object obj)
        {
            return this == ((Node<T>)obj) ? 1 : 0;
        }

        public static bool operator <(Node<T> _lhs, T _rhs)
        {
            return Comparison(_lhs.Value, _rhs) == -1;
        }

        public static bool operator >(Node<T> _lhs, T _rhs)
        {
            return Comparison(_lhs.Value, _rhs) == 1;
        }

        public static int Comparison(T _lhs, T _rhs)
        {
            dynamic lhs = _lhs, rhs = _rhs;
            if (lhs < rhs)
            {
                return -1;
            }

            else if (lhs == rhs){
                return 0;
            }

            else if (lhs > rhs){
                return 1;
            }
            return 0;
        }

        public int CompareTo(T other)
        {
            return ((IComparable<T>)left.value).CompareTo(other);
        }
    }
}