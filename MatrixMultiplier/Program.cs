using System;
using System.Collections.Generic;
using System.Text;

public class Program
{
    static Dictionary<string, Matrix2D> store = new Dictionary<string, Matrix2D>(StringComparer.OrdinalIgnoreCase);

    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8; // (opsiyonel) bazı karakterler düzgün çıksın

        // Başlangıç örnekleri (istersen sil)
        store["A"] = new Matrix2D(new int[,] { { 2, 1 }, { 3, 4 } });
        store["B"] = new Matrix2D(new int[,] { { 5, 6 }, { 7, 8 } });

        while (true)
        {
            Console.WriteLine("\n===== MATRIX MULTIPLIER MENU =====");
            Console.WriteLine("1) Add new matrix (input)");
            Console.WriteLine("2) List matrices");
            Console.WriteLine("3) Multiply two matrices");
            Console.WriteLine("4) Run quick tests (PASS/FAIL)");
            Console.WriteLine("0) Exit");
            Console.Write("Choose: ");

            string choice = Console.ReadLine()?.Trim() ?? "";

            switch (choice)
            {
                case "1":
                    AddMatrixFlow();
                    break;

                case "2":
                    ListMatrices();
                    break;

                case "3":
                    MultiplyFlow();
                    break;

                case "4":
                    RunQuickTests();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }
    }

    static void AddMatrixFlow()
    {
        Console.Write("\nMatrix name (example: M1): ");
        string name = (Console.ReadLine() ?? "").Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Name cannot be empty.");
            return;
        }

        int rows = ReadInt("Number of rows: ");
        int cols = ReadInt("Number of columns: ");

        if (rows <= 0 || cols <= 0)
        {
            Console.WriteLine("Rows/Cols must be > 0.");
            return;
        }

        int[,] data = new int[rows, cols];

        Console.WriteLine("Enter each row values separated by space or comma.");
        Console.WriteLine($"Example for {cols} columns: 1 2 3   OR   1,2,3\n");

        for (int r = 0; r < rows; r++)
        {
            while (true)
            {
                Console.Write($"Row {r + 1}: ");
                string line = Console.ReadLine() ?? "";
                int[] values = ParseRow(line);

                if (values.Length != cols)
                {
                    Console.WriteLine($"Please enter exactly {cols} integers.");
                    continue;
                }

                for (int c = 0; c < cols; c++)
                    data[r, c] = values[c];

                break;
            }
        }

