using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HelloWorld1
{
    public class MathGame
    {
        Random rng = new Random();
        bool exitSheet, exitGame, allowNeg;
        string problemType;
        int difficulty;

        public MathGame()
        {
            Console.Clear();
            allowNeg = false;
            exitGame = false;
            problemType = "+";
            difficulty = 1;

            while (!exitGame)
            {
                openMenu();
            }
        }

        private void openMenu()
        {
            exitSheet = false;
            Console.WriteLine($"\n{ getConfig() }\n"); // print current settings
            Console.WriteLine("START\t Begin game\nOPTIONS\t Open settings \nEXIT\t Quit the game \n"); //controls
            string input = Console.ReadLine().Trim().ToLower();
            if (input == "exit" || input == "quit")
            {
                exitGame = true;
                return;
            }
            else if (input == "options" || input == "option" || input == "settings" || input == "setting")
            {
                openOptions();
                Console.Clear();
                return;
            }
            else if (input == "start" || input == "go")
            {
                Console.Clear();
                Console.WriteLine("");
                // start game. One sheet = 10 questions
                problemSheet(problemType, difficulty);
            }
            else
            {
                Console.Clear();
            }
        }

        private string getConfig()
        {
            string res = "Practice ";
            switch (problemType)
            {
                case "+": res += "addition problems "; break;
                case "-": res += "subtration problems "; break;
                case "*": res += "multiplication problems "; break;
                case "/": res += "division problems "; break;
                case "m": res += "mixed problems "; break;
                default:  res += $"the {problemType} times table."; return res;
            }
            res += $"on difficulty {difficulty}, ";
            if (allowNeg) { res += "with negative numbers."; }
            else { res += "without negative numbers."; }

            return res;
        }

        private void openOptions()
        {
            string input;
            Console.Clear();
            Console.WriteLine("OPTIONS\n");

            Console.WriteLine("Change problem type");
            Console.WriteLine("\t+   : Addition");
            Console.WriteLine("\t-   : Subtration");
            Console.WriteLine("\t*   : Multiplication");
            Console.WriteLine("\t/   : Division");
            Console.WriteLine("\t2-9 : Times table");
            Console.WriteLine("\tm   : Mixed tasks");
            string[] allowedTypes = { "+", "-", "*", "/", "2", "3", "4", "5", "6", "7", "8", "9", "m" };

            input = Console.ReadLine().Trim().ToLower();
            if (input == "exit")
            {
                return;
            }
            else if (allowedTypes.Contains(input))
            {
                problemType = input;
                if (int.TryParse(input, out int num))
                {
                    // if type is a table, don't ask for difficulty or allow negatives 
                    return;
                }
            }
            else
            {
                Console.Write("Invalid input. Press ENTER to try again");
                Console.ReadLine();
                openOptions();
                return;
            }
            
            Console.Write("Change difficulty (1-4): ");
            input = Console.ReadLine();
            if(int.TryParse(input, out int dif))
            {
                if (dif < 1) { difficulty = 1; }
                else if (dif > 4) { difficulty = 4; }
                else { difficulty = dif; }
            }

            Console.Write("Allow for negative numbers? (yes/no): ");
            input = Console.ReadLine().Trim().ToLower();
            allowNeg = (input.Substring(0, 1) == "y");
        }

        private void problemSheet(string type, int diff)
        {
            int size = 10;
            int score = 0;
            bool isTable = int.TryParse(type, out int num);

            for (int i = 0; i < size; ++i)
            {
                // loop problem() 10 times, generating 10 questions 
                // each problem function returns the score as an int
                if (isTable)
                {
                    // table problems
                    score += tableProblem(num, i+1);
                }
                else
                {
                    // other problems ( + - * / mixed )
                    score += problem(type, diff);
                }

                if (exitSheet)
                {
                    // return to menu (start/options/exit)
                    Console.Clear();
                    return;
                }
                Console.WriteLine("");
            }
            Console.WriteLine($"\nDone. Your score is {score}");
            try
            {
                Highscore hs = new Highscore();
                if (hs.inTop10(score))
                {
                    Console.Write("Enter your name: ");
                    string name = Console.ReadLine();
                    if (name.Length > 8) { name = name.Substring(0, 8); }
                    hs.writeToFile(name, score);
                }
                hs.printHighscore();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to locate/open highscore file. \n");
            }
            Console.WriteLine("Press ENTER to continue");
            Console.ReadLine();
            Console.Clear();
        }

        private float randomNumber(int diff, bool allowNegative)
        {
            if (allowNegative)
            {
                switch (diff)
                {
                    case 0: return (float)this.rng.Next(-10, 10);
                    case 1: return (float)this.rng.Next(-10, 10);
                    case 2: return (float)this.rng.Next(-100, 100);
                    case 3: return (float)this.rng.Next(-1000, 1000);
                    case 4: return (float)this.rng.Next(-9999, 9999);
                    default: return 1;
                }
            }
            else
            {
                switch (diff)
                {
                    case 0: return (float)this.rng.Next(1, 10);
                    case 1: return (float)this.rng.Next(1, 10);
                    case 2: return (float)this.rng.Next(1, 100);
                    case 3: return (float)this.rng.Next(1, 1000);
                    case 4: return (float)this.rng.Next(1, 9999);
                    default: return 1;
                }
            }
        }

        private int problem(string type, int diff)
        {
            float a, b, ans;
            int tries = 0;
            string input;
            string question = "";
            string answer = "";
            
            a = randomNumber(diff,allowNeg);
            b = randomNumber(diff,false); // second number always positive. Cannot have two operators side by side
            ans = randomNumber(diff-1,allowNeg); // used for division case

            if (type == "m")
            {
                // random problem type selected
                string[] types = { "+", "-", "*", "/" };
                type = types[rng.Next(4)];
            }

            switch (type)
            {
                case "+": ans = a + b;
                    question = $" {a} + {b} = ";
                    answer = $" {a} + {b} = {ans} ";
                    break;
                case "-": ans = a - b;
                    question = $" {a} - {b} = ";
                    answer = $" {a} - {b} = {ans} ";
                    break;
                case "*":
                    ans = a * b;
                    question = $" {a} * {b} = ";
                    answer = $" {a} * {b} = {ans} ";
                    break;
                case "/":
                    a = ans * b; // ensures answer is an int
                    question = $" {a} / {b} = ";
                    answer = $" {a} / {b} = {ans} ";
                    break;
                default: break;
            }

            while (tries < 3)
            {
                Console.Write(question);
                input = Console.ReadLine().Trim();
                if (input.ToLower() == "exit")
                {
                    this.exitSheet = true;
                    return 0;
                }
                else if (input == ans.ToString())
                {
                    return score(diff,tries);
                }
                else
                {
                    Console.WriteLine(" Incorrect");
                    ++tries;
                }
            }
            Console.WriteLine($" No more tries ({answer})");
            return 0;
        }

        private int tableProblem(int table, int num)
        {
            string input;
            float ans = table * num;
            string question = $" {table} * {num} = ";
            string answer = $" {table} * {num} = {ans} ";
            int tries = 0;

            while (tries < 3)
            {
                Console.Write(question);
                input = Console.ReadLine().Trim();
                if (input.ToLower() == "exit")
                {
                    exitSheet = true;
                    return 0;
                }
                else if (input == ans.ToString())
                {
                    return score(1, tries);
                }
                else
                {
                    Console.WriteLine(" Incorrect");
                    ++tries;
                }
            }
            Console.WriteLine($" No more tries ({answer})");
            return 0;
        }

        private int score(int diff, int tries)
        {
            return (3 * diff) - tries;
        }
    }

    public class Highscore
    {
        string filename = "mathgame_highscore.txt";
        List<Score> scoreList;
        int top, low;

        public Highscore()
        {
            loadFile();
        }
        private void loadFile()
        {
            scoreList = new List<Score>();
            // read from file
            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // save all data from file as Score structs and added to list
                    Score s = new Score(line.Split(';')[0], int.Parse(line.Split(';')[1]));
                    scoreList.Add(s);
                }
                reader.Close();
            }
            // sort scores and remove all but top 10
            scoreList.Sort((s2, s1) => s1.value.CompareTo(s2.value));
            scoreList.RemoveRange(10, scoreList.Count-10);
            top = scoreList[0].value;
            low = scoreList[9].value;
        }
        public void printHighscore()
        {
            int i = 1;
            Console.WriteLine("\n----- Highscore -----");
            foreach(Score s in scoreList)
            {
                Console.WriteLine($"{i.ToString().PadLeft(3)} | {s.name.PadLeft(8)} | {s.value.ToString().PadLeft(3)}");
                ++i;
            }
            Console.WriteLine("");
        }
        
        public bool inTop10(int score)
        {
            // check if input is within top 10
            return score > low;
        }

        public void writeToFile(string name, int score)
        {
            using (StreamWriter writer = new StreamWriter(filename, true))
            {
                writer.WriteLine($"{name};{score}");
                writer.Close();
            }
            // reload file to ensure new score is displayed
            loadFile();
        }

        public struct Score
        {
            // glorified pair of username and score
            public string name { get; private set; }
            public int value { get; private set; }
            public Score(string n, int v)
            {
                name = n;
                value = v;
            }
        }
    }
}