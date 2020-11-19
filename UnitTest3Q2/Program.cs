using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest3Q2
{
    class Program
    {
        //       Red DBlue Gray LBlue Yellow Orange Purple Green
        // Red
        // DBlue
        // Gray
        // LBlue
        // Yellow
        // Orange
        // Purple
        // Greeen

        static int[,] matrix = new int[,]
        {
            {-1, 1, 5, -1, -1, -1, -1, -1},
            {-1, -1, -1, 1, 8, -1, -1, -1},
            {-1, -1, -1, 0, -1, 1, -1, -1},
            {-1, 1, 0, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, 6},
            {-1, -1, -1, -1, -1, -1, 1, -1},
            {-1, -1, -1, -1, 1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1}
        };

        static (int, int)[][] list = new (int, int)[][]
        {
            new (int, int)[] {(1, 1), (2, 5)},
            new (int, int)[] {(3, 1), (4, 8)},
            new (int, int)[] {(3, 0)},
            new (int, int)[] {(1, 1), (2, 0)},
            new (int, int)[] {(7, 6)},
            new (int, int)[] {(6, 1)},
            new (int, int)[] {(4, 1)},
            null
        };
        static void Main(string[] args)
        {
        }
    }
}
