using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = Console.Read();
            SplineV2 s = new SplineV2();
            s.solve();
        }
    }
}
