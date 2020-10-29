using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dont_Die_Part_1
{
    class Program
    {
        enum D
        {
            north, south, east, west, none
        }

        static D n = D.north;
        static D s = D.south;
        static D e = D.east;
        static D w = D.west;
        static D x = D.none;

        //  A B C D E F G H
        //A
        //B
        //C
        //D
        //E
        //F
        //G
        //H

        // neighboring points can be determined by if (maxtrixGraph[x, y].Item1 == x)
        // relative direction is Item1 in the tuple
        // cost is Item2 in the tuple
        static (D, int)[,] matrixGraph = new (D, int)[,]
        {
            {(n, 0), (s, 2), (x, -1), (x, -1), (x, -1), (x, -1), (x, -1), (x, -1)},
            {(x, -1), (x, -1), (s, 2), (e, 3), (x, -1), (x, -1), (x, -1), (x, -1)},
            {(x, -1), (n, 2), (x, -1), (x, -1), (x, -1), (x, -1), (x, -1), (s, 20)},
            {(x, -1), (w, 3), (s, 5), (x, -1), (n, 2), (e, 4), (x, -1), (x, -1)},
            {(x, -1), (x, -1), (x, -1), (x, -1), (x, -1), (s, 3), (x, -1), (x, -1)},
            {(x, -1), (x, -1), (x, -1), (x, -1), (x, -1), (x, -1), (e, 1), (x, -1)},
            {(x, -1), (x, -1), (x, -1), (x, -1), (n, 0), (x, -1), (x, -1), (s, 2)},
            {(x, -1), (x, -1), (x, -1), (x, -1), (x, -1), (x, -1), (x, -1), (x, -1)}
        };

        //Item1 is the index of the neighbor, Item2 is the direction and Item3 is the cost
        static (int, D, int)[][] listGraph = new (int, D, int)[][]
        {
            new (int, D, int)[] {(0, n, 0), (1, s, 2)},
            new (int, D, int)[] {(2, s, 2), (3, e, 3)},
            new (int, D, int)[] {(1, n, 2), (7, s, 20)},
            new (int, D, int)[] {(1, w, 3), (2, s, 5), (4, n, 2), (5, e, 4)},
            new (int, D, int)[] {(5, s, 3)},
            new (int, D, int)[] {(6, e, 1)},
            new (int, D, int)[] {(4, n, 0)},
            null
        };
        static void Main(string[] args)
        {
            
        }
    }
}
