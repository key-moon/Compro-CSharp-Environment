using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

enum ProblemKind
{
    Normal = 0,
    Special = 1,
    Interactive = 2
}

[TestFixture]
public static class Tests
{
    static string TestCaseDirectory = @$"C:\Users\{Secret.USERNAME}\compro\Program\C-Sharp\Cases";
    static string ProblemInfoFileName = "probleminfo.json";
    static string InputCaseDirectoryName = "In";
    static string OutputCaseDirectoryName = "Out";

    static ProblemKind ProblemKind;
    static ProblemInfo ProblemInfo;

    static string InputCaseDirectoryPath => Path.Combine(TestCaseDirectory, InputCaseDirectoryName);
    static string OutputCaseDirectoryPath => Path.Combine(TestCaseDirectory, OutputCaseDirectoryName);

    static IEnumerable<string> CaseNames => Directory.GetFiles(InputCaseDirectoryPath).Select(Path.GetFileName).Intersect(Directory.GetFiles(OutputCaseDirectoryPath).Select(Path.GetFileName));

    static IEnumerable<TestCaseData> Cases =>
        CaseNames.Select(CaseName =>
            {
                var input = ReadToEnd(Path.Combine(InputCaseDirectoryPath, CaseName));
                var output = ReadToEnd(Path.Combine(OutputCaseDirectoryPath, CaseName));
                return new TestCaseData(input, output)
                    .Returns(true)
                    .SetName($"{CaseName}");
            }
        );

    [SetUp]
    public static void Setup()
    {
        ProblemInfo = JsonConvert.DeserializeObject<ProblemInfo>(ReadToEnd(Path.Combine(TestCaseDirectory, ProblemInfoFileName)));
        ProblemKind = ProblemInfo.Interactive ? ProblemKind.Interactive : ProblemKind.Normal;
    }

    [TestCaseSource("Cases")]
    public static bool TestExecuter(string input, string output)
    {
        if (ProblemKind == ProblemKind.Normal) UnilateralTest(input, NormalValidator.Factory(ProblemInfo.Url, output));
        if (ProblemKind == ProblemKind.Special) UnilateralTest(input, new SpecialValidator((ans) => Validator.Validate(input, ans)));
        if (ProblemKind == ProblemKind.Interactive) InteractiveTest(input);
        return true;
    }

    public static void UnilateralTest(string input, ValidatorBase validator)
    {
        var inStream = new MemoryStream();
        var outBuilder = new StringBuilder();
        var errorBuilder = new StringBuilder();
        Console.SetIn(new StreamReader(inStream));
        Console.SetOut(new StringWriter(outBuilder));
        Trace.Listeners.Add(new TextWriterTraceListener(new StringWriter(errorBuilder)));
        Console.SetError(new StringWriter(errorBuilder));
        var bytes = Encoding.UTF8.GetBytes(input);
        inStream.Write(bytes, 0, bytes.Length);
        inStream.Position = 0;
        SetStreamToReader(inStream);
        
        RunProgram();

        var res = outBuilder.ToString();
        try
        {
            validator.Validate(res);
        }
        catch (WrongAnswerException waException)
        {
            waException.Input = input;
            waException.Debug = errorBuilder.ToString();
            throw waException;
        }
    }

    //WIP: 多分前のbufferが残ってるか何かしていて、複数テストケース連続でやると死ぬ
    private static void SetStreamToReader(Stream stream)
    {
        var reader = Type.GetType("Reader, C-Sharp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
        if (reader is null) return;
        var streamField = reader.GetField("Stream", BindingFlags.NonPublic | BindingFlags.Static);
        streamField.SetValue(null, stream);
    }

    public static void InteractiveTest(string input)
    {
        var inStream = new MemoryStream();
        var outStream = new MemoryStream();
        Console.SetIn(new StreamReader(inStream));
        Console.SetOut(new StreamWriter(outStream));
        Interactive.In = new StreamReader(outStream);
        Interactive.Out = new StreamWriter(inStream);
        var judge = Task.Run(() => Interactive.Test(input));

        try
        {
            RunProgram();
        }
        catch (Exception e)
        {
            throw new RuntimeErrorException(e);
        }
        if (judge.IsFaulted) throw new WrongAnswerException(judge.Exception);
    }

    private static void RunProgram()
    {
        try
        {
            P.Main();
            /*var mainClass = Type.GetType("P, C-Sharp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            var mainFunc = mainClass.GetMethod("Main", BindingFlags.NonPublic & BindingFlags.Static);
            mainFunc.Invoke(null, null);*/
            //if (!Task.Run(P.Main).Wait(ProblemInfo.TimeLimit))
            //throw new TimeLimitExceededException(ProblemInfo.TimeLimit);
        }
        catch (AggregateException e)
        {
            throw e.InnerException;
        }
        catch (TargetInvocationException e)
        {
            throw e.GetBaseException();
        }
    }

    private static string ReadToEnd(string path)
    {
        using (StreamReader reader = new StreamReader(path))
        {
            return reader.ReadToEnd();
        }
    }
}

public class ProblemInfo
{
    public string Name;
    public string Group;
    public string Url;
    public bool Interactive;
    public int MemoryLimit;
    public int TimeLimit;
}
