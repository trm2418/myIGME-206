using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest3Q6
{
    class Program
    {
        public class BTree
        {
            //////////////////////////////////////////////////////////
            ///  The most important 3 fields of any Binary Search Tree

            // the "less than" branch off of this node
            public BTree ltChild;

            // the "greater than or equal to" branch off of this node
            public BTree gteChild;

            // the data contained in this node
            public object data;
            //////////////////////////////////////////////////////////


            // a boolean to indicate if this is actual data or seed data to prime the tree
            public bool isData;

            // internal counter which is needed by the visualizer
            public int id;

            // keep track of last counter to set this.id
            private static int nLastCntr;


            //////////////////////////////////////////////////////////
            // overload all operators to compare BTree nodes by int or string data
            public static bool operator ==(BTree a, BTree b)
            {
                bool returnVal = false;

                try
                {
                    if (a.data.GetType() == typeof(int))
                    {
                        returnVal = ((int)a.data == (int)b.data);
                    }

                    if (a.data.GetType() == typeof(string))
                    {
                        returnVal = ((string)a.data == (string)b.data);
                    }
                }
                catch
                {
                    returnVal = (a == (object)b);
                }

                return (returnVal);
            }

            public static bool operator !=(BTree a, BTree b)
            {
                bool returnVal = false;

                try
                {
                    if (a.data.GetType() == typeof(int))
                    {
                        returnVal = ((int)a.data != (int)b.data);
                    }

                    if (a.data.GetType() == typeof(string))
                    {
                        returnVal = ((string)a.data != (string)b.data);
                    }
                }
                catch
                {
                    returnVal = (a != (object)b);
                }

                return (returnVal);
            }

            public static bool operator <(BTree a, BTree b)
            {
                bool returnVal = false;

                try
                {
                    if (a.data.GetType() == typeof(int))
                    {
                        returnVal = ((int)a.data < (int)b.data);
                    }

                    if (a.data.GetType() == typeof(string))
                    {
                        returnVal = (((string)a.data).CompareTo((string)b.data) < 0);
                    }
                }
                catch
                {
                    returnVal = false;
                }

                return (returnVal);
            }

            public static bool operator >(BTree a, BTree b)
            {
                bool returnVal = false;

                try
                {
                    if (a.data.GetType() == typeof(int))
                    {
                        returnVal = ((int)a.data > (int)b.data);
                    }

                    if (a.data.GetType() == typeof(string))
                    {
                        returnVal = (((string)a.data).CompareTo((string)b.data) > 0);
                    }
                }
                catch
                {
                    returnVal = false;
                }

                return (returnVal);
            }

            public static bool operator >=(BTree a, BTree b)
            {
                bool returnVal = false;

                try
                {
                    if (a.data.GetType() == typeof(int))
                    {
                        returnVal = ((int)a.data >= (int)b.data);
                    }

                    if (a.data.GetType() == typeof(string))
                    {
                        returnVal = (((string)a.data).CompareTo((string)b.data) >= 0);
                    }
                }
                catch
                {
                    returnVal = false;
                }

                return (returnVal);
            }

            public static bool operator <=(BTree a, BTree b)
            {
                bool returnVal = false;

                try
                {
                    if (a.data.GetType() == typeof(int))
                    {
                        returnVal = ((int)a.data <= (int)b.data);
                    }

                    if (a.data.GetType() == typeof(string))
                    {
                        returnVal = (((string)a.data).CompareTo((string)b.data) <= 0);
                    }
                }
                catch
                {
                    returnVal = false;
                }

                return (returnVal);
            }
            //////////////////////////////////////////////////////////


            //////////////////////////////////////////////////////////
            // The constructor which will add the new node to an existing tree
            // if a non-null root is passed in
            // isData defaults to true, but can be set to false if seed data is being added to prime the tree
            public BTree(object nData, BTree root, bool isData = true)
            {
                this.ltChild = null;
                this.gteChild = null;
                this.data = nData;
                this.isData = isData;

                // set internal id which is used by the visualizer
                this.id = nLastCntr;
                ++nLastCntr;

                //form1.richTextBox1.Text += nData.ToString() + " ";

                // if a tree exists to add this node to
                if (root != null)
                {
                    AddChildNode(this, root);
                }
            }
            //////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////
            /// Recursive AddChildNode method adds BTree nodes to an existing tree
            public static void AddChildNode(BTree newNode, BTree treeNode)
            {
                // if the new node >= the tree node (use the operator overrides)
                if (newNode >= treeNode)
                {
                    // if there is not a child node greater than this tree node (ie. gteChild == null)
                    if (treeNode.gteChild == null)
                    {
                        // set this node's "greater than or equal to" child to this new node
                        treeNode.gteChild = newNode;
                    }
                    else
                    {
                        // otherwise recursively add the new node to the "greater than or equal to" branch
                        AddChildNode(newNode, treeNode.gteChild);
                    }
                }
                else
                {
                    // the new node < this tree node
                    // if there is not a child node less than this tree node (ie. ltChild == null)
                    if (treeNode.ltChild == null)
                    {
                        // set this node's "less than" child to this new node
                        treeNode.ltChild = newNode;
                    }
                    else
                    {
                        // otherwise recursively add the new node to the "less than" branch
                        AddChildNode(newNode, treeNode.ltChild);
                    }
                }
            }


            //////////////////////////////////////////////////////////
            // Print the tree in ascending order
            public static void TraverseAscending(BTree node)
            {
                if (node != null)
                {
                    // handle "less than" children
                    TraverseAscending(node.ltChild);

                    if (node.isData)
                    {
                        // handle current node
                    }

                    // handle "greater than or equal to children"
                    TraverseAscending(node.gteChild);
                }
            }
        }

        static List<int> numbers = new List<int>();

        // BalanceTree method
        // uses a min and max int variable to find the midpoint of a subsection of the
        // numbers list. recursively calls itself until the subsection is empty
        static void BalanceTree(BTree root, BTree node, List<int> nums, int min, int max)
        {
            if (min != max)
            {
                // find the middle index
                int midIndex = min + (max - min) / 2;

                // add middle index to tree
                node = new BTree(nums[midIndex], root);

                // if this is the first time this function runs
                if (min == 0 && max == numbers.Count)
                {
                    // set this node as the root
                    root = node;
                }

                // recursively call this method with the left and right subsection
                BalanceTree(root, node, nums, min, midIndex);
                BalanceTree(root, node, nums, midIndex + 1, max);
            }
        }

        static void Main(string[] args)
        {
            BTree root = null;
            BTree node = null;

            // add numbers to the tree and numbers List
            foreach (int i in new int[] { 1, 5, 15, 20, 21, 22, 23, 24, 25, 30, 35, 37, 40, 55, 60})
            {
                node = new BTree(i, root);
                numbers.Add(i);

                if (i == 1)
                {
                    root = node;
                }
            }

            // traverse the tree in ascending order
            BTree.TraverseAscending(root);

            // sort the numbers List so that it works with BalanceTree method
            numbers.Sort();

            // call BalanceTree method
            BalanceTree(root, node, numbers, 0, numbers.Count);
        }
    }
}
