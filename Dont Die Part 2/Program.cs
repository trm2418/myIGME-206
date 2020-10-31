using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Web;

using System.Timers;

namespace Dont_Die_Part_2
{
    class TriviaResult
    {
        public string category;
        public string type;
        public string difficulty;
        public string question;
        public string correct_answer;
        public List<string> incorrect_answers;
    }
    class Trivia
    {
        public int response_code;
        public List<TriviaResult> results;
    }
    class Program
    {
        public static Random rnd = new Random();

        static bool bTimeOut = false;
        static Timer timeOutTimer;

        // Direction enum, D for short
        public enum D
        {
            north, south, east, west, none
        }

        // shortened variable names to make the graph and list easier
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

        // keeps track of the players current room and health
        public class Player
        {
            public static int roomIndex = 0;
            public static int health = 1;
        }

        // gets the name and event in each room from its index
        public static Dictionary<int, string[]> roomDict = new Dictionary<int, string[]>()
        {
            {0, new string[] {"Maze Entrance"} }, {1, new string[] {"Cave", "A bear sat on you!"} },
            {2, new string[] {"Waterfall", "You tripped and fell in the water and then got bit by a megalodon shark!" } }, {3, new string[] {"Jungle", "You got dropkicked by a tiger!" } },
            {4, new string[] {"Ice Cave", "An ice dragon punched you in the face!" } }, {5, new string[] {"Volcano", "A fire elemental burned your hair off!" } },
            {6, new string[] {"River", "A beaver threw a log at you!"} }, {7, new string[] {"Maze Exit"} }
        };

