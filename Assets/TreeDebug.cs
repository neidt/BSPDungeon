using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class TreeDebug : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //todo:
        //use BinaryTree class to demonsrate its correctness
        BinaryTree<int> sampleTree = new BinaryTree<int>(42);

        BinaryTreeNode<int> left = sampleTree.Root().AddChild(5);
        BinaryTreeNode<int> right = sampleTree.Root().AddChild(17);

        left.AddChild(-6);
        left.AddChild(12);

        right.AddChild(128);
        right.AddChild(1024);

        //now:
        BinaryTreeNode<int> treeRoot = sampleTree.Root();
        List<BinaryTreeNode<int>> leaves = new List<BinaryTreeNode<int>>();

        CollectLeaves(treeRoot, leaves);
        //at this piont <leaves> nshould conatain all chldless nodes in the tree

        foreach ( BinaryTreeNode<int> leaf in leaves)
        {
            print("Leaf found with value: " + leaf.Value() + " and parent value: " + leaf.parent.Value());
            int currentLeafSum = CountFromNodeToRoot(leaf);
            print("sum to root is: " + currentLeafSum);
        }
        
        /*
        //task: find the sum from all the leaf nodes (dumb way)
        //BinaryTreeNode<int> current;

        //int leftofLeftSum = 0;
        //current = leftofLeft;
        //while(current != null)
        //{
        //    leftofLeftSum += current.Value();
        //    current = current.parent;
        //}
        ////left of left sum shpould be 41
        //print("L.O.L SUM = " + leftofLeftSum);

        //int rightofleftSum = 0;
        //current = rightofLeft;
        //while (current != null)
        //{
        //    rightofleftSum += current.Value();
        //    current = current.parent;
        //}
        ////left of left sum shpould be 59
        //print("R.O.L SUM = " + rightofleftSum);
        
        ////another way
        //int leftOfLeftSum = CountFromNodeToRoot(leftofLeft);
        //int rightOfLeftSum = CountFromNodeToRoot(rightofLeft);
        //int leftOfRightSum = CountFromNodeToRoot(leftofRight);
        //int rightOfRightSum = CountFromNodeToRoot(rightofRight);

        ////print all the sums
        //print("leftofleftsum= " + leftOfLeftSum);
        */

    }//end start

    private void CollectLeaves(BinaryTreeNode<int> currentNode, List<BinaryTreeNode<int>> leaves)
    {
        if (currentNode == null) return;

        //practical exit case: currentNode is a leaf node
        if (currentNode.IsLeaf())
        {
            //is this a leaf or not a leaf? -> is a leaf
            leaves.Add(currentNode);
        }
        else
        {
            CollectLeaves(currentNode.leftChild, leaves);
            CollectLeaves(currentNode.rightChild, leaves);
        }
    }

    private int CountFromNodeToRoot(BinaryTreeNode<int> startNode)
    {
        BinaryTreeNode<int> current = startNode;
        int totalvalue = 0;

        //while loops are dangerous b/c they can create inescapeable loops
        while (current != null)
        {
            //print("Sanity check: Current value is: " + current.Value());
            totalvalue += current.Value();
            current = current.parent;
        }
        return totalvalue;
    }
    
    // Update is called once per frame
    void Update()
    {

    }
}
