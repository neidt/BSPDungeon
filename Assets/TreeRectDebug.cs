using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using Assets;
using UnityEngine;
public class TreeRectDebug : MonoBehaviour
{
    //dimensions of the overall level
    public int levelWidth;
    public int levelHeight;
    public int numSplits;
    List<BinaryTreeNode<RectInt>> leaves = new List<BinaryTreeNode<RectInt>>();
    List<BinaryTreeNode<RectInt>> processedLeaves = new List<BinaryTreeNode<RectInt>>();
    List<BinaryTreeNode<RectInt>> roomNodes = new List<BinaryTreeNode<RectInt>>();
    List<BinaryTreeNode<RectInt>> partitions = new List<BinaryTreeNode<RectInt>>();
    List<RectInt> cooridors = new List<RectInt>();

    void Start()
    {
        //root of "tree" of width and height of the level
        BinaryTree<RectInt> sampleRectTree;
        sampleRectTree = new BinaryTree<RectInt>(new RectInt(0, 0, levelWidth, levelHeight));

        //extra stuff VV
        /**
        *left partition dimensions
        *int partitionWidth = levelWidth / 2;
        *int partitionHeight = levelHeight;

        *make the left half of the level
        *RectInt leftPartitionRect = new RectInt(0, 0, partitionWidth, partitionHeight);

        *set that half of the level to a child of the root 
        *BinaryTreeNode<RectInt> leftPartitionNode = sampleRectTree.Root().AddChild(leftPartitionRect);

        *print("Left Half: " + leftPartitionNode.ToString());

        *right partition dimensions
        *int rightPartitionX = levelWidth / 2;
        *int rightPartitionY = 0;

        *make the right half of the level
        *RectInt rightPartitionRect = new RectInt(rightPartitionX, rightPartitionY, partitionWidth, partitionHeight);

        *also set that to a child node under the root 
        *BinaryTreeNode<RectInt> rightPartitionNode = sampleRectTree.Root().AddChild(rightPartitionRect);

        *print("Right Half: " + rightPartitionNode.ToString());

        *translates the local coordinates to world(level) coordinates
        *RectInt leftPartitionWorld = LocalNodeCoorsToWorldCoors(leftPartitionNode);
        *RectInt rightPartitionWorld = LocalNodeCoorsToWorldCoors(rightPartitionNode);
        */

        //old way of splitting
        /**
        ///variables for tree
        BinaryTreeNode<RectInt> leftChild, rightChild, topChild, bottomChild;
        
        //call the splitting functions
        print("creating vertical split");
        CreatePartitionVertical(sampleRectTree.Root(), out leftChild, out rightChild);

        print("creating horizontal split 1");
        CreatePartitionHorizontal(leftChild, out topChild, out bottomChild);

        print("creating horizontal split 2");
        CreatePartitionHorizontal(rightChild, out topChild, out bottomChild);
        */

        //old debugging stuff
        /**
        //might be wrong with this thing V??
        *print("root node: " + sampleRectTree.Root().Value());
        *print("left part:" + leftChild.Value());
        *print("Right part:" + rightChild.Value());

        *print("left child.left child" + leftChild.leftChild.Value());
        *print("left child.right child" + leftChild.rightChild.Value());
        *print("right child.left child" + rightChild.leftChild.Value());
        *print("right child.right child" + rightChild.rightChild.Value());*/

        //split recursively 
        RecursiveSplit(sampleRectTree.Root(), numSplits, true);

        //create partitions
        //CreateHorizCoors(sampleRectTree.Root());


        //display rooms
        DisplayRooms(sampleRectTree);
    }//end start

    private void RecursiveDiv(BinaryTreeNode<RectInt> T)
    {
        if (T.IsLeaf())
        {
            leaves.Add(T);
            return;
        }
        processedLeaves.Add(T);
        if (T.leftChild != null)
        {
            RecursiveDiv(T.leftChild);
        }
        if (T.rightChild != null)
        {
            RecursiveDiv(T.rightChild);
        }
    }//end recursiveDiv

    private void RecursiveSplit(BinaryTreeNode<RectInt> nodeToSplit, int numsplits, bool verticalDirection)
    {
        BinaryTreeNode<RectInt> leftChild, rightChild, topChild, bottomChild, coorChild;

        //if(numsplits == 0) return
        //else split t into 2 subpartitions
        //create new nodes for them , with t as parent, then do again
        //if even depth and vertiDir, split vertically, else split horizontally

        if (numsplits == 0)
        {
            //bottom level
            CreateRoom(nodeToSplit, roomNodes);
            return;
        }
        else
        {
            if (verticalDirection)
            {
                //split vertical
                print("created vertical partition");
                CreatePartitionVertical(nodeToSplit, out leftChild, out rightChild);
                numsplits--;
                RecursiveSplit(leftChild, numsplits, false);
                RecursiveSplit(rightChild, numsplits, false);
            }
            else
            {
                //split horizontal
                print("created horizontal partitions");
                CreatePartitionHorizontal(nodeToSplit, out topChild, out bottomChild);
                numsplits--;
                RecursiveSplit(topChild, numsplits, true);
                RecursiveSplit(bottomChild, numsplits, true);
            }
        }
    }//end recursive split

