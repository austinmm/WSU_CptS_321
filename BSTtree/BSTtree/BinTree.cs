using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSTtree
{
    public abstract class BinTree<T>
    {
        public abstract void Insert(T val); // Insert new item of type T
        public abstract bool Contains(T val); // Returns true if item is in tree
        public abstract void InOrder(); // Print elements in tree inorder traversal
        public abstract void PreOrder();
        public abstract void PostOrder();
    }
}
