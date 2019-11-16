using System;
using System.Collections.Generic;
using System.Text;

[Serializable]
public class RuntimeErrorException : Exception
{
    public override string Message =>
        $"{(Input is null ? "" : $"\nInput: \n{Input.Trim()}")}" +
        $"{(Exception is null ? "" : $"\nMessage: \n{Exception.Message}")}" +
        $"{(Debug is null ? "" : $"\nDebug: \n{Debug.Trim()}")}";


    public string Input;
    public Exception Exception;
    public string Debug;
    public RuntimeErrorException() { }
    public RuntimeErrorException(Exception inner) : base($"\nMessage : \n{inner.Message}\n", inner) { }
    protected RuntimeErrorException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
