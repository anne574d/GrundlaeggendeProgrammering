using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld1
{
    class Calculator
    {
        bool reset;
        float ans;

        public Calculator()
        {
            clear();

            while (true)
            {
                try
                {
                    string input = Console.ReadLine().Trim();
                    if (input.ToLower() == "exit")
                    {
                        break;
                    }
                    else if (input.ToLower() == "clear")
                    {
                        clear();
                    }
                    else if (!String.IsNullOrEmpty(input))
                    {
                        handleInput(input);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nSYNTAX ERROR\nPress enter to restart...");
                    Console.ReadLine();
                    clear();
                }
            }
        }

        private void clear()
        {
            Console.Clear();
            Console.WriteLine("Welcome to the calculator\n");
            reset = true;
        }

        private void handleInput(string input)
        {
            string op;
            float a, b;

            input = input.Replace(".", ",");

            if (!reset)
            {
                // continues with answer from previous calculation
                op = input.Substring(0, 1);
                a = float.Parse(input.Substring(1));
                ans = calculate(ans, a, op);
            }
            else
            {
                // first input after clear
                // needs to find operator and seperate the two numbers around it
                int opPos = findOperator(input);
                op = input.Substring(opPos, 1);
                a = float.Parse(input.Substring(0, opPos));
                b = float.Parse(input.Substring(opPos + 1));
                ans = calculate(a, b, op);
            }

            if (ans == float.PositiveInfinity || ans == float.NegativeInfinity)
            {
                throw new Exception("Divide by zero");
            }
            Console.Write(ans);
            reset = false;
        }

        private float calculate(float a, float b, string op)
        {
            switch (op)
            {
                case "+": return a + b;
                case "-": return a - b;
                case "*": return a * b;
                case "/": return a / b;
                case "^": return (float)Math.Pow(a, b);

                default: throw new Exception("Invalid operator");
            }
        }

        private int findOperator(string input)
        {
            string[] operators = { "+", "*", "/", "^", "-" };
            int pos;
            foreach (string op in operators)
            {
                pos = input.IndexOf(op,1); // start at index 1 to skip leading minus
                if (pos != -1)
                {
                    return pos;
                }
            }
            throw new Exception("Cannot find valid operator");
        }
    } 
}
