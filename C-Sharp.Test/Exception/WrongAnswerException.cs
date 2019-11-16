using System;
using System.Collections.Generic;
using System.Text;

[Serializable]
public class WrongAnswerException : Exception
{
    public override string Message =>
        $"{(Input is null ? "" : $"\nInput: \n{Input.Trim()}")}" +
        $"{(Expected is null ? "" : $"\nExpected: \n{Expected.Trim()}")}" +
        $"{(Output is null ? "" : $"\nOutput: \n{Output.Trim()}")}" +
        $"{(Debug is null ? "" : $"\nDebug: \n{Debug.Trim()}")}";

    public string Input;
    public string Expected;
    public string Output;
    public string Debug;
    public WrongAnswerException() { }
    public WrongAnswerException(Exception innerException) : base("", innerException) { }
    public WrongAnswerException(string input, string expected, string output, string debug)
    {
        Input = input;
        Expected = expected;
        Output = output;
        Debug = debug;
    }
    protected WrongAnswerException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
