using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class BinaryTree<T>
    {
        private BinaryTreeNode<T> root;
        public BinaryTreeNode<T> Root()
        {
            return root;
        }

        //public BinaryTree(BinaryTreeNode<T> rootNode)
        //{
        //    rootNode.parent = null;
        //    this.root = rootNode;
        //}

        public BinaryTree(T rootValue)
        {
            this.root = new BinaryTreeNode<T>(rootValue);
        }
    }//end class tree

    public class BinaryTreeNode<T>
    {
        public BinaryTreeNode<T> parent;
        public BinaryTreeNode<T> leftChild;
        public BinaryTreeNode<T> rightChild;
        public BinaryTreeNode<T> coorChild;
        
        /*
        //public bool IsLeaf() version 1
        //{
        //    if (this.leftChild == null && this.rightChild == null)
        //    {
        //        //is this a leaf or not a leaf? -> is a leaf
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //version 2 ???
        //if (this.leftChild == null && this.rightChild == null)
        //{
        //    return true;
        //}
        //version ???
        //public bool IsLeaf()
        //{
        //    if (this.leftChild == null && this.rightChild == null) return true;
        //    return false;
        //}

        //verison 5
        //public bool IsLeaf()
        //{
        //    return (this.leftChild == null && this.rightChild == null);
        //}
        //veriosn 6??*/
        public bool IsLeaf()
        {
            return leftChild == null && rightChild == null;
        }

        public bool isCoorLeaf()
        {
            return coorChild != null;
        }

        private T innerValue;
        public T Value()
        {
            return innerValue;
        }

        public BinaryTreeNode(T nodeValue)
        {
            this.innerValue = nodeValue;
        }

        public BinaryTreeNode<T> AddChild(T childValue)
        {
            if (leftChild == null)
            {
                leftChild = new BinaryTreeNode<T>(childValue);
                leftChild.parent = this;
                return leftChild;
            }

            if (rightChild == null)
            {
                rightChild = new BinaryTreeNode<T>(childValue);
                rightChild.parent = this;
                return rightChild;
            }
            
            throw new InvalidOperationException("Cannot add more than two children to a binary node.");
        }

        public BinaryTreeNode<T> AddCoor(T childValue)
        {
            if (coorChild == null)
            {
                coorChild = new BinaryTreeNode<T>(childValue);
                coorChild.parent = this;
                return coorChild;
            }
            throw new InvalidOperationException("Cant add more cooridors to the tree");
        }
    }//end class treenode
}//end namespace
