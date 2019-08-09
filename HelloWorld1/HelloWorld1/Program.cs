using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HelloWorld1
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                selectTask();
            }
        }

        static void selectTask()
        {
            // print menu with avaiable taks to run 
            string[] tasks = {
                "Intro",
                "Are you old?",
                "Times tables",
                "Dice roller",
                "Simple calculator",
                "Frog jumper",
                "Array rotating",
                "Find odd-one-out",
                "Math game"
            };

            for (int i = 0; i < tasks.Length; ++i)
            {
                Console.WriteLine($"[{i}] {tasks[i]}");
            }
            Console.Write($"\nSelect a task (0-{tasks.Length - 1}): ");
            string select = Console.ReadLine().Trim().ToLower();

            Console.Clear();
            switch (select)
            {
                case "0": helloWorld(); break;
                case "1": userInfo(); break;
                case "2": timesTables(); break;
                case "3": allSixes(); break;
                case "4": Calculator cal = new Calculator(); break;
                case "5": frogJumpDialog(); break;
                case "6": rotateArrayDialog(); break;
                case "7": findSingleDialog(); break;
                case "8": MathGame game = new MathGame(); break;

                case "info": Console.WriteLine("\n Fundamental programming // H1 SDE \n Anne-Sofie Qvist Fisker // anne574d \n August 2019"); Console.ReadLine(); break;
                case "sort": sortingDialog(); break;
                case "stars": starryNight(); break;
                case "exit": Environment.Exit(0); break;

                default: break;
            }
            Console.Clear();
        }
        static void sortingDialog()
        {
            int[] array = new int[] { 1, 100, 62, 54, 3, 12, 84, 77, 62, 0, -3, -88, 91, 98, -132, 4 };
            Console.WriteLine($"Input array: \n[{string.Join(", ", array)}]\n");

            int[] swappedAscend = swapSort(array, true);
            Console.WriteLine($"Swap sorted array (ascending): \n[{string.Join(", ", swappedAscend)}]\n");

            int[] swappedDescend = swapSort(array, false);
            Console.WriteLine($"Swap sorted array (descending): \n[{string.Join(", ", swappedDescend)}]\n");

            int[] bubbledAscend = bubbleSort(array, true);
            Console.WriteLine($"Bubble sorted array (ascending): \n[{string.Join(", ", bubbledAscend)}]\n");

            int[] bubbledDescend = bubbleSort(array, false);
            Console.WriteLine($"Bubble sorted array (descending): \n[{string.Join(", ", bubbledDescend)}]\n");

            Console.ReadLine();
        }

        static int[] bubbleSort(int[] a, bool ascending)
        {
            int ext; // extremus (min or max)

            for (int i = 0; i < a.Length; ++i)
            {
                ext = a[i];
                for(int j = i + 1; j < a.Length; ++j)
                {
                    if ( (ascending && ext > a[j]) || 
                        (!ascending && ext < a[j]))
                    {
                        // swap 
                        a[i] = a[j];
                        a[j] = ext; 
                        ext = a[i];
                    }
                }
            }
            return a;
        }

        static int[] swapSort(int[] a, bool ascending)
        {
            bool swapOccured;
            int temp;

            do
            {
                swapOccured = false;
                for (int i = 1; i < a.Length; ++i)
                {
                    if ((ascending && (a[i] < a[i - 1])) ||
                        (!ascending && (a[i] > a[i - 1])))
                    {
                        // swap
                        temp = a[i];
                        a[i] = a[i - 1];
                        a[i - 1] = temp;
                        swapOccured = true;
                    }
                }
            } while (swapOccured);

            return a;
        }

        static void findSingleDialog()
        {
            // create very big array with pairs and one single
            int[] array = new int[99999]; // must be odd length
            array[0] = array.Length + 1234; // the single
            for (int i = 1; i < array.Length; i+=2)
            {
                // add pairs of numbers to array
                array[i] = i;
                array[i + 1] = i;
            }
            // shuffle array
            Random rnd = new Random(); 
            array = array.OrderBy(x => rnd.Next()).ToArray();

            //Console.WriteLine($"Input array: [{string.Join(", ", array)}]\n"); // takes too long to print
            Console.WriteLine($"Input array length: {array.Length}\n");

            // run the swapping function with a timer 
            Stopwatch timer = Stopwatch.StartNew();
            List<int> singles = findSingleSwap(array);
            Console.WriteLine($"Singles found via swap function: {string.Join(", ", singles)}");
            Console.WriteLine($"Run time: {timer.ElapsedMilliseconds} ms\n");

            array = array.OrderBy(x => rnd.Next()).ToArray(); // shuffle array again

            if (singles.Count == 1)
            {
                // run the XOR function with timer
                timer.Restart();
                int single = findSingleXOR(array);
                Console.WriteLine($"Single found via XOR function: {single}");
                Console.WriteLine($"Run time: {timer.ElapsedMilliseconds} ms");
            }
            else
            {
                Console.WriteLine("Cannot use XOR method on arrays with more than one unpaired element");
                // show how XORing multiple singles becomes nonsense
                Console.WriteLine($"{string.Join(" XOR ", singles)} = {findSingleXOR(array)}");  
            }
            Console.ReadLine();
        }

        static int findSingleXOR(int[] array)
        {
            int res = array[0];
            for (int i = 1; i < array.Length; ++i)
            {
                res ^= array[i];
            }
            return res;
        }

        static List<int> findSingleSwap(int[] array)
        {
            int temp;
            List<int> res = new List<int>();
            bool pairFound; 

            for (int i = 0; i < array.Length; ++i)
            {
                pairFound = false;
                for (int j = i+1; j < array.Length; ++j) 
                {
                    // check for match on all indexes to the right of current index
                    if (array[i] == array[j])
                    {
                        //swap if match found
                        temp = array[i + 1];
                        array[i + 1] = array[j];
                        array[j] = temp;

                        ++i; // double iteration to skip newly formed pair
                        pairFound = true;
                        break;
                    }
                }
                if (!pairFound)
                {
                    res.Add(array[i]);
                }
            }
            return res;
        }

        static void rotateArrayDialog()
        {
            int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            //int[] array = { 3, 8, 9, 7, 6 };
            Console.WriteLine($"Original array: [{string.Join(", ", array)}]");

            Random rng = new Random();
            int k = rng.Next(1,17);
            Console.WriteLine($"Rotating array {k} times");

            int[] newArray = rotateArray(array, k);
            Console.WriteLine($"Rotated array:  [{string.Join(", ", newArray)}]");

            Console.ReadLine();
        }
        static int[] rotateArray(int[] a, int k)
        {
            int last = a.Length - 1;
            int temp;

            while (k > 0)
            {
                temp = a[last]; // save last element
                for (int i = last; i > 0; --i)
                {
                    a[i] = a[i - 1]; 
                }
                a[0] = temp;
                --k;
            }
            return a;
        }

        static void frogJumpDialog() // opgave 5
        {
            int jumplength, start, end, jumps;
            string input;

            do
            {
                Console.Write("Choose the frog's starting position: ");
                input = Console.ReadLine();
            } while (!int.TryParse(input, out start));

            do
            {
                Console.Write("Choose the frog's final position: ");
                input = Console.ReadLine();
            } while (!int.TryParse(input, out end));

            do
            {
                Console.Write("Choose the frog's jumping length: ");
                input = Console.ReadLine();
            } while (!int.TryParse(input, out jumplength));

            try
            {
                jumps = frogJump(start, end, jumplength);
                Console.WriteLine($"The frog arrives after {jumps} jumps. ");
                Console.WriteLine("      _ \n    .'_`--.___   __ \n   ( 'o`   - .`.'_ )\n     `-._      `_`./_ \n       '/\\    ( .'/ ) \n      ,__//`---'`-'_/ \n      /-'        '/ \n                 ' ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
            Console.ReadLine();
        }
        static int frogJump(int x, int y, int d) // opgave 5
        {
            if (x < 1 || y < 1 || y < x || d < 1 || x > 1000000000 || y > 1000000000)
            {
                throw new Exception("Invalid input");
            }
            else
            {
                return (int)Math.Ceiling((float)(y - x) / (float)d);
            }
        }

        static void allSixes() //opgave 3
        {
            int dice, tries = 0, diceSum;
            string input;
            Random rnd = new Random();

            do
            {
                // with 11 or more dice, run time becomes too long
                Console.Write("Choose the number of dice you wanna roll (1-10): ");
                input = Console.ReadLine();
            }
            while (!int.TryParse(input, out dice) || dice > 10 || dice < 1);
            
            Stopwatch timer = Stopwatch.StartNew();
            do
            {
                // roll dice until they all land on six
                ++tries;
                diceSum = 0;
                for (int i = 0; i < dice; ++i)
                {
                    diceSum += rnd.Next(1, 7);
                }
            } while (diceSum != dice * 6);

            Console.WriteLine($"You rolled {dice} sixes in {tries} tries. \nRun time: {timer.ElapsedMilliseconds} ms");
            Console.ReadLine();
        }

        static void timesTables() // opgave 2
        {
            string result = "";
            for (int i = 1; i < 11; ++i) // tables 1-10
            {
                for (int j = 1; j < 11; ++j)
                {
                    result += $"{ i * j } ".PadLeft(5);
                }
                Console.WriteLine(result);
                result = "";
            }
            Console.ReadLine();
        }

        static void userInfo() // opgave 1
        {            
            // first name
            Console.Write("Please provide your first name: ");
            string firstName = Console.ReadLine().Trim();

            // last name
            Console.Write("Please provide your last name: ");
            string lastName = Console.ReadLine().Trim();

            // age
            int ageInt;
            string ageStr;
            do
            {
                Console.Write("Please provide your age: ");
                ageStr = Console.ReadLine();
            } while (!int.TryParse(ageStr, out ageInt));

            // gender
            string genderStr;
            char gender = ' ';
            do
            {
                Console.Write("Are you female or male? (f/m): ");
                genderStr = Console.ReadLine().Trim();
                if (!String.IsNullOrEmpty(genderStr))
                {
                    gender = genderStr[0];
                }
            } while (gender != 'm' && gender != 'f');
            
            if (gender == 'f') { genderStr = "woman"; } // save gender as word for printed output
            else if (gender == 'm') { genderStr = "man"; }

            // old
            int oldAgeInt;
            string oldAgeStr;
            do
            {
                Console.Write("In your opinion, how old does one need to be to be considered old? ");
                oldAgeStr = Console.ReadLine();
            } while (!int.TryParse(oldAgeStr, out oldAgeInt));

            // output
            int ageDiff = oldAgeInt - ageInt;
            if (ageDiff == 1) // single year
            {
                Console.WriteLine($"You, {firstName} {lastName}, will be considered an old {genderStr} next year ({DateTime.Now.Year + ageDiff})");
            }
            else if (ageDiff > 0)// still young
            {
                Console.WriteLine($"You, {firstName} {lastName}, will be considered an old {genderStr} in {ageDiff} years ({DateTime.Now.Year + ageDiff})");
            }
            else // old
            {
                Console.WriteLine($"Bad news, {firstName} {lastName}: You are already considered an old {genderStr} (since {DateTime.Now.Year + ageDiff})");
            }
            Console.ReadLine();
        }

        static void helloWorld() // intro
        {
            string input;
            int stars = 0;
            Random rnd = new Random();

            Console.Write("What's your name, user? ");
            input = Console.ReadLine();
            Console.Write($"G'day, {input}, how many stars do you want? ");
            input = Console.ReadLine();

            if(int.TryParse(input, out stars))
            {
                for (int n = 0; n < stars; ++n)
                {
                    Console.Write("*");
                }
            }
            else
            {
                Console.WriteLine("That's not a number, ya dingus! ");
            }
            Console.ReadLine();
        }

        static void starryNight() // for fun
        {
            int space;
            Random rnd = new Random();
            for (int n = 0; n < 1000; ++n)
            {
                space = rnd.Next(8, 40);
                for (int m = 0; m < space; ++m)
                {
                    Console.Write(" ");
                }
                Console.Write("*");
            }
            Console.ReadLine();
        }
    }
}