        static void Main(string[] args)
        {
            // loop until you finish or die
            while (Player.roomIndex != 7 && Player.health > 0)
            {
                bTimeOut = false;

                // store valid input choices
                string[] validInputs = new string[4];

                // print current room and your health
                Console.WriteLine("You are in the " + roomDict[Player.roomIndex][0] + ". You have " + Player.health + " health");

                // loop through each exit in the current room
                foreach ((int, D, int) room in listGraph[Player.roomIndex])
                {
                    // if the room is north and you have enough health to go there
                    if (room.Item2 == n && Player.health > room.Item3)
                    {
                        // write N for north and the cost to exit there
                        Console.Write("N: " + room.Item3);

                        // add "n" for north to the validInputs array
                        validInputs[0] = "n";
                    }
                    // else if the room is east and you have enough health to go there
                    else if (room.Item2 == e && Player.health > room.Item3)
                    {
                        Console.Write("E: " + room.Item3);
                        validInputs[1] = "e";
                    }
                    // else if the room is south and you have enough health to go there
                    else if (room.Item2 == s && Player.health > room.Item3)
                    {
                        Console.Write("S: " + room.Item3);
                        validInputs[2] = "s";
                    }
                    // else if the room is west and you have enough health to go there
                    else if (room.Item2 == w && Player.health > room.Item3)
                    {
                        Console.Write("W: " + room.Item3);
                        validInputs[3] = "w";
                    }
                    
                    // add spacing between each option
                    Console.Write("   ");
                }
                // always have the option to wager health
                Console.WriteLine("   Type anything else to wager health");

                // get user input
                string input = Console.ReadLine().ToLower();

                // if the user's input is contained in the validInputs array
                if (validInputs.Contains(input))
                {
                    // initialize damage int
                    int damage = 0;

                    // if the user chose north
                    if (input == "n")
                    {
                        // find the north room
                        foreach ((int, D, int) room in listGraph[Player.roomIndex])
                        {
                            if (room.Item2 == n)
                            {
                                // set the player's room to the north room
                                Player.roomIndex = room.Item1;

                                // set the damage equal to the cost of that edge
                                damage = room.Item3;
                            }
                        }
                    }
                    // same functionality as north but for east
                    else if (input == "e")
                    {
                        foreach ((int, D, int) room in listGraph[Player.roomIndex])
                        {
                            if (room.Item2 == e)
                            {
                                Player.roomIndex = room.Item1;
                                damage = room.Item3;
                            }
                        }
                    }
                    // south
                    else if (input == "s")
                    {
                        foreach ((int, D, int) room in listGraph[Player.roomIndex])
                        {
                            if (room.Item2 == s)
                            {
                                Player.roomIndex = room.Item1;
                                damage = room.Item3;
                            }
                        }
                    }
                    // west
                    if (input == "w")
                    {
                        foreach ((int, D, int) room in listGraph[Player.roomIndex])
                        {
                            if (room.Item2 == w)
                            {
                                Player.roomIndex = room.Item1;
                                damage = room.Item3;
                            }
                        }
                    }

                    // print which room was entered
                    Console.WriteLine("You entered the " + roomDict[Player.roomIndex][0]);

                    // print event message for rooms not including A and H
                    if (Player.roomIndex != 0 && Player.roomIndex != 7)
                    {
                        // print the event message
                        Console.WriteLine(roomDict[Player.roomIndex][1]);

                        // print how much damage you took
                        Console.WriteLine("You took " + damage + " damage!");

                        // subtract the damage from the player's health
                        Player.health -= damage;
                    }
                }
                // wager health
                else
                {
                    // ask user how much health they want to wager
                    Console.WriteLine("How much health?");
                    int wager = 0;

                    // marker to return to if input is invalid
                    GetInput:
                    try
                    {
                        // parse input
                        wager = int.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        // if input isn't an integer return to marker
                        Console.WriteLine("Please enter an integer");
                        goto GetInput;
                    }
                    // if input < 1 return to marker
                    if (wager < 1)
                    {
                        Console.WriteLine("Wager needs to be for one or more health");
                        goto GetInput;
                    }
                    // if input > current health return to marker
                    else if (wager > Player.health)
                    {
                        Console.WriteLine("Wager can't be more than your current health");
                        goto GetInput;
                    }

                    // get the trivia question
                    string url = null;
                    string s = null;

                    HttpWebRequest request;
                    HttpWebResponse response;
                    StreamReader reader;

                    url = "https://opentdb.com/api.php?amount=1";

                    request = (HttpWebRequest)WebRequest.Create(url);
                    response = (HttpWebResponse)request.GetResponse();
                    reader = new StreamReader(response.GetResponseStream());
                    s = reader.ReadToEnd();
                    reader.Close();

                    Trivia trivia = JsonConvert.DeserializeObject<Trivia>(s);

                    trivia.results[0].question = HttpUtility.HtmlDecode(trivia.results[0].question);
                    trivia.results[0].correct_answer = HttpUtility.HtmlDecode(trivia.results[0].correct_answer);

                    for (int i = 0; i < trivia.results[0].incorrect_answers.Count; ++i)
                    {
                        trivia.results[0].incorrect_answers[i] = HttpUtility.HtmlDecode(trivia.results[0].incorrect_answers[i]);
                    }

                    // print trivia question
                    Console.WriteLine("\n" + trivia.results[0].question);

                    // set a new int to the total number of answers
                    int numAnswers = trivia.results[0].incorrect_answers.Count + 1;

                    // create an int List to randomize order of answers
                    List<int> indexes = new List<int>();

                    // add 0 to numAnswers-1 to the list
                    for (int i = 0; i < numAnswers; i++)
                    {
                        indexes.Add(i);
                    }

                    // initialize an int to store the random int
                    int rand = 0;

                    // loop once for each answer
                    for (int i = 0; i < numAnswers; i++)
                    {
                        // get the index to be displayed without repeating indexes
                        do
                        {
                            rand = rnd.Next(0, numAnswers);
                        } while (!indexes.Contains(rand));
                        
                        // if index is 0 display the correct answer
                        if (rand == 0)
                        {
                            Console.Write(trivia.results[0].correct_answer + "   ");
                        }
                        // else display an incorrect answer
                        else
                        {
                            Console.Write(trivia.results[0].incorrect_answers[rand - 1] + "   ");
                        }
                        // remove that index from the list
                        indexes.Remove(rand);
                    }

                    Console.WriteLine();

                    // start Timer
                    bTimeOut = false;
                    timeOutTimer = new Timer(15000);
                    ElapsedEventHandler timesUpMethod;
                    timesUpMethod = new ElapsedEventHandler(TimesUp);
                    timeOutTimer.Elapsed += timesUpMethod;
                    timeOutTimer.Start();

                    // get input
                    string userAnswer = Console.ReadLine();

                    // stop timer
                    timeOutTimer.Stop();

                    // if correct
                    if (userAnswer == trivia.results[0].correct_answer && !bTimeOut)
                    {
                        // gain health
                        Console.WriteLine("\nCorrect!");
                        Console.WriteLine("You got " + wager + " health!");
                        Player.health += wager;
                    }
                    // if incorrect
                    else if (!bTimeOut)
                    {
                        // lose health
                        Console.WriteLine("\nWrong!");
                        Console.WriteLine("You lost " + wager + " health!");
                        Player.health -= wager;

                        // if you die print that you died
                        if (Player.health <= 0)
                        {
                            Console.WriteLine("You died!");
                        }
                    }
                    // time out
                    else
                    {
                        // lose health
                        Console.WriteLine("You lost " + wager + " health!");
                        Player.health -= wager;

                        // if you die print that you died
                        if (Player.health <= 0)
                        {
                            Console.WriteLine("You died!");
                        }
                    }
                    Console.WriteLine();

                }
            }
            
        }

        static void TimesUp(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("\nYour time is up!");

            bTimeOut = true;
            timeOutTimer.Stop();
        }
    }
}
