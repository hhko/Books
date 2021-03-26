using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Step_05_DecoratorValidation
{
    public class DivideValidator : AbstractValidator<Divide>
    {
        public DivideValidator()
        {
            RuleFor(_ => _.Y)
                .NotEqual(0)
                .WithMessage("분모 Y 값이 0입니다. 0 외 값을 입력하십시오.");
        }
    }
}