    private void CreatePartitionVertical(BinaryTreeNode<RectInt> nodeToSplit, out BinaryTreeNode<RectInt> leftChild, out BinaryTreeNode<RectInt> rightChild)
    {
        int vertSplitNodeX = 0;
        int vertSplitNodeY = 0;
        int vertSplitNodeWidth = nodeToSplit.Value().width / 2;
        int vertSplitNodeHeight = nodeToSplit.Value().height;

        RectInt newVertNodeLeft = new RectInt(vertSplitNodeX, vertSplitNodeY, vertSplitNodeWidth, vertSplitNodeHeight);
        RectInt newVertNodeRight = new RectInt(vertSplitNodeWidth, vertSplitNodeY, nodeToSplit.Value().width - vertSplitNodeWidth, vertSplitNodeHeight);

        //set children
        leftChild = nodeToSplit.AddChild(newVertNodeLeft);
        rightChild = nodeToSplit.AddChild(newVertNodeRight);

        //add to list of partitions
        partitions.Add(leftChild);
        partitions.Add(rightChild);

        //create coors
        RectInt vertCoor = CreateVertCoors(leftChild);
        //leftChild.AddCoor(vertCoor);

        //debug stuff
        print("LeftSide: " + newVertNodeLeft);
        print("RightSide: " + newVertNodeRight);
    }//end vertical split

    private void CreatePartitionHorizontal(BinaryTreeNode<RectInt> nodeToSplit, out BinaryTreeNode<RectInt> topChild, out BinaryTreeNode<RectInt> bottomChild)
    {
        int horizSplitNodeX = 0;
        int horizSplitNodeY = 0;
        int horizSplitNodeWidth = nodeToSplit.Value().width;
        int horizSplitNodeHeight = nodeToSplit.Value().height / 2;

        RectInt newHorizNodeTop = new RectInt(horizSplitNodeX, horizSplitNodeHeight, horizSplitNodeWidth, nodeToSplit.Value().height - horizSplitNodeHeight);
        RectInt newHorizNodeBottom = new RectInt(horizSplitNodeX, horizSplitNodeY, horizSplitNodeWidth, horizSplitNodeHeight);

        //set children
        topChild = nodeToSplit.AddChild(newHorizNodeTop);
        bottomChild = nodeToSplit.AddChild(newHorizNodeBottom);

        //add to list of partitions
        partitions.Add(topChild);
        partitions.Add(bottomChild);

        //create corridor and transfer to world coors
        RectInt horizCoor = CreateHorizCoors(bottomChild);
        //bottomChild.AddCoor(horizCoor);

        //debug stuff
        print("Top: " + newHorizNodeTop);
        print("Bottom: " + newHorizNodeBottom);
    }//end horizontal split

    private void CreateRoom(BinaryTreeNode<RectInt> nodeToRommify, List<BinaryTreeNode<RectInt>> roomNodes)
    {
        int roomWidth = nodeToRommify.Value().width - 2;
        int roomHeight = nodeToRommify.Value().height - 2;
        int roomXStart = 1;
        int roomYStart = 1;

        //create a room based off of the current node's(partition's) dimensions
        if (nodeToRommify.leftChild == null && nodeToRommify.rightChild == null)
        {
            //generate a room inside the chosen node
            RectInt room = new RectInt(roomXStart, roomYStart, roomWidth, roomHeight);
            BinaryTreeNode<RectInt> roomNode = nodeToRommify.AddChild(room);
            //LocalNodeCoorsToWorldCoors(roomNode);
            roomNodes.Add(roomNode);
        }
    }//end createRoom

    public RectInt CreateVertCoors(BinaryTreeNode<RectInt> partitionNode)
    {
        //if current partition is horizontal, then connect the top and bottom nodes
        //add a cooridor at a spot at the end of the room
        RectInt verticalCoor = new RectInt((partitionNode.Value().width - 1), (partitionNode.Value().height - 2), 2, 1);
        //add to child of partition
        BinaryTreeNode<RectInt> coorVertChild = partitionNode.AddCoor(verticalCoor);
        //convert to world coors
        //LocalNodeCoorsToWorldCoors(coorVertChild);
        cooridors.Add(verticalCoor);
        print("vertical cooridor at: " + verticalCoor.x + ", " + verticalCoor.y +
            " of height and width: " + verticalCoor.height + ", " + verticalCoor.width);
        return verticalCoor;
    }//end createVerticalCoors

