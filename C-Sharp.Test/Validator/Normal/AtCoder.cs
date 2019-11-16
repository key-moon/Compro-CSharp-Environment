using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

public class AtCoderValidator : NormalValidator
{
    public AtCoderValidator(string ans) : base(ans) { }
    public override bool Equal(string a, string b)
    {
        var sharpedA = string.Join("\n", a.Trim((char)0x0d, (char)0x0a).Split('\n').Select(x => x.Trim()).Where(x => x.Length != 0));
        var sharpedB = string.Join("\n", b.Trim((char)0x0d, (char)0x0a).Split('\n').Select(x => x.Trim()).Where(x => x.Length != 0));
        return sharpedA == sharpedB;
    }
}
