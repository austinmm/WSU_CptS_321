using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSTtree
{
    class BST<T>: BinTree<T>
    {
        //Root of BST tree
        private Node<T> root;
        public Node<T> Root { get { return this.root; } }
        //Amount of nodes in BST
        private int count;
        public int Count { get { return this.count; } }

        public BST()
        {
            this.root = null;
        }

        public BST(Node<T> _root = null, int _count = 0)
        {
            this.root = _root;
            this.count = _count;
        }

        //Checks if the BST is empty
        public bool IsEmpty()
        {
            return this.root == null ? true : false;
        }

        public static bool IsEmpty(Node<T> _root)
        {
            return _root == null ? true : false;
        }

        public override void Insert(T val)
        {
            if (this.IsEmpty())
            {
                this.root = this.CreateNode(val);
                return;
            }
            Node<T> curr = this.root;
            while (!IsEmpty(curr))
            {
                //If the value passed in is less than the current node in the BST
                if (curr > val)
                {
                    //if the next Left node (root.Left) is null then I set root.Left to a newNode that is created
                    if (curr.Left == null)
                    {
                        curr.Left = this.CreateNode(val);
                        break;
                    }
                    else
                    {
                        curr = curr.Left;
                        continue;
                    }
                }
                //If the value passed in is greater than the current node in the BST
                else if (curr < val)
                {
                    //if the next Right node (root.Right) is null then I set root.Right to a newNode that is created
                    if (curr.Right == null)
                    {
                        curr.Right = this.CreateNode(val);
                        break;
                    }
                    else
                    {
                        curr = curr.Right;
                        continue;
                    }
                }
                //If the value is the same as an already existent node in the BST...
                else { return; }
            }
        }

        private Node<T> CreateNode(T _value)
        {
            Node<T> newNode = new Node<T>();
            if (newNode != null)
            {
                newNode.Value = _value;
                this.count++;
            }
            return newNode;
        }

        //Returns the amount of nodes in the tree
        static public void NodeCount(Node<T> _root, ref int _count)
        {
            //checks if node is empty or not
            if (_root != null)
            {
                BST<T>.NodeCount(_root.Left, ref _count);
                _count++;
                BST<T>.NodeCount(_root.Right, ref _count);
            }
        }

        //Returns the depth of the tree
        static public int Depth(Node<T> _root)
        {
            if (_root == null) { return 0; }
            int Left = BST<T>.Depth(_root.Left);
            int Right = BST<T>.Depth(_root.Right);
            return Left > Right ? Left + 1 : Right + 1;
        }

        static public int MinimumLevels(Node<T> root, Nullable<int> count = null)
        {
            int value = count.HasValue ? count.Value : 0;
            if (!count.HasValue) { BST<T>.NodeCount(root, ref value); }
            //Minimum = Floor(ln(2 * Total-Nodes))
            //https://www.geeksforgeeks.org/relationship-number-nodes-height-binary-tree/
            return Convert.ToInt32(Math.Floor(Math.Log(2 * value)) + 1);
        }

        public override bool Contains(T val)
        {
            Node<T> curr = this.root;
            while (!IsEmpty(curr))
            {
                //If the value passed in is less than the current node in the BST
                if (curr > val)
                {
                    if (curr.Left == null){
                        return false;
                    }else{
                        curr = curr.Left;
                    }
                }
                //If the value passed in is greater than the current node in the BST
                else if (curr < val)
                {
                    if (curr.Right == null){
                        return false;
                    } else{
                        curr = curr.Right;
                    }
                }
                //If the value equals another node's value in the BST...
                else { return true; }
            }
            return false;
        }

        public override void InOrder()
        {
            InOrder(this.root);
        }
        
        //Prints out all the nodes in the BST in order
        private static void InOrder(Node<T> _root)
        {
            if (_root != null)
            {
                BST<T>.InOrder(_root.Left);
                Console.Write($"{_root.Value} ");
                BST<T>.InOrder(_root.Right);
            }
        }

        public override void PreOrder()
        {
            PreOrder(this.root);
        }

        //Prints out all the nodes in the BST in order
        private static void PreOrder(Node<T> _root)
        {
            if (_root != null)
            {
                Console.Write($"{_root.Value} ");
                BST<T>.PreOrder(_root.Left);
                BST<T>.PreOrder(_root.Right);
            }
        }

        public override void PostOrder()
        {
            PostOrder(this.root);
        }
        private static void PostOrder(Node<T> _root)
        {
            if (_root != null)
            {
                BST<T>.PostOrder(_root.Left);
                BST<T>.PostOrder(_root.Right);
                Console.Write($"{_root.Value} ");
            }
        }
    }
}