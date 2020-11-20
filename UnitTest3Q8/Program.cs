using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicSquares
{
    // Adjacency Matrix (19,683 x 19,683)  (512 x 512) 

    static class Program
    {
        const int MAX_SPACES = 16;
        const int MAX_WINSTATES = 86;

        // Adjacency List (strength, next moves = List<state>)
        static (int, List<int>)[] aList;

        static List<int>[] lWinStates = new List<int>[MAX_SPACES];

        static int[] strengths = new int[MAX_SPACES];
        static int[] winStates = new int[MAX_WINSTATES];

        static Random random = new Random();

        static void Main(string[] args)
        {
            bool[] grid = new bool[MAX_SPACES];

            // p1 and p2 are the integer representations of the players game boards
            // using the lowest 9 bits to indicate their chosen spaces
            // bit 8 corresponds to the top left space of the game board
            // bit 7 corresponds to the top center space
            // ...
            // bit 0 (the least significant bit) corresponds to the bottom right space
            int p1 = 0;
            int p2 = 0;

            int nWinner = 0;
            int nPlayer = 1;

            // represent the gameboard as a Magic Square to calculate winning states
            int[] nValues = new int[]
            {
                16, 3, 2, 13,
                5, 10, 11, 8,
                9, 6, 7, 12,
                4, 15, 14, 1
            };

            int i;

            int nMagicNumber = 34;

            for (i = 0; i < MAX_SPACES; ++i)
            {
                lWinStates[i] = new List<int>();
            }

            int wCntr = 0;

            for (int nState = 0; nState < Math.Pow(2, MAX_SPACES); ++nState)
            {
                bool[] bState;
                int i1;
                (bState, i1, i1) = IntToGrid(nState);

                int nTaken = 0;
                int nSum = 0;
                for (i = 0; i < MAX_SPACES; ++i)
                {
                    if (bState[i])
                    {
                        nTaken++;
                        nSum += nValues[i];
                    }
                }

                if (nTaken == Math.Sqrt(MAX_SPACES) && nSum == nMagicNumber)
                {
                    for (i = 0; i < MAX_SPACES; ++i)
                    {
                        if (bState[i])
                        {
                            lWinStates[i].Add(nState);
                        }
                    }

                    winStates[wCntr] = nState;
                    ++wCntr;
                }
            }


            for (int j = 0; j < 100; ++j)
            {
                nPlayer = 1;
                nWinner = 0;

                p1 = 0;
                p2 = 0;

                bool bFirst = false;

                int nPlayers = 0;
                do
                {
                    Console.Write("How many human players (0-1): ");
                } while (!int.TryParse(Console.ReadLine(), out nPlayers));

                if (nPlayers == 1)
                {
                    Console.Write("Do you want to go first?: ");

                    if (Console.ReadLine().ToLower().StartsWith("n"))
                    {
                        nPlayer = 2;
                        bFirst = true;
                    }
                    else
                    {
                        PrintBoard(p1, p2, nValues);
                    }
                }
                else
                {
                    bFirst = true;
                }


                while (nWinner == 0 && ((p1 | p2) != Math.Pow(2, MAX_SPACES) - 1))
                {
                    if (nPlayer == 1)
                    {
                        if (nPlayers == 1)
                        {
                            int nMove = 0;
                            do
                            {
                                Console.Write("Player 1 Move (Enter the number you want): ");
                            } while (!int.TryParse(Console.ReadLine(), out nMove));

                            // get the index of the input and use that as the move
                            nMove = Array.IndexOf(nValues, nMove) + 1;

                            p1 |= 1 << (MAX_SPACES - nMove);
                        }
                        else
                        {
                            if (!bFirst)
                            {
                                SetNextMove(ref p1, p2);
                            }
                            else
                            {
                                p1 = 1 << random.Next(0, MAX_SPACES);
                            }
                        }

                        nPlayer = 2;
                        bFirst = false;
                    }
                    else
                    {
                        if (!bFirst)
                        {
                            SetNextMove(ref p2, p1);
                        }
                        else
                        {
                            p2 = 1 << random.Next(0, MAX_SPACES);
                        }

                        nPlayer = 1;
                        bFirst = false;
                    }

                    PrintBoard(p1, p2, nValues);

                    nWinner = Winner(p1, p2);
                }

                switch (nWinner)
                {
                    case 0:
                        Console.WriteLine("Cat's game.  Meow!");
                        break;
                    case 1:
                        Console.WriteLine("Player 1 wins!");
                        break;
                    case 2:
                        Console.WriteLine("Player 2 wins!");
                        break;
                }

                Console.WriteLine($"--------- Game: {j} ----------------");

            }
        }

        static int Winner(int p1, int p2)
        {
            int nWinner = 0;
            int i;

            for (i = 0; i < winStates.Length; ++i)
            {
                if ((p1 & winStates[i]) == winStates[i])
                {
                    nWinner = 1;
                }
                else if ((p2 & winStates[i]) == winStates[i])
                {
                    nWinner = 2;
                }
            }

            return (nWinner);
        }


        static void PrintBoard(int p1, int p2, int[] nValues)
        {
            bool[] p1G;
            bool[] p2G;
            int nCnt;
            int strength;

            (p1G, nCnt, strength) = IntToGrid(p1);
            (p2G, nCnt, strength) = IntToGrid(p2);

            for (int i = 0; i < MAX_SPACES; ++i)
            {
                if (p1G[i])
                {
                    // set color to green for player (or first ai in a 0 player game)
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else if (p2G[i])
                {
                    // set color to red for ai (or second ai in a 0 player game)
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                }

                // spacing would be weird with some single digit numbers and some double digits
                // so this adds a 0 in front of single digit numbers
                if (nValues[i] < 10)
                {
                    Console.Write(" 0" + nValues[i] + " ");
                }
                else
                {
                    Console.Write(" " + nValues[i] + " ");
                }
                Console.ResetColor();

                if ((i + 1) % Math.Sqrt(MAX_SPACES) == 0)
                {
                    Console.WriteLine();
                }
            }

            Console.WriteLine("-------------------------");
        }


        static void SetNextMove(ref int a, int b)
        {
            int aMove = 0;
            int bMove = 0;
            int aStrength = 0;
            int bStrength = 0;

            CalculateGraph(a);
            // get best move for player b
            (bMove, bStrength) = GetNextMove(b, a);

            CalculateGraph(b);
            // get best move for player a
            (aMove, aStrength) = GetNextMove(a, b);

            // if the first move for a and b already moved
            if (a == 0 && b != 0)
            {
                // counter attack b
                bStrength = aStrength + 1;
            }

            // if b has a better move, then block
            if (bStrength > aStrength)
            {
                a |= (bMove ^ b);
            }
            else
            {
                a = aMove;
            }
        }

        // return the move and the strength of the move
        static (int, int) GetNextMove(int a, int b)
        {
            int nStrength = 0;
            int moveBit = 0;
            int nMove = 0;

            foreach (int n in aList[a].Item2)
            {
                // fetch only the new bit in the next move
                moveBit = (n ^ a);

                // ensure that spot is available
                if ((moveBit & b) == 0)
                {
                    nStrength = aList[n].Item1;
                    nMove = n;
                    break;
                }
            }

            return (nMove, nStrength);
        }


        private static void CalculateGraph(int a)
        {
            int i;
            int j;
            int nCnt;
            bool[] grid = new bool[MAX_SPACES];
            int strength = 0;

            aList = new (int, List<int>)[65536];

            Dictionary<int, bool> dWinStates = new Dictionary<int, bool>();

            for (i = 0; i < MAX_WINSTATES; ++i)
            {
                if (((winStates[i] ^ a) & winStates[i]) == winStates[i])
                {
                    dWinStates[winStates[i]] = true;
                }
                else
                {
                    dWinStates[winStates[i]] = false;
                }
            }

            for (i = 0; i < MAX_SPACES; ++i)
            {
                strengths[i] = 0;

                for (j = 0; j < lWinStates[i].Count; ++j)
                {
                    if (dWinStates[lWinStates[i][j]])
                    {
                        ++strengths[i];
                    }
                }
            }

            // populate all possible board states for 1 player
            // there are a theoretical 2^9 possible states
            // but a smaller practical limit
            for (i = 0; i < Math.Pow(2, MAX_SPACES); ++i)
            {
                // (Item1, Item2, Item3)
                (grid, nCnt, strength) = IntToGrid(i, dWinStates);

                if (nCnt <= (MAX_SPACES / 2) + 1)
                {
                    // aList[0].Item1 = strength
                    // aList[0].Item2 = List<int> (weighted list of neighbors)
                    aList[i] = (strength, new List<int>());
                }
                else
                {
                    aList[i] = (0, null);
                }
            }

            for (i = 0; i < aList.Length; ++i)
            {
                if (aList[i].Item1 == 0)
                {
                    continue;
                }

                (grid, nCnt, strength) = IntToGrid(i);

                for (int g = 0; g < MAX_SPACES; ++g)
                {
                    bool[] neighbor = new bool[MAX_SPACES];
                    Array.Copy(grid, neighbor, MAX_SPACES);

                    if (!neighbor[g])
                    {
                        neighbor[g] = true;
                        aList[i].Item2.Add(GridToInt(neighbor));
                    }
                }

                // sort the neighbors by their strengths in descending order
                aList[i].Item2.Sort(
                    // the following 4 expressions are equivalent
                    delegate (int m, int n)
                    {
                        return aList[n].Item1.CompareTo(aList[m].Item1);
                    });

                //(int m, int n) =>
                //{
                //    return aList[n].Item1.CompareTo(aList[m].Item1);
                //});

                //(m,n) =>
                //{
                //    return aList[n].Item1.CompareTo(aList[m].Item1);
                //});

                // (m, n) => aList[n].Item1.CompareTo(aList[m].Item1));
            }
        }

        public static int GridToInt(bool[] g)
        {
            int r = 0;

            for (int i = 0; i < MAX_SPACES; ++i)
            {
                if (g[i])
                {
                    r += (1 << ((MAX_SPACES - 1) - i));
                }
            }

            return (r);
        }

        // convert current state c to (boolean grid, move count, strength)
        public static (bool[], int, int) IntToGrid(int c, Dictionary<int, bool> dWinStates = null)
        {
            bool[] bCell = new bool[MAX_SPACES];
            int nCnt = 0;
            int nMaxStrength = 1;
            int i = 0;
            int j = 0;

            // check for a winning move
            for (i = 0; i < winStates.Length; ++i)
            {
                if ((dWinStates != null) && !dWinStates[winStates[i]])
                {
                    continue;
                }

                // winning move in this state
                if ((c & winStates[i]) == winStates[i])
                {
                    // we want this move!
                    nMaxStrength = 1000;
                }
            }

            for (i = 0; i < MAX_SPACES && nCnt <= (MAX_SPACES / 2) + 1; ++i)
            {
                if (((1 << i) & c) != 0)
                {
                    if (nMaxStrength < 1000)
                    {
                        nMaxStrength += strengths[(MAX_SPACES - 1) - i];
                    }

                    bCell[(MAX_SPACES - 1) - i] = true;
                    ++nCnt;
                }
                else
                {
                    bCell[(MAX_SPACES - 1) - i] = false;
                }
            }

            return (bCell, nCnt, nMaxStrength);
        }
    }
}
