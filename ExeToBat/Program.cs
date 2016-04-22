using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace exeToBat
{
    enum FileType { Exe = 0, Dll = 1,}

    class ArgumentOptions
    {
        private String[] Args;
        private UInt16 argsAmount;

        internal FilePathLocations Locations = new FilePathLocations();
        internal FileType Type = FileType.Exe;

        internal ArgumentOptions(string [] args)
        {
            Args = args.Select(x => x.ToLower()).ToArray();
            argsAmount = (ushort)Args.Length;
            if (argsAmount == 0) WrongArgs();
            AnalyzeFlags();
            if (argsAmount > 3 || argsAmount == 0) WrongArgs();
            Locations.MainFile = Args[0];
            if (argsAmount > 1)
            {
                Locations.OutputFile = args[1];

                if (argsAmount > 2)
                {
                    if (args[2].Last() != '\\') args[2] = args[2] + '\\';
                    Locations.OutcomeFile = args[2] + Path.GetFileNameWithoutExtension(Locations.OutputFile) + ".vbs";

                }
                else
                {
                    Locations.OutcomeFile = @"%temp%\" + Path.GetFileNameWithoutExtension(Locations.OutputFile) + ".vbs";
                }
            }
            else
            {
                Locations.OutputFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + Path.GetFileNameWithoutExtension(Locations.MainFile) + ".bat";
                Locations.OutcomeFile = @"%temp%\" + Path.GetFileNameWithoutExtension(Locations.OutputFile) + ".vbs";
            }

            Locations.SetEndingExt(this.Type);
        }

        private void AnalyzeFlags()
        {
            foreach (string arg in Args)
            {
                if (arg[0] == '-')
                {
                    switch (arg[1])
                    {
                        case 'd':
                            Type = FileType.Dll;
                            break;
                        case 'e':
                            Type = FileType.Exe;
                            break;
                        default:
                            break;
                    }
                }
            }

            Args = Args.Where(X => X[0] != '-').ToArray();
            argsAmount = (ushort)Args.Length;
        }

        private void WrongArgs()
        {
            Console.WriteLine("ExeToBat.exe [-d][-e] ExeFile [Output File] [Outcome File]");
            Environment.Exit(1);
        }
    }

    class FilePathLocations
    {
        internal String MainFile;
        internal String OutputFile;
        internal String OutcomeFile;

        private String finalEnding = ".exe";

        internal void SetEndingExt(FileType byType) { if (byType == FileType.Dll) finalEnding = ".dll"; }
        internal String OutcomeFinalFile { get {   return OutcomeFile.Replace(".vbs", finalEnding); } }
    }

    class MyVBStreamWriter : StreamWriter
    {
        String VBLocation;
        internal MyVBStreamWriter(string vbLocation, string fileToWrite, bool append) : base(fileToWrite, append)
        {
            VBLocation = vbLocation;
        }

        internal void WriteToVB(string text, params string[] param)
        {
            this.WriteLine(String.Format("echo " + text + " >> " + VBLocation, param));
        }

        internal void WriteToBat(string text, params string[] param)
        {
            this.WriteLine(String.Format(text, param));
        }
    }

    class Program
    {
        const int SPLIT_SIZE = 500;

        /// <summary>
        /// Wrote by Odahviing
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("ExeToBat - Version 1.2, by Odahviing");
            ArgumentOptions Options = new ArgumentOptions(args);

            try
            {
                using (MyVBStreamWriter Writer = new MyVBStreamWriter(Options.Locations.OutcomeFile, Options.Locations.OutputFile, false))
                {
                    Writer.WriteToBat("if not DEFINED IS_MINIMIZED set IS_MINIMIZED=1 && start \"\" /min \"%~f0\" %* && exit");
                    Writer.WriteToVB("On Error Resume Next");

                    Byte[] Array = System.IO.File.ReadAllBytes(Options.Locations.MainFile);
                    char[] hex = BitConverter.ToString(Array).ToCharArray();
                    int count = 0;

                    string text = "";
                    Writer.WriteToVB("c = \"\"");

                    for (uint i = 0; i < hex.Length; i++)
                    {
                        if (hex[i] == '-') continue;

                        if (count == SPLIT_SIZE)
                        {
                            text = text + hex[i];
                            Writer.WriteToVB("c = c + \"{0}\"", text);
                            text = "";
                            count = 0;
                        }
                        else
                        {
                            text = text + hex[i];
                            count++;
                        }
                    }


                    Writer.WriteToVB("c = c + \"{0}\"", text);
                    Writer.WriteToVB("Set ts = CreateObject(\"Scripting.FileSystemObject\").OpenTextFile(\"{0}\", 2, True)", Options.Locations.OutcomeFinalFile);
                    Writer.WriteToVB("For x = 1 To len(c) Step 2");
                    Writer.WriteToVB("ts.Write Chr(CInt(\"&H\" + Mid(c, x, 2)))");
                    Writer.WriteToVB("Next");
                    Writer.WriteToVB("ts.Close");
                    Writer.WriteToVB("Set s = WScript.CreateObject(\"WScript.Shell\")");

                    if (Options.Type == FileType.Dll)
                    {
                        Writer.WriteToVB("s.Run \"rundll32.exe \"\"{0}\"\" dllmain\"", Options.Locations.OutcomeFinalFile);
                    }
                    else // FileType.Exe
                        Writer.WriteToVB("s.Run \"{0}\"", Options.Locations.OutcomeFinalFile);
                    Writer.WriteToBat(Options.Locations.OutcomeFile);
                    Writer.WriteToBat("timeout 60");
                    Writer.WriteToBat("del {0}", Options.Locations.OutcomeFile);
                    Writer.WriteToBat("del {0}", Options.Locations.OutcomeFinalFile);
                    Writer.WriteToBat("exit");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
