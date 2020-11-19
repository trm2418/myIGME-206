using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest3Q2
{
    class Program
    {
        // Node class used by Dijsktra's Shortest Path algorithm
        public class Node : IComparable<Node>
        {
            public int nState;
            public List<Edge> edges = new List<Edge>();

            public int minCostToStart;
            public Node nearestToStart;
            public bool visited;

            public Node(int nState)
            {
                this.nState = nState;
                this.minCostToStart = int.MaxValue;
            }

            public void AddEdge(int cost, Node connection)
            {
                Edge e = new Edge(cost, connection);
                edges.Add(e);
            }

            public int CompareTo(Node n)
            {
                return minCostToStart.CompareTo(n.minCostToStart);
            }
        }

        // Edge class used to store connected nodes and the cost to travel there
        public class Edge : IComparable<Edge>
        {
            public int cost;
            public Node connectedNode;

            public Edge(int cost, Node connectedNode)
            {
                this.cost = cost;
                this.connectedNode = connectedNode;
            }

            public int CompareTo(Edge e)
            {
                return cost.CompareTo(e.cost);
            }
        }

        // Cell class used in the Linked List implementation
        public class Cell
        {
            public string color;

            // the edges and their costs
            public (Cell, int)[] edges = new (Cell, int)[] { };

            public Cell(string color)
            {
                this.color = color;
            }

            public void AddEdges(params (Cell, int)[] edges)
            {
                this.edges = edges;
            }
        }

        //       Red DBlue Gray LBlue Yellow Orange Purple Green
        // Red
        // DBlue
        // Gray
        // LBlue
        // Yellow
        // Orange
        // Purple
        // Greeen

        // used to convert the index of the node in nodes List to its color
        static string[] colors = new string[] { "red", "dark blue", "gray", "light blue", "yellow", "orange", "purple", "green" };

        // adjacency matrix representation of digraph
        static int[,] matrix = new int[,]
        {
            {-1, 1, 5, -1, -1, -1, -1, -1},
            {-1, -1, -1, 1, 8, -1, -1, -1},
            {-1, -1, -1, 0, -1, 1, -1, -1},
            {-1, 1, 0, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, 6},
            {-1, -1, -1, -1, -1, -1, 1, -1},
            {-1, -1, -1, -1, 1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1}
        };

        // adjacency list representation of digraph
        static (int, int)[][] list = new (int, int)[][]
        {
            new (int, int)[] {(1, 1), (2, 5)},
            new (int, int)[] {(3, 1), (4, 8)},
            new (int, int)[] {(3, 0), (5, 1)},
            new (int, int)[] {(1, 1), (2, 0)},
            new (int, int)[] {(7, 6)},
            new (int, int)[] {(6, 1)},
            new (int, int)[] {(4, 1)},
            new (int, int)[] {}
        };

        // stores all the nodes, used by Dijkstra methods
        static List<Node> nodes = new List<Node>();

        static LinkedList<Cell> linkedList = new LinkedList<Cell>();

        static void Main(string[] args)
        {
            // Linked List implementation
            Cell red = new Cell("red");
            Cell darkBlue = new Cell("dark blue");
            Cell gray = new Cell("gray");
            Cell lightBlue = new Cell("light blue");
            Cell yellow = new Cell("yellow");
            Cell orange = new Cell("orange");
            Cell purple = new Cell("purple");
            Cell green = new Cell("green");

            // Connect Linked List Cells to each other
            red.AddEdges((darkBlue, 1), (gray, 5));
            darkBlue.AddEdges((lightBlue, 1), (yellow, 8));
            gray.AddEdges((lightBlue, 0), (orange, 1));
            lightBlue.AddEdges((darkBlue, 1), (gray, 0));
            yellow.AddEdges((green, 6));
            orange.AddEdges((purple, 1));
            purple.AddEdges((yellow, 1));



            // Call and output DFS results
            Console.WriteLine("Depth First Search: ");
            DFS();


            // initialize nodes for Dijkstra
            Node node;

            node = new Node(0);
            nodes.Add(node);

            node = new Node(1);
            nodes.Add(node);

            node = new Node(2);
            nodes.Add(node);

            node = new Node(3);
            nodes.Add(node);

            node = new Node(4);
            nodes.Add(node);

            node = new Node(5);
            nodes.Add(node);

            node = new Node(6);
            nodes.Add(node);

            node = new Node(7);
            nodes.Add(node);

            // add edges to nodes
            nodes[0].AddEdge(1, nodes[1]);
            nodes[0].AddEdge(5, nodes[2]);
            nodes[0].edges.Sort();

            nodes[1].AddEdge(1, nodes[3]);
            nodes[1].AddEdge(8, nodes[4]);
            nodes[1].edges.Sort();

            nodes[2].AddEdge(0, nodes[3]);
            nodes[2].AddEdge(1, nodes[5]);
            nodes[2].edges.Sort();

            nodes[3].AddEdge(1, nodes[1]);
            nodes[3].AddEdge(0, nodes[2]);
            nodes[3].edges.Sort();

            nodes[4].AddEdge(6, nodes[7]);
            nodes[4].edges.Sort();

            nodes[5].AddEdge(1, nodes[6]);
            nodes[5].edges.Sort();

            nodes[6].AddEdge(1, nodes[4]);
            nodes[6].edges.Sort();

            // get the shortest path
            List<Node> shortestPath = GetShortestPathDijkstra();
            
            // output results
            Console.WriteLine("\nDijkstra's Shortest Path: ");
            foreach (Node noe in shortestPath)
            {
                Console.WriteLine(colors[nodes.IndexOf(noe)]);
            }
        }

        // Depth First Search method, 
        static void DFS()
        {
            bool[] visited = new bool[list.Length];
            DFSUtil(0, visited);
        }

        // Utility method used by DFS
        static void DFSUtil(int v, bool[] visited)
        {
            visited[v] = true;
            Console.WriteLine(colors[v]);

            (int, int)[] thisStateNeighbors = list[v];
            foreach((int, int) n in thisStateNeighbors)
            {
                if (!visited[n.Item1])
                {
                    DFSUtil(n.Item1, visited);
                }
            }
        }

        // Gets the shortest path using Dijkstra's algorithm
        static public List<Node> GetShortestPathDijkstra()
        {
            DijkstraSearch();
            List<Node> shortestPath = new List<Node>();
            shortestPath.Add(nodes[7]);
            BuildShortestPath(shortestPath, nodes[7]);
            shortestPath.Reverse();
            return shortestPath;
        }

        // the method for Dijkstra's Shortest Path algorithm
        static private void DijkstraSearch()
        {
            Node start = nodes[0];

            start.minCostToStart = 0;
            List<Node> prioQueue = new List<Node>();
            prioQueue.Add(start);

            do
            {
                // sort prioQueue by minCostToStart
                prioQueue.Sort();

                Node node = prioQueue.First();
                prioQueue.Remove(node);
                foreach (Edge cnn in node.edges)
                {
                    Node childNode = cnn.connectedNode;
                    if (childNode.visited)
                    {
                        continue;
                    }

                    if (childNode.minCostToStart == int.MaxValue || node.minCostToStart + cnn.cost < childNode.minCostToStart)
                    {
                        childNode.minCostToStart = node.minCostToStart + cnn.cost;
                        childNode.nearestToStart = node;
                        if (!prioQueue.Contains(childNode))
                        {
                            prioQueue.Add(childNode);
                        }
                    }
                }

                node.visited = true;

                if (node == nodes[7])
                {
                    return;
                }
            } while (prioQueue.Any());

            foreach (Node node in prioQueue)
            {
                Console.WriteLine(colors[nodes.IndexOf(node)]);
            }
        }

        // method that builds the shortest path
        static private void BuildShortestPath(List<Node> list, Node node)
        {
            if (node.nearestToStart == null)
            {
                return;
            }

            list.Add(node.nearestToStart);
            BuildShortestPath(list, node.nearestToStart);
        }
    }
}
