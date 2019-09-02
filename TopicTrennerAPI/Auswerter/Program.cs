using System;

namespace Auswerter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("File converter");

            int counter = 0;
            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(@"c:\run\test.txt");
            while ((line = file.ReadLine()) != null)
            {
                if(line.Length > 26 && line[0] == 'I' && line[1] == 'd' && line[2] == ':')
                {
                    // System.Console.WriteLine(line);
                    System.Console.WriteLine(toCsv(line));
                    counter++;
                }
            }

            file.Close();
            System.Console.WriteLine("There were {0} lines.", counter);
            // Suspend the screen.  
            System.Console.ReadLine();
        }
        static string toCsv(string line)
        {
            string[] spilt = line.Split(" ");

            string id = spilt[0].Split(":")[1];
            string ticks = spilt[1].Split(":")[1];
            string received = spilt[2].Split(":")[1];

            string diffTicks = "error";
            string diffMilSec = "error";

            if (long.TryParse(ticks, out long ticksL)
                && long.TryParse(received, out long receivedL))
            {
                long diffTicksL = ticksL - receivedL;
                diffTicks = "" + diffTicksL;
                diffMilSec = "" + (diffTicksL / 10000);
            }

            return id +";"+ ticks+";"+ received + ";" + diffTicks + ";" +diffMilSec;
        }
    }
}
