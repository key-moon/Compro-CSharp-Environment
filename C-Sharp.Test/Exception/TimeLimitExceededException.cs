using System;
using System.Collections.Generic;
using System.Text;

[Serializable]
public class TimeLimitExceededException : Exception
{
    public TimeLimitExceededException() { }
    public TimeLimitExceededException(int tl) : base($"\nExceed time limit({tl}ms)") { }
    protected TimeLimitExceededException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
