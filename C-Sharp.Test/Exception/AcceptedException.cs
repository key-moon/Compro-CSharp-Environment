using System;
using System.Collections.Generic;
using System.Text;

[Serializable]
public class AcceptedException : Exception
{
    public override string Message =>
        $"{(Input is null ? "" : $"\nInput: \n{Input.Trim()}")}" +
        $"{(Expected is null ? "" : $"\nExpected: \n{Expected.Trim()}")}" +
        $"{(Output is null ? "" : $"\nOutput: \n{Output.Trim()}")}";

    public string Input;
    public string Expected;
    public string Output;
    public AcceptedException() { }
    public AcceptedException(Exception innerException) : base("", innerException) { }
    public AcceptedException(string input, string expected, string output)
    {
        Input = input;
        Expected = expected;
        Output = output;
    }
    protected AcceptedException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