        store[name] = new Matrix2D(data);
        Console.WriteLine($"\nSaved '{name}' ({rows}x{cols}).");
        Console.WriteLine(store[name].OutputMatrix());
    }

    static void ListMatrices()
    {
        Console.WriteLine("\n----- Stored Matrices -----");
        if (store.Count == 0)
        {
            Console.WriteLine("(none)");
            return;
        }

        foreach (var kv in store)
        {
            Matrix2D m = kv.Value;
            Console.WriteLine($"{kv.Key}: {m.NumberOfRows()}x{m.NumberOfColumns()}");
        }
    }

    static void MultiplyFlow()
    {
        if (store.Count < 2)
        {
            Console.WriteLine("You need at least 2 matrices stored.");
            return;
        }

        Console.WriteLine("\nPick two matrices to multiply: (A x B)");
        ListMatrices();

        Console.Write("\nFirst matrix name (A): ");
        string aName = (Console.ReadLine() ?? "").Trim();

        Console.Write("Second matrix name (B): ");
        string bName = (Console.ReadLine() ?? "").Trim();

        if (!store.ContainsKey(aName) || !store.ContainsKey(bName))
        {
            Console.WriteLine("One (or both) matrix names not found.");
            return;
        }

        Matrix2D A = store[aName];
        Matrix2D B = store[bName];

        Console.WriteLine($"\n{aName} =\n{A.OutputMatrix()}");
        Console.WriteLine($"{bName} =\n{B.OutputMatrix()}");

        Matrix2D result = Matrix2D.Multiply(A, B);

        if (result.NumberOfRows() == 0 && result.NumberOfColumns() == 0)
        {
            Console.WriteLine("Multiply not possible: A columns must equal B rows.");
            return;
        }

        Console.WriteLine($"\nResult ({aName} x {bName}) =");
        Console.WriteLine(result.OutputMatrix());

        Console.Write("Save result as a new matrix? (y/n): ");
        string save = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();

        if (save == "y" || save == "yes")
        {
            Console.Write("New matrix name: ");
            string newName = (Console.ReadLine() ?? "").Trim();
            if (string.IsNullOrWhiteSpace(newName))
            {
                Console.WriteLine("Name cannot be empty. Not saved.");
                return;
            }

            store[newName] = result;
            Console.WriteLine($"Saved '{newName}' ({result.NumberOfRows()}x{result.NumberOfColumns()}).");
        }
    }

    static void RunQuickTests()
    {
        Console.WriteLine("\n----- QUICK TESTS (PASS/FAIL) -----");

        Matrix2D a = new Matrix2D(new int[,] { { 2, 1 }, { 3, 4 } });
        Matrix2D b = new Matrix2D(new int[,] { { 5, 6 }, { 7, 8 } });
        Matrix2D ab = Matrix2D.Multiply(a, b);

        RunTest("Test 1: Known example A x B", ab, new int[,] { { 17, 20 }, { 43, 50 } });

        Matrix2D I = new Matrix2D(new int[,] { { 1, 0 }, { 0, 1 } });
        RunTest("Test 2: A x I = A", Matrix2D.Multiply(a, I), a.matrix);

        Matrix2D Z = new Matrix2D(new int[,] { { 0, 0 }, { 0, 0 } });
        RunTest("Test 3: A x Zero = Zero", Matrix2D.Multiply(a, Z), Z.matrix);

        Matrix2D A23 = new Matrix2D(new int[,] { { 1, 2, 3 }, { 4, 5, 6 } }); // 2x3
        Matrix2D B22 = new Matrix2D(new int[,] { { 1, 2 }, { 3, 4 } });       // 2x2
        Matrix2D bad = Matrix2D.Multiply(A23, B22);

        Console.WriteLine("\nExtra: 2x3 x 2x2 should be invalid (empty 0x0)");
        Console.WriteLine($"Rows: {bad.NumberOfRows()}, Cols: {bad.NumberOfColumns()}");
    }

    static void RunTest(string name, Matrix2D got, int[,] expected)
    {
        Console.WriteLine("\n" + name);
        bool pass = AreEqual(got.matrix, expected);
        Console.WriteLine(pass ? "PASS" : "FAIL");
        Console.WriteLine("Got:");
        Console.WriteLine(got.OutputMatrix());
    }

    static bool AreEqual(int[,] x, int[,] y)
    {
        if (x.GetLength(0) != y.GetLength(0) || x.GetLength(1) != y.GetLength(1))
            return false;

        for (int i = 0; i < x.GetLength(0); i++)
            for (int j = 0; j < x.GetLength(1); j++)
                if (x[i, j] != y[i, j]) return false;

        return true;
    }

    static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string s = (Console.ReadLine() ?? "").Trim();
            if (int.TryParse(s, out int val))
                return val;

            Console.WriteLine("Please enter a valid integer.");
        }
    }

    static int[] ParseRow(string line)
    {
        // Hem boşluk hem virgül desteklesin
        string cleaned = line.Replace(",", " ");
        string[] parts = cleaned.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        int[] values = new int[parts.Length];
        for (int i = 0; i < parts.Length; i++)
        {
            if (!int.TryParse(parts[i], out values[i]))
                return Array.Empty<int>();
        }
        return values;
    }
}

public class Matrix2D
{
    public int[,] matrix;

    public Matrix2D()
    {
        matrix = new int[0, 0];
    }

    public Matrix2D(int rows, int cols)
    {
        matrix = new int[rows, cols];
    }

    public Matrix2D(int[,] toSet)
    {
        matrix = toSet;
    }

    public int NumberOfRows()
    {
        return matrix.GetLength(0);
    }

    public int NumberOfColumns()
    {
        return matrix.GetLength(1);
    }

    public string OutputMatrix()
    {
        string toOut = "";
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                toOut += matrix[i, j].ToString();
                if (j < matrix.GetLength(1) - 1) toOut += ",";
            }
            toOut += "\n";
        }
        return toOut;
    }

    // Works for any size if dimensions allow: (m x n) * (n x p) = (m x p)
    public static Matrix2D Multiply(Matrix2D MatrixA, Matrix2D MatrixB)
    {
        int aRows = MatrixA.matrix.GetLength(0);
        int aCols = MatrixA.matrix.GetLength(1);

        int bRows = MatrixB.matrix.GetLength(0);
        int bCols = MatrixB.matrix.GetLength(1);

        if (aCols != bRows)
            return new Matrix2D(); // invalid -> empty 0x0

        Matrix2D result = new Matrix2D(aRows, bCols);

        for (int i = 0; i < aRows; i++)
        {
            for (int j = 0; j < bCols; j++)
            {
                int sum = 0;
                for (int k = 0; k < aCols; k++)
                {
                    sum += MatrixA.matrix[i, k] * MatrixB.matrix[k, j];
                }
                result.matrix[i, j] = sum;
            }
        }

        return result;
    }
}