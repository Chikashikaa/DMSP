using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LR4
{
    class Program
    {
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
        private static bool BFS(int[,] RMatrix, int verticeCount, int source, int sink, int[] parent)
        {            
            bool[] visited = new bool[verticeCount];
            List<int> queue = new List<int>();

            for (int i = 0; i < verticeCount; ++i) visited[i] = false;
            queue.Add(source);
            visited[source] = true;
            parent[source] = -1;

            while (queue.Count != 0)
            {
                int u = queue[0];
                queue.RemoveAt(0);
                for (int v = 0; v < verticeCount; v++)
                {
                    if ((visited[v] == false) && (RMatrix[u, v] > 0))
                    {
                        if (v == sink)
                        {
                            parent[v] = u;
                            return true;
                        }
                        queue.Add(v);
                        parent[v] = u;
                        visited[v] = true;
                    }
                }
            }
            return false;
        }
        private static int FordFulkerson(int[,] GMatrix, int verticeCount, int s, int t)
        {
            int u, v, path_flow = int.MaxValue, max_flow = 0;
            int[] resPath = new int[verticeCount];
            int[,] RMatrix = new int[verticeCount, verticeCount];

            for (u = 0; u < verticeCount; u++)
            {
                for (v = 0; v < verticeCount; v++)
                {
                    RMatrix[u, v] = GMatrix[u, v];
                }
            }
            while (BFS(RMatrix, verticeCount, s, t, resPath))
            {
                for (v = t; v != s; v = resPath[v])
                {
                    u = resPath[v];
                    path_flow
                        = Math.Min(path_flow, RMatrix[u, v]);
                }
                for (v = t; v != s; v = resPath[v])
                {
                    u = resPath[v];
                    RMatrix[u, v] -= path_flow;
                    RMatrix[v, u] += path_flow;
                }
                max_flow += path_flow;
            }
            return max_flow;
        }
        public static void Main()
        {
            string filePath;
            int[,] GMatrix;
            int verticeCount, source, sink;

            filePath = "C:\\Users\\chika\\source\\repos\\LR4\\l4_2.txt";
            GMatrix = ReadGMatrix(filePath);
            verticeCount = GMatrix.GetLength(0);
            source = 0;
            sink = 7;

            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("\nМаксимальний потік: " + FordFulkerson(GMatrix, verticeCount, source, sink));
            Console.ReadLine();
        }
    }
}
