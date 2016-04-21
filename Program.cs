using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace exeToBat
{
    class Program
    {
        const int SPLIT_SIZE = 5000;

        /// <summary>
        /// Wrote by Odahviing for Bugsec use
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            #region Args
            string exeFile, outPutFile, finalExeFile;

            if (args.Length == 1)
            {
                exeFile = args[0];
                outPutFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + Path.GetFileNameWithoutExtension(exeFile) + ".bat";
                finalExeFile = @"%temp%\" + Path.GetFileNameWithoutExtension(outPutFile) + ".vbs";
            }
            else if(args.Length == 2)
            {
                exeFile = args[0];
                outPutFile = args[1];
                finalExeFile = @"%temp%\" + Path.GetFileNameWithoutExtension(outPutFile) + ".vbs";
            }
            else if(args.Length == 3)
            {
                exeFile = args[0];
                outPutFile = args[1];
                if (args[2].Last() != '\\') args[2] = args[2] + '\\';
                finalExeFile = args[2] + Path.GetFileNameWithoutExtension(outPutFile) + ".vbs";

            }
            else
            {
                exeFile = @"F:\Workspace\Projects\Bat2Exe\hh.exe";
                outPutFile = @"F:\Workspace\Projects\Bat2Exe\hh.bat";
                finalExeFile = @"%temp%\" + Path.GetFileNameWithoutExtension(outPutFile) + ".vbs";
            }

            #endregion

            try
            {
                using (StreamWriter Writer = new StreamWriter(outPutFile, true))
                {
                    Writer.WriteLine("if not DEFINED IS_MINIMIZED set IS_MINIMIZED=1 && start \"\" /min \"%~f0\" %* && exit");
                    Writer.WriteLine(@"echo On Error Resume Next >> " + finalExeFile);
                    Byte[] Array = System.IO.File.ReadAllBytes(exeFile);
                    char[] hex = BitConverter.ToString(Array).ToCharArray();
                    int count = 0;

                    string text = "";
                    Writer.WriteLine(@"echo c = """" >> " + finalExeFile);
                    for (uint i = 0; i < hex.Length; i++)
                    {
                        if (hex[i] == '-') continue;
                        
                        if (count == SPLIT_SIZE)
                        {
                            text = text + hex[i];
                            Writer.WriteLine("echo c = c + \"" + text + "\" >> " + finalExeFile);
                            text = "";
                            count = 0;
                        }
                        else
                        {
                            text = text + hex[i];
                            count++;
                        }
                    }

                    Writer.WriteLine("echo c = c + \"" + text + "\" >> " + finalExeFile);
                    Writer.WriteLine(@"echo Set ts = CreateObject(""Scripting.FileSystemObject"").OpenTextFile(""" + finalExeFile.Replace(".vbs",".exe") + "\", 2, True) >> " + finalExeFile);
                    Writer.WriteLine(@"echo For x = 1 To len(c) Step 2 >> " + finalExeFile);
                    Writer.WriteLine(@"echo ts.Write Chr(CInt(""&H"" + Mid(c, x, 2))) >> " + finalExeFile);
                    Writer.WriteLine(@"echo Next >> " + finalExeFile);
                    Writer.WriteLine(@"echo ts.Close >> " + finalExeFile);
                    Writer.WriteLine(@"echo Set s = WScript.CreateObject(""WScript.Shell"") >> " + finalExeFile);
                    Writer.WriteLine(@"echo s.Run """ + finalExeFile.Replace(".vbs",".exe") + @""" >> " + finalExeFile);
                    Writer.WriteLine(finalExeFile);
                    Writer.WriteLine("timeout 60");
                    Writer.WriteLine("del finalExeFile");
                    Writer.WriteLine("exit");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
}
