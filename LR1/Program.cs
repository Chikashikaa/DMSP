using System;
using System.IO;
using System.Text;

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

        private static int[,] ReadGMatrix(string filePath)
        {
            string[] f = File.ReadAllLines(@filePath);

            int v = Int32.Parse(f[0]);
            int[,] Gm = new int[v, v];
            Console.WriteLine(v);
            for (int i = 1; i < v + 1; i++)
            {
                string[] w = f[i].Split(' ');
                for (int j = 0; j < v; j++)
                {
                    Gm[i - 1, j] = Convert.ToInt32(w[j]);
                }
                Console.WriteLine(f[i]);
            }
            return Gm;
        }
        private static int GetEdgeNumber(int[,] Gm)
        {
            int e = 0;
            for (int i = 0; i < Gm.GetLength(0); i++)
            {
                for (int j = 0; j < Gm.GetLength(1); j++)
                {
                    if (Gm[i, j] != 0)
                    {
                        e++;
                    }
                }
            }
            e /= 2;
            return e;
        }       
        private static Graph CreateGraph(int v, int e)
        {
            Graph g = new Graph();
            g.VerticesCount = v;
            g.EdgesCount = e;
            g.edge = new Edge[g.EdgesCount];

            return g;
        }
        private static Graph FillGraph(Graph g, int[,] Gm)
        {
            Console.WriteLine("\nГраф:");
            int e = 0;
            for (int i = 0; i < Gm.GetLength(0); i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (Gm[i, j] != 0)
                    {
                        g.edge[e].Source = j;
                        g.edge[e].Destination = i;
                        g.edge[e].Weight = Gm[i, j];
                        Console.WriteLine("{0} -- {1} == {2}", g.edge[e].Source + 1, g.edge[e].Destination + 1, g.edge[e].Weight);                        
                        e++;
                    }
                }
            }
            Console.WriteLine();
            return g;
        }
        private static int Find(Subset[] sub, int i)
        {
            if (sub[i].Parent != i)
                sub[i].Parent = Find(sub, sub[i].Parent);

            return sub[i].Parent;
        }
        private static void Union(Subset[] sub, int x, int y)
        {
            int xroot = Find(sub, x);
            int yroot = Find(sub, y);

            if (sub[xroot].Rank < sub[yroot].Rank)
                sub[xroot].Parent = yroot;
            else if (sub[xroot].Rank > sub[yroot].Rank)
                sub[yroot].Parent = xroot;
            else
            {
                sub[yroot].Parent = xroot;
                ++sub[xroot].Rank;
            }
        }
        private static void Print(Edge[] result, int e)
        {
            Console.WriteLine("\nОстове дерево:");
            for (int i = 0; i < e; ++i)
                Console.WriteLine("{0} -- {1} == {2}", result[i].Source + 1, result[i].Destination + 1, result[i].Weight);
        }
        private static void Kruskal(Graph g)
        {
            int vc = g.VerticesCount;
            Edge[] result = new Edge[vc];
            int i = 0;
            int e = 0;

            Array.Sort(g.edge, delegate (Edge a, Edge b)
            {
                return a.Weight.CompareTo(b.Weight);
            });

            Subset[] subsets = new Subset[vc];

            for (int v = 0; v < vc; ++v)
            {
                subsets[v].Parent = v;
                subsets[v].Rank = 0;
            }

            while (e < vc - 1)
            {
                Edge nextEdge = g.edge[i++];
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
            string filePath;
            int[,] GMatrix;
            int verticeCount;
            int edgeCount;
            Graph graph;

            filePath = "C:\\Users\\chika\\source\\repos\\LR1\\l1_2.txt";
            GMatrix = ReadGMatrix(filePath);
            verticeCount = GMatrix.GetLength(0);
            edgeCount = GetEdgeNumber(GMatrix);
            graph = FillGraph(CreateGraph(verticeCount, edgeCount), GMatrix);

            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Кількість вершин: {0}", graph.VerticesCount);
            Console.WriteLine("Кількість ребер: {0}", graph.EdgesCount);
            Kruskal(graph);
            Console.ReadLine();
        }        
    }
}
