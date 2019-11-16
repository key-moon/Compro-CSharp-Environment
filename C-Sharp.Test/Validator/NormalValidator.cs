using System;
using System.Collections.Generic;
using System.Text;

public class NormalValidator : ValidatorBase
{
    string Ans;
    public NormalValidator(string ans) { Ans = ans; }
    public override void Validate(string a)
    {
        if (Equal(Ans, a)) return;
        throw new WrongAnswerException(null, Ans.Trim(), a.Trim(), null);
    }
    public virtual bool Equal(string a, string b) => a.Equals(b);

    public static NormalValidator Factory(string contestSite, string ans)
    {
        return new AtCoderValidator(ans);
        //とりあえず全部AtCoder validatorで…
        if (contestSite.Contains("atcoder")) return new AtCoderValidator(ans);
        return new NormalValidator(ans);
    }
}


