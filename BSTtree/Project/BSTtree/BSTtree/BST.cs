using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSTtree
{
    class BST
    {
        //Root of BST tree
        private Node root;
        public Node Root { get { return this.root; } }
        //Amount of nodes in BSt
        private int count;
        public int Count { get { return this.count; } }

        public BST()
        {
            this.root = null;
        }

        public BST(Node _root = null, int _count = 0)
        {
            this.root = _root;
            this.count = _count;
        }

        //Checks if the BST is empty
        public bool IsEmpty() {
            return this.root == null ? true : false;
        }

        //Prints out all the nodes in the BST in order
        static public void InOrderTraversal(Node _root)
        {
            if (_root != null)
            {
                BST.InOrderTraversal(_root.Left);
                Console.Write($"{_root.Value} ");
                BST.InOrderTraversal(_root.Right);
            }
        }

        //Inserts a new node into the tree in the correct spot
        public Node Insert(int _value)
        {
            //This condition checks if the BST root node is null indicating an empty tree
            if (this.IsEmpty())
            {
                //Creates the new node and is assigned to the root of the tree
                this.root = this.CreateNode(_value);
                return this.root;
            }
            return this.Insert(this.root, _value);
        }

        private Node Insert(Node _root, int _value)
        {          
            //If the value passed in is less than the current node in the BST
            if (_root.Value > _value)
            {
                //if the next Left node (root.Left) is null then I set root.Left to a newNode that is created
                if (_root.Left == null)
                {
                    _root.Left = this.CreateNode(_value);
                    return _root.Left;
                }
                else
                {
                    return this.Insert(_root.Left, _value);
                }
            }
            //If the value passed in is greater than the current node in the BST
            else if (_root.Value < _value)
            {
                //if the next Right node (root.Right) is null then I set root.Right to a newNode that is created
                if (_root.Right == null)
                {
                    _root.Right = this.CreateNode(_value);
                    return _root.Right;
                }
                else
                {
                    return this.Insert(_root.Right, _value);
                }
            }
            //If the value is the same as an already existent node in the BST...
            return null;
        }

        private Node CreateNode(int _value)
        {
            Node newNode = new Node();
            if (newNode != null)
            {
                newNode.Value = _value;
                this.count++;
            }
            return newNode;
        }

        //Returns the amount of nodes in the tree
        static public void NodeCount(Node _root, ref int _count)
        {
            //checks if node is empty or not
            if (_root != null)
            {
                BST.NodeCount(_root.Left, ref _count);
                _count++;
                BST.NodeCount(_root.Right, ref _count);
            }
        }

        //Returns the depth of the tree
        static public int Depth(Node _root)
        {
            if (_root == null) { return -1; }
            int Left = BST.Depth(_root.Left);
            int Right = BST.Depth(_root.Right);
            return Left > Right ? Left + 1: Right + 1;
        }

        static public int MinimumLevels(Node root, Nullable<int> count = null)
        {
            int value = count.HasValue ? count.Value : 0;
            if (!count.HasValue) { BST.NodeCount(root, ref value); }
            //Minimum = Floor(ln(2 * Total-Nodes))
            //https://www.geeksforgeeks.org/relationship-number-nodes-height-binary-tree/
            return Convert.ToInt32(Math.Floor(Math.Log(2 * value)));
        }
    }
}