    public RectInt CreateHorizCoors(BinaryTreeNode<RectInt> partitionNode)
    {
        RectInt horizontalCoor = new RectInt((partitionNode.Value().width - 2), (partitionNode.Value().height - 1), 1, 2);
        BinaryTreeNode<RectInt> coorHorzChild = partitionNode.AddCoor(horizontalCoor);
        //LocalNodeCoorsToWorldCoors(coorHorzChild);
        cooridors.Add(horizontalCoor);
        print("horizontal cooridor at: " + horizontalCoor.x + ", " + horizontalCoor.y +
            " of height and width: " + horizontalCoor.height + ", " + horizontalCoor.width);
        return horizontalCoor;
    }//end createHorizCoors

    private RectInt LocalNodeCoorsToWorldCoors(BinaryTreeNode<RectInt> node)
    {
        //something to do with the world coordinates v the local coords?? -> turns local coors into world coors
        BinaryTreeNode<RectInt> current = node;
        RectInt rectWorld = node.Value();
        rectWorld.x = 0;
        rectWorld.y = 0;

        while (current != null)
        {
            rectWorld.x += current.Value().x;
            rectWorld.y += current.Value().y;

            current = current.parent;
        }
        return rectWorld;
    }//end localCoorsToWorldCoors

    public void DisplayRooms(BinaryTree<RectInt> currentNode)
    {
        List<BinaryTreeNode<RectInt>> leaves = new List<BinaryTreeNode<RectInt>>();
        CollectLeaves(currentNode.Root(), leaves);
        List<BinaryTreeNode<RectInt>> coorLeaves = new List<BinaryTreeNode<RectInt>>();
        CollectCoors(currentNode.Root(), coorLeaves);

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        string[,] treeArray = new string[levelWidth, levelHeight];

        //int currentLeaf = 0;
        int currentCoorLeaf = 0;

        //setting the value of each section
        foreach (BinaryTreeNode<RectInt> leaf in leaves)
        {
            //currentLeaf++;
            print("Current leaf is: " + LocalNodeCoorsToWorldCoors(leaf));
            RectInt leafWorld = LocalNodeCoorsToWorldCoors(leaf);
            int leafX = leafWorld.x;
            int leafY = leafWorld.y;

            int currentLeafWidth = leafWorld.width;
            int currentLeafHeight = leafWorld.height;

            for (int cw = 0; cw < currentLeafWidth; cw++)
            {
                for (int ch = 0; ch < currentLeafHeight; ch++)
                {
                    //print("current leaf val: " + currentLeaf);
                    treeArray[(cw + leafX), (ch + leafY)] = "F\t";
                }
            }
        }

        foreach (BinaryTreeNode<RectInt> coorLeaf in coorLeaves)
        {
            currentCoorLeaf++;

            RectInt coorLeafWorld = LocalNodeCoorsToWorldCoors(coorLeaf);
            print("cooridor leaf is: " + coorLeaf.Value());

            int coorLeafX = coorLeafWorld.x;
            int coorLeafY = coorLeafWorld.y;

            int currentCoorLeafWidth = coorLeafWorld.width;
            int currentCoorLeafHeight = coorLeafWorld.height;

            for (int cw = 0; cw < currentCoorLeafWidth; cw++)
            {
                for (int ch = 0; ch < currentCoorLeafHeight; ch++)
                {
                    print("current leaf val: " + currentCoorLeaf);
                    treeArray[(cw + coorLeafX), (ch + coorLeafY)] = "C\t";
                }
            }
        }

        for (int j = 0; j < levelHeight; j++)
        {
            for (int i = 0; i < levelWidth; i++)
            {
                if (treeArray[i, j] == null)
                {
                    treeArray[i, j] = "E\t";
                }
            }
        }

        //print out stuff
        for (int j = 0; j < levelHeight; j++)
        {
            for (int i = 0; i < levelWidth; i++)
            {
                sb.Append(treeArray[i, j]);
            }
            sb.AppendLine();
            sb.AppendLine();
        }
        string content = sb.ToString();
        File.WriteAllText(Path.GetFullPath(@"Assets") + "\\sample.bspd", content);
        //File.WriteAllText("F:\\Unity Stuff\\4-9 to 4-16\\Assets\\sample.bspd", content);
    }//end display rooms

    private void CollectLeaves(BinaryTreeNode<RectInt> currentNode, List<BinaryTreeNode<RectInt>> leaves)
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
    }//end collect leaves

    private void CollectCoors(BinaryTreeNode<RectInt> currentNode, List<BinaryTreeNode<RectInt>> coorLeaves)
    {
        if (currentNode == null) return;

        if (currentNode.isCoorLeaf())
        {
            coorLeaves.Add(currentNode.coorChild);
            CollectCoors(currentNode.leftChild, coorLeaves);
            CollectCoors(currentNode.rightChild, coorLeaves);
        }
        else
        {
            CollectCoors(currentNode.leftChild, coorLeaves);
            CollectCoors(currentNode.rightChild, coorLeaves);
        }
    }
}//end class