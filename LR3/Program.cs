using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LR3
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
        private static int    GetEdgeNumber(int[,] Gm)
            {
                int e = 0;
                for (int i = 0; i < Gm.GetLength(0); i++)
                {
                    for (int j = 0; i > j; j++)
                    {
                        if (Gm[i, j] != 0)
                        {
                            e++;
                        }
                    }
                }
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
        private static int FindMin(int[,] GMatrix, int startpoint, bool[] isvisited)
        {
            int nx = 0, min = 1000;
            for (int i = 0; i < GMatrix.GetLength(0); i++)
            {
                if (GMatrix[startpoint, i] != 0)
                {
                    if ((min > GMatrix[startpoint, i])&&(isvisited[i] != true))
                    {
                        min = GMatrix[startpoint, i];
                        nx = i;
                    }
                }
            }
            return nx;
        }
        private static Edge[] NearestNeighbor(Edge[] resEdge, int[,] GMatrix, int startpoint)
        {
            int verticeCount = GMatrix.GetLength(0), nx, u;
            bool[] isvisited = new bool[verticeCount];
            u = startpoint;
            for (int i = 0; i < verticeCount; i++)
            {
                if (isvisited[startpoint] == false)
                {
                    isvisited[startpoint] = true;
                    nx = FindMin(GMatrix, startpoint, isvisited);
                    resEdge[i].Source = startpoint;
                    resEdge[i].Destination = nx;
                    resEdge[i].Weight = GMatrix[startpoint, nx];
                    startpoint = nx;
                }
            }
            resEdge[verticeCount - 1].Source = startpoint;
            resEdge[verticeCount - 1].Destination = u;
            resEdge[verticeCount - 1].Weight = GMatrix[u, startpoint];
            return resEdge;
        }
        public static void Main(String[] args)
        {
            string filePath;
            int[,] GMatrix;
            int verticeCount, edgeCount, startpoint, w = 0, nx;
            Graph graph;
            Edge[] resEdge;
            Random rnd = new Random();

            filePath = "C:\\Users\\chika\\source\\repos\\LR3\\l3_2.txt";
            GMatrix = ReadGMatrix(filePath);
            verticeCount = GMatrix.GetLength(0);
            edgeCount = GetEdgeNumber(GMatrix);
            resEdge = new Edge[verticeCount];
            graph = FillGraph(CreateGraph(verticeCount, edgeCount), GMatrix);
            startpoint = rnd.Next(0, verticeCount);

            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("\nВершин: {0}", verticeCount);
            Console.WriteLine("Ребер: {0}\n", edgeCount);
            Console.WriteLine("\nСтартова точка: " + (startpoint + 1));

            NearestNeighbor(resEdge, GMatrix, startpoint);
            for (int i = 0; i < resEdge.Length; i++)
            {
                Console.WriteLine("{0} ребро шляху:  {1} -- {2} = {3}", i + 1, resEdge[i].Source + 1, resEdge[i].Destination + 1, resEdge[i].Weight);
                w += resEdge[i].Weight;
            }
            Console.WriteLine("\nВага отриманого шляху: " + w); 
            Console.ReadLine();
        }
    }
}