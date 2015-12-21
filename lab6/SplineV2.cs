using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace lab6
{
    class SplineV2
    {
        double[] A_MAT, B_MAT, C_MAT, D_MAT;
        double a = 1;
        double b = 35;
        int size = 100;
        double f(double x)
        {
            return Math.Log10(x * x) * Math.Sin(x / 2) * Math.Exp(Math.Pow(x, 1.0 / 7.0));
        }
        double x_i(int i)
        {
            return a + (b - a) / (size - 1) * i;
        }
        double func(double x)
        {
            int i = (int)((x - a) / (b - a) * (size - 1));
            if (i == 0) i++;
            double xi = x_i(i);
            return A_MAT[i] + B_MAT[i] * (x - xi) + (C_MAT[i] / 2) * (x - xi) * (x - xi) + (D_MAT[i] / 6)*(x - xi) * (x - xi) * (x - xi);
        }
        double h_i(int i)
        {
            return (i > 0) ? x_i(i) - x_i(i - 1) : 0;
        }
        double[] Gause(int N, double[,] M)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = i + 1; j < N + 1; j++)
                    M[i, j] /= M[i, i];
                M[i, i] = 1;
                for (int j = 0; j < N; j++)
                    if (j != i)
                        for (int k = 0; k < N + 1; k++)
                            M[j, k] -= M[i, k] * M[j, i];
            }
            double[] A = new double[N];
            for (int i = 0; i < N; i++)
                A[i] = M[i, N];
            return A;
        }
        public void solve()
        {
            A_MAT = new double[size];
            B_MAT = new double[size];
            C_MAT = new double[size];
            D_MAT = new double[size];
            for (int i = 0; i < size; i++)
                A_MAT[i] = f(x_i(i));
            if (size >2)
            {
                double[,] SOLVE_C = new double[size - 2, size - 1];
                for (int i = 0; i < size - 2; i++)
                    for (int j = 0; j < size - 2; j++)
                        SOLVE_C[i, j] = 0;
                for (int i = 0; i < size - 2; i++)
                {
                    if (i > 0) SOLVE_C[i, i - 1] = h_i(i);
                    SOLVE_C[i, i] = 2 * (h_i(i) + h_i(i + 1));
                    if (i < size - 3) SOLVE_C[i, i + 1] = h_i(i + 1);
                    SOLVE_C[i, size - 2] = 6 * ((A_MAT[i + 2] - A_MAT[i + 1]) / h_i(i + 2) - (A_MAT[i + 1] - A_MAT[i]) / h_i(i + 1));
                }
                double[] TMP = Gause(size - 2, SOLVE_C);
                D_MAT[0] = B_MAT[0] = 0;
                for (int i = 1; i < size - 1; i++)
                    C_MAT[i] = TMP[i - 1];
                for(int i=1;i< size;i++)
                {
                    double hi = h_i(i);
                    D_MAT[i] = (C_MAT[i] - C_MAT[i - 1]) / h_i(i);
                    B_MAT[i] = (hi / 2) * C_MAT[i] - (hi * hi / 6) * D_MAT[i] + (A_MAT[i] - A_MAT[i - 1]) / hi;
                }
                int n = 300;
                double k = (b - a) / n;
                double x = a;
                using (StreamWriter sw = new StreamWriter("out.txt"))
                {

                    while (x < b)
                    {
                        sw.WriteLine("{0};{1}", x, func(x));
                        x += k;
                    }
                }
            }
        }
    }
}
