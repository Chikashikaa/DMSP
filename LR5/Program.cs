using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR5
{
    class Program
    {
        private static int[,] ReadMatrix(string filePath)
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
        private static int[,] CopyMatrix(int[,] Gm)
        {
            int[,] copy = new int[Gm.GetLength(0), Gm.GetLength(1)];

            for (int i = 0; i < copy.GetLength(0); i++)
            {
                for (int j = 0; j < copy.GetLength(1); j++)
                {
                    copy[i, j] = Gm[i, j];
                }
            }

            return copy;
        }
        private static void PrintMatrix(int[,] Gm)
        {

            for (int i = 0; i < Gm.GetLength(0); i++)
            {
                for (int j = 0; j < Gm.GetLength(1); j++)
                {
                    Console.Write(Gm[i, j] + " ");
                }
                Console.WriteLine();
            }
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
        private static int AdjacencyCount(int[,] Gm)
        {
            int count, sum = 0;
            int[] adj = new int[Gm.GetLength(0)];

            for (int i = 0; i < Gm.GetLength(0); i++)
            {
                count = 0;
                for (int j = 0; j < Gm.GetLength(1); j++)
                {
                    if (Gm[i, j] != 0) count++;
                }
                adj[i] = count;
            }
            for (int i = 0; i < Gm.GetLength(0); i++)
            {
                sum += adj[i];
            }            
            return sum;
        }      
        private static int [,] SwapColumns(int[,] GM, int x, int y)
        {
            int tmp;
            for (int i = 0; i < GM.GetLength(0); i++)
            {
                tmp = GM[i, x];
                GM[i, x] = GM[i, y];
                GM[i, y] = tmp;
            }
            return GM;
        }
        private static int[,] SwapRows(int[,] GM, int x, int y)
        {
            int tmp;
            for (int j = 0; j < GM.GetLength(1); j++)
            {
                tmp = GM[x, j];
                GM[x, j] = GM[y, j];
                GM[y, j] = tmp;
            }
            return GM;
        }
        private static bool VerticeCheck(int[,] GM1, int[,] GM2)
        {
            if (GM1.GetLength(0) == GM2.GetLength(0))
            {
                Console.WriteLine("Кількість вершин однакова\n");
                return true;
            }
            else
            {
                Console.WriteLine("Кількість вершин не однакова\n");
                return false;
            }
        }
        private static bool EdgeCheck(int[,] GM1, int[,] GM2)
        {
            int eC1, eC2;
            eC1 = GetEdgeNumber(GM1);
            eC2 = GetEdgeNumber(GM2);
            if (eC1 == eC2)
            {
                Console.WriteLine("Кількість ребер однакова\n");
                return true;
            }
            else
            {
                Console.WriteLine("Кількість ребер не однакова\n");
                return false;
            }
        }
        private static bool AdjacencyCheck(int[,] GM1, int[,] GM2)
        {
            int adj1, adj2;
            adj1 = AdjacencyCount(GM1);
            adj2 = AdjacencyCount(GM2);
            if (adj1 == adj2)
            {
                Console.WriteLine("Сума степенів вершин однакова\n");
                return true;
            }
            else
            {
                Console.WriteLine("Сума степенів вершин не однакова\n");
                return false;
            }
        }
        private static bool MatrixCheck(int[,] GM1, int[,] GM2)
        {
            bool checker = false;
            for (int i = 0; i < GM1.GetLength(0); i++)
                for (int j = 0; j < GM1.GetLength(1); j++)
                    if (GM1[i, j] == GM2[i, j]) checker = true;
            return checker;
        }
        private static bool Permutations(int[,] GM1, int[,] GM2)
        {
            bool checker = false;
            int locker = 0, ch;
            int[,] copyGM1 = CopyMatrix(GM1), copyGM2 = CopyMatrix(GM2);
            List<int> rowGM1, rowGM2, ch1, ch2, colGM1, colGM2;
            if (MatrixCheck(copyGM1, copyGM2)) checker = MatrixCheck(copyGM1, copyGM2);
            for (int n = 0; n < copyGM1.GetLength(0); n++)
            {
                for (int i = 0; i < copyGM1.GetLength(0) - 1; i++)
                {
                    SwapRows(copyGM1, i, i + 1);
                    for (int j = 0; j < copyGM1.GetLength(1) - 1; j++)
                    {
                        SwapColumns(copyGM1, j, j + 1);
                        Console.WriteLine();
                        PrintMatrix(copyGM1);
                        if (MatrixCheck(copyGM1, GM2)) checker = MatrixCheck(copyGM1, GM2);
                    }
                }
            }
            return checker;
        }
        private static void Isomorphism(int[,] GM1, int[,] GM2)
        {
            if ((VerticeCheck(GM1, GM2) == false) || (EdgeCheck(GM1, GM2) == false) || (AdjacencyCheck(GM1, GM2) == false))
            {
                Console.WriteLine("Графи не ізоморфні");
            }
            else
            {
                bool isomorphism = MatrixCheck(GM1, GM2);
                if (isomorphism == true)
                {
                    Console.WriteLine("Графи ізоморфні");
                }
                else
                {
                    isomorphism = Permutations(GM1, GM2);
                    if (isomorphism == true)
                    {
                        Console.WriteLine("Графи ізоморфні");
                    }
                    else Console.WriteLine("Графи не ізоморфні");
                }
            }
        }
        static void Main(string[] args)
        {
            string fP1, fP2;
            int[,] GM1, GM2;

            fP1 = "C:\\Users\\chika\\source\\repos\\LR5\\l5_1.txt";
            fP2 = "C:\\Users\\chika\\source\\repos\\LR5\\l5_2.txt";

            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("\nГраф 1:");
            GM1 = ReadMatrix(fP1);
            Console.WriteLine("\nГраф 2:");
            GM2 = ReadMatrix(fP2);
            Console.WriteLine();

            Isomorphism(GM1, GM2);

            Console.ReadLine();
        }
    }
}
