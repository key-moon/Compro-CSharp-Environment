using System;
using System.Collections.Generic;
using System.Text;

public class SpecialValidator : ValidatorBase
{
    Action<string> ValidatorFunc;
    public SpecialValidator(Action<string> validatorFunc) { ValidatorFunc = validatorFunc; }
    public override void Validate(string a) => ValidatorFunc(a);
}
