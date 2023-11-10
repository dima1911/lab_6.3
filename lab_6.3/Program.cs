﻿using System;

public class Quaternion
{
    public double W { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    // Конструктор
    public Quaternion(double w, double x, double y, double z)
    {
        W = w;
        X = x;
        Y = y;
        Z = z;
    }

    // Перевантаження операторів додавання
    public static Quaternion operator +(Quaternion q1, Quaternion q2)
    {
        return new Quaternion(q1.W + q2.W, q1.X + q2.X, q1.Y + q2.Y, q1.Z + q2.Z);
    }

    // Перевантаження операторів віднімання
    public static Quaternion operator -(Quaternion q1, Quaternion q2)
    {
        return new Quaternion(q1.W - q2.W, q1.X - q2.X, q1.Y - q2.Y, q1.Z - q2.Z);
    }

    // Перевантаження операторів множення
    public static Quaternion operator *(Quaternion q1, Quaternion q2)
    {
        double w = q1.W * q2.W - q1.X * q2.X - q1.Y * q2.Y - q1.Z * q2.Z;
        double x = q1.W * q2.X + q1.X * q2.W + q1.Y * q2.Z - q1.Z * q2.Y;
        double y = q1.W * q2.Y - q1.X * q2.Z + q1.Y * q2.W + q1.Z * q2.X;
        double z = q1.W * q2.Z + q1.X * q2.Y - q1.Y * q2.X + q1.Z * q2.W;

        return new Quaternion(w, x, y, z);
    }

    // Обчислення норми
    public double Magnitude()
    {
        return Math.Sqrt(W * W + X * X + Y * Y + Z * Z);
    }

    // Обчислення спряженого кватерніона
    public Quaternion Conjugate()
    {
        return new Quaternion(W, -X, -Y, -Z);
    }

    // Обчислення інверсного кватерніона
    public Quaternion Inverse()
    {
        double magnitudeSquared = W * W + X * X + Y * Y + Z * Z;
        if (magnitudeSquared == 0)
            throw new InvalidOperationException("Quaternion has zero magnitude, cannot compute inverse.");

        double inverseMagnitudeSquared = 1 / magnitudeSquared;
        return new Quaternion(W * inverseMagnitudeSquared, -X * inverseMagnitudeSquared, -Y * inverseMagnitudeSquared, -Z * inverseMagnitudeSquared);
    }

    // Перевантаження операторів порівняння
    public static bool operator ==(Quaternion q1, Quaternion q2)
    {
        return q1.W == q2.W && q1.X == q2.X && q1.Y == q2.Y && q1.Z == q2.Z;
    }

    public static bool operator !=(Quaternion q1, Quaternion q2)
    {
        return !(q1 == q2);
    }

    // Конвертація кватерніона в матрицю обертання
    public double[,] ToRotationMatrix()
    {
        double[,] matrix = new double[3, 3];

        matrix[0, 0] = 1 - 2 * (Y * Y + Z * Z);
        matrix[0, 1] = 2 * (X * Y - W * Z);
        matrix[0, 2] = 2 * (X * Z + W * Y);

        matrix[1, 0] = 2 * (X * Y + W * Z);
        matrix[1, 1] = 1 - 2 * (X * X + Z * Z);
        matrix[1, 2] = 2 * (Y * Z - W * X);

        matrix[2, 0] = 2 * (X * Z - W * Y);
        matrix[2, 1] = 2 * (Y * Z + W * X);
        matrix[2, 2] = 1 - 2 * (X * X + Y * Y);

        return matrix;
    }

    // Конвертація матриці обертання в кватерніон
    public static Quaternion FromRotationMatrix(double[,] matrix)
    {
        if (matrix.GetLength(0) != 3 || matrix.GetLength(1) != 3)
            throw new ArgumentException("Rotation matrix must be a 3x3 matrix.");

        double trace = matrix[0, 0] + matrix[1, 1] + matrix[2, 2];

        double w, x, y, z;

        if (trace > 0)
        {
            double s = 0.5 / Math.Sqrt(trace + 1);
            w = 0.25 / s;
            x = (matrix[2, 1] - matrix[1, 2]) * s;
            y = (matrix[0, 2] - matrix[2, 0]) * s;
            z = (matrix[1, 0] - matrix[0, 1]) * s;
        }
        else if (matrix[0, 0] > matrix[1, 1] && matrix[0, 0] > matrix[2, 2])
        {
            double s = 2 * Math.Sqrt(1 + matrix[0, 0] - matrix[1, 1] - matrix[2, 2]);
            w = (matrix[2, 1] - matrix[1, 2]) / s;
            x = 0.25 * s;
            y = (matrix[0, 1] + matrix[1, 0]) / s;
            z = (matrix[0, 2] + matrix[2, 0]) / s;
        }
        else if (matrix[1, 1] > matrix[2, 2])
        {
            double s = 2 * Math.Sqrt(1 + matrix[1, 1] - matrix[0, 0] - matrix[2, 2]);
            w = (matrix[0, 2] - matrix[2, 0]) / s;
            x = (matrix[0, 1] + matrix[1, 0]) / s;
            y = 0.25 * s;
            z = (matrix[1, 2] + matrix[2, 1]) / s;
        }
        else
        {
            double s = 2 * Math.Sqrt(1 + matrix[2, 2] - matrix[0, 0] - matrix[1, 1]);
            w = (matrix[1, 0] - matrix[0, 1]) / s;
            x = (matrix[0, 2] + matrix[2, 0]) / s;
            y = (matrix[1, 2] + matrix[2, 1]) / s;
            z = 0.25 * s;
        }

        return new Quaternion(w, x, y, z);
    }
}

class Program
{
    static void Main()
    {
        // Приклад використання
        Quaternion q1 = new Quaternion(1, 2, 3, 4);
        Quaternion q2 = new Quaternion(5, 6, 7, 8);

        // Додавання
        Quaternion sum = q1 + q2;
        Console.WriteLine($"Sum: {sum.W}, {sum.X}, {sum.Y}, {sum.Z}");

        // Віднімання
        Quaternion difference = q1 - q2;
        Console.WriteLine($"Difference: {difference.W}, {difference.X}, {difference.Y}, {difference.Z}");

        // Множення
        Quaternion product = q1 * q2;
        Console.WriteLine($"Product: {product.W}, {product.X}, {product.Y}, {product.Z}");

        // Норма
        double magnitude = q1.Magnitude();
        Console.WriteLine($"Magnitude: {magnitude}");

        // Спряжений кватерніон
        Quaternion conjugate = q1.Conjugate();
        Console.WriteLine($"Conjugate: {conjugate.W}, {conjugate.X}, {conjugate.Y}, {conjugate.Z}");

        // Інверсний кватерніон
        Quaternion inverse = q1.Inverse();
        Console.WriteLine($"Inverse: {inverse.W}, {inverse.X}, {inverse.Y}, {inverse.Z}");

        // Перевірка рівності
        Console.WriteLine($"q1 == q2: {q1 == q2}");
        Console.WriteLine($"q1 != q2: {q1 != q2}");

        // Конвертація кватерніона в матрицю обертання
        double[,] rotationMatrix = q1.ToRotationMatrix();
        Console.WriteLine("Rotation Matrix:");
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Console.Write(rotationMatrix[i, j] + " ");
            }
            Console.WriteLine();
        }

        // Конвертація матриці обертання в кватерніон
        Quaternion fromMatrix = Quaternion.FromRotationMatrix(rotationMatrix);
        Console.WriteLine($"Quaternion from Matrix: {fromMatrix.W}, {fromMatrix.X}, {fromMatrix.Y}, {fromMatrix.Z}");
    }
}
