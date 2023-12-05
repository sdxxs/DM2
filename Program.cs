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
        Console.WriteLine("Програма написана для неорiєнтованого графа");
        char type = '-';
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

        int[,] MatrixDistance = CreateDistanceMatrix(MatrixAdj, type);

        FindRDC(MatrixDistance, V, type);

        WriteMatrix("Матриця вiдстаней R", MatrixAdj);

        int[,] D = ReachabilityMatrix(MatrixAdj);
        WriteMatrix("Матриця досяжностi D", D);

        // Рахуємо цикли

        for (int i = 0; i < N; i++)
        {
            graph[i] = new List<int>();
            cycles[i] = new List<int>();
        }

        for (int i = 0; i < Matrix.GetLength(0); i++)
        {
            addEdge(Matrix[i, 0], Matrix[i, 1]);
        }

        int[] color = new int[N];
        int[] par = new int[N];

        // Зберігаємо к-ть циклів

        cyclenumber = 0;


        dfs_cycle(1, 0, color, par);

        // Виводимо цикли на консоль
        Console.WriteLine("\n" + "Простi цикли");
        printCycles();

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

    static void FindRDC(int[,] MatrixDistance, int[] V, char type)
    {
        int n = V.Length;
        int radius = n;
        int maxE;
        int diametr = 0;
        int length = 0;
        int[] ArrCenter = new int[n];

        Console.Write("C - { ");
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
                    ArrCenter[length] = V[i];
                    length++;
                    Console.Write(V[i] + " ");
                }

            }
        }

        Array.Resize(ref ArrCenter, length);
        Console.Write("}");
        Console.WriteLine();
        Console.WriteLine("Diametr = " + diametr);
        Console.WriteLine("Radius = " + radius);
        Console.WriteLine();

        // Виводимо яруси для кожного центра
        for (int c = 0; c < ArrCenter.Length; c++)
        {
            Console.Write($"Центр {ArrCenter[c]}: ");
            for (int i = 1; i <= radius; i++)
            {
                Console.Write("\n");
                Console.Write($"Ярус {i}: ");

                for (int j = 0; j < MatrixDistance.GetLength(1); j++)
                {
                    if (i == MatrixDistance[ArrCenter[c] - 1, j]) 
                    { 
                        Console.Write($"{j+1} "); 
                    }
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine();
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

    static readonly int N = 100000;

    // змінні для використання
    // в обох функціях
    static List<int>[] graph = new List<int>[N];
    static List<int>[] cycles = new List<int>[N];
    static int cyclenumber;

    // Функція для позначення вершини
    // різними кольорами для різних циклів
    static void dfs_cycle(int u, int p, int[] color, int[] par)
    {
        // Вже (повністю) відвідана вершина.
        if (color[u] == 2)
        {
            return;
        }

        // Знайдена вершина, але вона не була повністю відвідана -> цикл знайдено.
        // відкотитися назад на основи батьківських вершин, щоб знайти повний цикл.
        if (color[u] == 1)
        {

            List<int> v = new List<int>();
            int cur = p;
            v.Add(cur);

            // Повернутись до вершини, яка знаходиться
            // в поточному циклі, що знайшов її
            while (cur != u)
            {
                cur = par[cur];
                v.Add(cur);
            }
            cycles[cyclenumber] = v;
            cyclenumber++;
            return;
        }
        par[u] = p;

        // Частково відвідані вершини
        color[u] = 1;

        // прості dfs на графі
        foreach (int v in graph[u])
        {
            // Якщо вона не була відвідана раніше
            if (v == par[u])
            {
                continue;
            }
            dfs_cycle(v, u, color, par);
        }

        // Повністю відвідані вершини
        color[u] = 2;
    }

    // Додати ребра до графа 
    static void addEdge(int u, int v)
    {
        graph[u].Add(v);
        graph[v].Add(u);
    }

    // Функція для виведення циклів
    static void printCycles()
    {
        // вивести всі вершини з одного і того самого циклу
        for (int i = 0; i < cyclenumber; i++)
        {
            // Вивести i-й цикл
            Console.Write("Цикл " + (i + 1) + ":");
            foreach (int x in cycles[i])
                Console.Write(" " + x);
            Console.WriteLine();
        }
        Console.ReadKey();
    }
}