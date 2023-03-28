using System;
using System.IO;

namespace LR1
{
    class Program
    {
        public struct Edge
        {
            public int Source;
            public int Destination;
            public int Weight;
        }
        public struct Graph
        {
            public int VerticesCount;
            public int EdgesCount;
            public Edge[] edge;
        }
        public struct Subset
        {
            public int Parent;
            public int Rank;
        }

        public static int[,] ReadGMatrix(string filePath)
        {
            string[] input = File.ReadAllLines(@filePath);

            int VerticlesNum = Int32.Parse(input[0]);
            int[,] GMatrix = new int[VerticlesNum, VerticlesNum];
            Console.WriteLine(VerticlesNum);
            for (int i = 1; i < VerticlesNum + 1; i++)
            {
                string[] weights = input[i].Split(' ');
                for (int j = 0; j < VerticlesNum; j++)
                {
                    GMatrix[i - 1, j] = Convert.ToInt32(weights[j]);
                }
                Console.WriteLine(input[i]);
            }
            return GMatrix;
        }
        public static int GetEdgeNumber(int[,] GMatrix)
        {
            int edgeCount = 0;
            for (int i = 0; i < GMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < GMatrix.GetLength(1); j++)
                {
                    if (GMatrix[i, j] != 0)
                    {
                        edgeCount++;
                    }
                }
            }
            edgeCount = edgeCount / 2;
            return edgeCount;
        }       
        public static Graph CreateGraph(int verticesCount, int edgesCount)
        {
            Graph graph = new Graph();
            graph.VerticesCount = verticesCount;
            graph.EdgesCount = edgesCount;
            graph.edge = new Edge[graph.EdgesCount];

            return graph;
        }
        public static Graph FillGraph(Graph graph, int[,] GMatrix)
        {
            Console.WriteLine("\nГраф:");
            int e = 0;
            for (int i = 0; i < GMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (GMatrix[i, j] != 0)
                    {
                        graph.edge[e].Source = i;
                        graph.edge[e].Destination = j;
                        graph.edge[e].Weight = GMatrix[i, j];
                        Console.WriteLine("{0} -- {1} == {2}", graph.edge[e].Source, graph.edge[e].Destination, graph.edge[e].Weight);                        
                        e++;
                    }
                }
            }
            Console.WriteLine();
            return graph;
        }
        public static int Find(Subset[] subsets, int i)
        {
            if (subsets[i].Parent != i)
                subsets[i].Parent = Find(subsets, subsets[i].Parent);

            return subsets[i].Parent;
        }
        public static void Union(Subset[] subsets, int x, int y)
        {
            int xroot = Find(subsets, x);
            int yroot = Find(subsets, y);

            if (subsets[xroot].Rank < subsets[yroot].Rank)
                subsets[xroot].Parent = yroot;
            else if (subsets[xroot].Rank > subsets[yroot].Rank)
                subsets[yroot].Parent = xroot;
            else
            {
                subsets[yroot].Parent = xroot;
                ++subsets[xroot].Rank;
            }
        }
        public static void Print(Edge[] result, int e)
        {
            for (int i = 0; i < e; ++i)
                Console.WriteLine("{0} -- {1} == {2}", result[i].Source, result[i].Destination, result[i].Weight);
        }
        public static void Kruskal(Graph graph)
        {
            int verticesCount = graph.VerticesCount;
            Edge[] result = new Edge[verticesCount];
            int i = 0;
            int e = 0;

            Array.Sort(graph.edge, delegate (Edge a, Edge b)
            {
                return a.Weight.CompareTo(b.Weight);
            });

            Subset[] subsets = new Subset[verticesCount];

            for (int v = 0; v < verticesCount; ++v)
            {
                subsets[v].Parent = v;
                subsets[v].Rank = 0;
            }

            while (e < verticesCount - 1)
            {
                Edge nextEdge = graph.edge[i++];
                int x = Find(subsets, nextEdge.Source);
                int y = Find(subsets, nextEdge.Destination);

                if (x != y)
                {
                    result[e++] = nextEdge;
                    Union(subsets, x, y);
                }
            }

            Print(result, e);
        }
        static void Main(string[] args)
        {
            string filePath = "C:\\Users\\chika\\source\\repos\\LR1\\l1_2.txt";
            int[,] GMatrix = ReadGMatrix(filePath);
            int verticeCount = GMatrix.GetLength(0);
            int edgeCount = GetEdgeNumber(GMatrix);
            Graph graph = CreateGraph(verticeCount, edgeCount);
            graph = FillGraph(graph, GMatrix);
            Console.WriteLine(graph.VerticesCount);
            Console.WriteLine(graph.EdgesCount);
            Kruskal(graph);
            Console.ReadLine();
        }        
    }
}
