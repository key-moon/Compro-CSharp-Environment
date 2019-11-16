using System;
using System.IO;
using System.Diagnostics;

using System.Collections.Generic;
using System.Text;

public static class Interactive
{
    public static TextReader In;
    public static TextWriter Out;
    public static Func<string> ReadLine => In.ReadLine;
    public static Action<string> WriteLine => Out.WriteLine;
    public static void Test(string input)
    {
        //TestWithPythonScript(@$"C:\Users\{Secret.USERNAME}\Downloads\testing_tool.py", input);
    }

    static void TestWithPythonScript(string pythonScriptPath, string arg)
    {
        using (var tester = new Process())
        {
            tester.StartInfo = new ProcessStartInfo("py.exe")
            {
                Arguments = $"{pythonScriptPath} {arg}",
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };
            tester.Start();
            tester.WaitForExit();
        }
    }
}
