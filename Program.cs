using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;


class Graph
{
    static void Main(string[] args)
    {
        int n, m;
        Console.Write("Введiть кiлькiсть вершин n: ");
        n = int.Parse(Console.ReadLine());
        Console.Write("Введiть кiлькiсть ребер m: ");
        m = int.Parse(Console.ReadLine());
        Console.WriteLine("Граф орiєнтовний/неорiєнтовний? Напишiть:" +
                "\n+ якщо граф орiєнтовний " +
                "\n- якщо граф неорiєнтовний.");
        char type = char.Parse(Console.ReadLine());
        Console.WriteLine("Введiть початкову та кiнцеву вершини через кому(приклад: 1, 2)");

        int[,] Matrix = new int[m, 2];

        // Парсимо дані
        for (int i = 0; i < m; i++)
        {
            string s = Console.ReadLine();
            string[] subs = s.Split(", ");
            Matrix[i, 0] = int.Parse(subs[0]);
            Matrix[i, 1] = int.Parse(subs[1]);
        }

        int[] V = GetUniqueVertices(Matrix);

        int[,] MatrixAdj = CreateAdjacencyMatrix(Matrix, V, type, m);

        WriteMatrix("Матриця сумiжностi", MatrixAdj);

        int[,] MatrixDistance = CreateDistanceMatrix(MatrixAdj,type);

        FindRDC(MatrixDistance,V,type);

        WriteMatrix("Матриця вiдстаней R", MatrixAdj);

        int[,] D = ReachabilityMatrix(MatrixAdj);
        WriteMatrix("Матриця досяжностi D", D);

        Console.ReadKey();
    }

    static int[] GetUniqueVertices(int[,] matrix)
    {
        HashSet<int> uniqueChars = new HashSet<int>();
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            uniqueChars.Add(matrix[i, 0]);
            uniqueChars.Add(matrix[i, 1]);
        }
        int[] V = new int[uniqueChars.Count];
        uniqueChars.CopyTo(V);
        Array.Sort(V); // Сортуємо вершини
        return V;
    }

    static int[,] CreateAdjacencyMatrix(int[,] matrix, int[] V, char type, int m)
    {
        int n = V.Length;
        int[,] MatrixAdj = new int[n, n];
        for (int i = 0; i < m; i++)
        {
            int vertex1 = matrix[i, 0];
            int vertex2 = matrix[i, 1];

            int index1 = Array.IndexOf(V, vertex1);
            int index2 = Array.IndexOf(V, vertex2);

            MatrixAdj[index1, index2] = 1;
            if (type == '-')
            {
                MatrixAdj[index2, index1] = 1;
            }
        }

        return MatrixAdj;
    }
    static int[,] CreateDistanceMatrix(int[,] MatrixAdj, char type)
    {
        
        int[,] MatrixDistance = MatrixAdj;
        int distance = 1;
        if (type == '-')
        {

            for (int r = 0; r < 3; r++)
            {
                for (int i = 0; i < MatrixDistance.GetLength(0); i++)
                {
                    for (int j = 0; j < MatrixDistance.GetLength(1); j++)
                    {
                        if (MatrixDistance[i, j] == 1)
                        {
                            for (int k = 0; k < MatrixDistance.GetLength(1); k++)
                            {
                                if (MatrixDistance[j, k] > 0)
                                {
                                    distance = MatrixDistance[j, k] + 1;
                                    if (((MatrixDistance[i, k] >= distance) || MatrixDistance[i, k] == 0) && (k != i))
                                    {
                                        MatrixDistance[i, k] = distance;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return MatrixDistance;
    }
    static void FindRDC(int[,] MatrixDistance, int[] V,char type)
    {
        int n = V.Length;
        int radius = n;
        int maxE;
        int diametr = 0;
        
        
            Console.Write("C - {");
        if (type == '-')
        {
            for (int i = 0; i < MatrixDistance.GetLength(0); i++)
            {
                maxE = MatrixDistance[i, 0];
                for (int j = 0; j < MatrixDistance.GetLength(1); j++)
                {
                    if (diametr <= MatrixDistance[i, j])
                    {
                        diametr = MatrixDistance[i, j];
                    }
                    if (maxE < MatrixDistance[i, j])
                    {
                        maxE = MatrixDistance[i, j];
                    }
                }
                if (maxE < radius) { radius = maxE; }
            }

            for (int i = 0; i < MatrixDistance.GetLength(0); i++)
            {

                maxE = MatrixDistance[i, 0];
                for (int j = 0; j < MatrixDistance.GetLength(1); j++)
                {

                    if (maxE < MatrixDistance[i, j])
                    {
                        maxE = MatrixDistance[i, j];
                    }
                }
                if (radius == maxE)
                {
                    Console.Write(V[i] + " ");
                }
            }
        }
            Console.Write("}");
        
        Console.WriteLine();
        Console.WriteLine("Diametr = " + diametr);
        Console.WriteLine("Radius = " + radius);

    }
    static void WriteMatrix(string text, int[,] Matrix)
    {
        Console.WriteLine(text);
        for (int i = 0; i < Matrix.GetLength(0); i++)
        {
            for (int j = 0; j < Matrix.GetLength(1); j++)
            {
                Console.Write(Matrix[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    static int[,] ReachabilityMatrix(int[,] A)
    /*Множення матриці саму на себе n-1 разів*/
    {
        int n = A.GetLength(0);
        int[,] result = new int[n, n];
        int[,] term = new int[n, n];
        int[,] I = new int[n, n];
        int[,] D = new int[n, n];
        int sum;

        // Створення одиничної матриці з розмірністю n
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (i == j)
                {
                    I[i, j] = 1;
                }
            }
        }
        Array.Copy(I, D, I.Length);  // Копіюємо одиничну матрицю в 
        Array.Copy(A, result, A.Length);
        Array.Copy(A, term, A.Length);

        for (int c = 1; c < n - 1; c++)
        {
            // Після кожного перемножання матриці A саму на себе, додаємо її до матриці досяжності D
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (D[i, j] + result[i, j] != 0)
                    {
                        D[i, j] = 1;
                    }
                }
            }

            // Множення матриці A саму на себе
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    sum = 0;
                    for (int k = 0; k < n; k++)
                    {
                        sum += term[i, k] * A[k, j];
                    }
                    result[i, j] = sum;
                }
            }
            Array.Copy(result, term, result.Length);
        }
        return D;
    }
}