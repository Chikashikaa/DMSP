using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LR2
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
        public struct Adjacency
        {
            public List<int> AdjVertice;
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
        private static Adjacency[] VerticeAdjacency(Graph g, int[,] Gm)
        {
            Adjacency[] adj = new Adjacency[g.VerticesCount];
             for(int i = 0; i < g.VerticesCount; i++)
            {
                adj[i].AdjVertice = new List<int>();
                for (int j = 0; j < g.VerticesCount; j++)
                {
                    if(Gm[i,j] != 0)
                        adj[i].AdjVertice.Add(j);                    
                }
                //adj[i].AdjVertice.ForEach(x => { Console.WriteLine("Root: {0} - Adjacency: {1}", i + 1, x + 1); });
            }
            return adj;
        }
        private static bool EulerPathCheck(Adjacency[] adj, int v)
        {
            int c = 0;
            for (int i = 0; i < v; i++)
            {
                if (adj[i].AdjVertice.Count() % 2 == 1) c++;
            }
            if ((c != 2)&&(c != 0))
                return false;            
            else return true;
        }
        private static bool EulerCycleCheck(Adjacency[] adj, int v)
        {
            int c = 0;
            for (int i = 0; i < v; i++)
            {
                if (adj[i].AdjVertice.Count() % 2 == 1) c++;
                if (c != 0) return false;
            }
            return true;
        }
        private static int DFS(int v, bool[] visited, Adjacency[] adj)
        {
            visited[v] = true;
            int count = 1;

            foreach (int i in adj[v].AdjVertice)
            {
                int n = i;
                if (!visited[n])
                    count += DFS(n, visited, adj);
            }
            return count;
        }
        private static void AddEdge(int u, int v, Adjacency[] adj)
        {
            adj[u].AdjVertice.Add(v);
            adj[v].AdjVertice.Add(u);
        }
        private static void RemoveEdge(int u, int v, Adjacency[] adj)
        {
            adj[u].AdjVertice.Remove(v);
            adj[v].AdjVertice.Remove(u);
        }
        private static bool IsValidNextEdge(int u, int v, int vc, Adjacency[] adj)
        {
            if (adj[u].AdjVertice.Count == 1)
            {
                return true;
            }
            bool[] isVisited = new bool[vc];
            int count1 = DFS(u, isVisited, adj);
            RemoveEdge(u, v, adj);
            isVisited = new bool[vc];
            int count2 = DFS(u, isVisited, adj);
            AddEdge(u, v, adj);
            return count1 <= count2;
        }
        private static void PrintEulerUtil(int u, int vc, Adjacency[] adj)
        {
            for (int i = 0; i < adj[u].AdjVertice.Count; i++)
            {
                int v = adj[u].AdjVertice[i];
                if (IsValidNextEdge(u, v, vc, adj))
                {
                    Console.Write(u + 1 + "-" + (v + 1) + " ");
                    RemoveEdge(u, v, adj);
                    PrintEulerUtil(v, vc, adj);
                }
            }
        }
        private static void PrintEulerTour(int vc, Adjacency[] adj)
        {
            int u = 0;
            for (int i = 0; i < vc; i++)
            {
                if (adj[i].AdjVertice.Count % 2 == 1)
                {
                    u = i;
                    break;
                }
            }
            PrintEulerUtil(u, vc, adj);
            Console.WriteLine();
        }
        static void Main(string[] args)
        {
            string filePath;
            int[,] GMatrix;
            int verticeCount, edgeCount;
            Graph graph;
            Adjacency[] adjacency;
            bool epath, ecycle;

            filePath = "C:\\Users\\chika\\source\\repos\\LR2\\l2_2ed_path.txt";
            GMatrix = ReadGMatrix(filePath);
            verticeCount = GMatrix.GetLength(0);
            edgeCount = GetEdgeNumber(GMatrix);
            graph = FillGraph(CreateGraph(verticeCount, edgeCount), GMatrix);
            adjacency = VerticeAdjacency(graph, GMatrix);
            epath = EulerPathCheck(adjacency, verticeCount);
            ecycle = EulerCycleCheck(adjacency, verticeCount);

            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("\nВершин: {0}", graph.VerticesCount);
            Console.WriteLine("Ребер: {0}\n", graph.EdgesCount);
            if (ecycle != true)
            {
                Console.WriteLine("Ейлеровий цикл не існує");
                if (epath != true)
                    Console.WriteLine("Ейлеровий шлях не існує");
                else
                {
                    Console.WriteLine("Ейлеровий шлях існує");
                    Console.WriteLine("Ейлеровий шлях: ");
                    PrintEulerTour(verticeCount, adjacency);
                }
            }
            else
            {
                Console.WriteLine("Ейлеровий цикл існує");
                Console.WriteLine("Ейлеровий цикл: ");
                PrintEulerTour(verticeCount, adjacency);
            }
            
            Console.ReadKey();
        }
    }
}
