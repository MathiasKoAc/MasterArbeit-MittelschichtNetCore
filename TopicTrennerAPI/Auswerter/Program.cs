using System;
using System.IO;

namespace Auswerter
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = System.IO.Directory.GetFiles(@"D:\workspace\Master-Arbeit\Benchmark\Raspberry", "*.txt");

            foreach (string s in files)
            {
                readAndConvertFile(s);
                System.Console.WriteLine(s+" convertet");
            }
            System.Console.WriteLine("--Finished--");
            System.Console.ReadLine();
        }

        static void readAndConvertFile(string fileName)
        {
            string line;

            // Read the file and display it line by line.  
            StreamReader file =
                new StreamReader(fileName);

            var fileNameSplit = fileName.Split(".");

            StreamWriter fileOutput = new StreamWriter(fileNameSplit[0]+".csv");

            fileOutput.WriteLine("id;ticks;received;diffTicks;diffMilSec");
            while ((line = file.ReadLine()) != null)
            {
                if (line.Length > 26 && line[0] == 'I' && line[1] == 'd' && line[2] == ':')
                {
                    // System.Console.WriteLine(line);
                    fileOutput.WriteLine(toCsv(line));
                }
            }
            file.Close();
            fileOutput.Flush();
            fileOutput.Close();
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
