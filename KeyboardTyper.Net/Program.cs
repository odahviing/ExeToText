using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace KeyboardWriter
{
    class Program
    {
        static void Main(string[] args)
        {         
            Console.WriteLine("### Keyboard Writer (Version " + Ver.Major + "." + Ver.Minor + ") by Odahviing ###");
            StringBuilder Text = new StringBuilder();
            if (args.Length == 0)
            {
                Console.WriteLine("Insert Data:");
                Text.Append(Console.ReadLine());
            }
            else if (args.Length == 1)
            {
                string filePath = args[0];
                Text.Append(readFile(filePath));
            }
            
            Console.WriteLine("Pointer To Window Location (5 Sec) ... ");
            Thread.Sleep(5000);
            SendKeys.SendWait(Text.ToString());
        }


        static Version Ver = Assembly.GetExecutingAssembly().GetName().Version;


        static string readFile(string filePath)
        {
            try
            {
                string value = File.ReadAllText(filePath);
                return value;
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Directory Not Found");
                Environment.Exit(1);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File Not Found");
                Environment.Exit(2);
            }
            catch (Exception)
            {
                Console.WriteLine("Argument Error");
                Environment.Exit(3);
            }
            return "";
        }
    }
}
