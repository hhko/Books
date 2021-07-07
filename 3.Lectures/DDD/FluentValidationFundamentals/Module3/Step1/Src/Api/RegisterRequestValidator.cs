using System.Text.RegularExpressions;
using FluentValidation;

namespace Api
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Name)
                //.NotNull()        // NULL
                .NotEmpty()         // NULL and Empty
                .Length(0, 200);

            RuleFor(x => x.Email)
                .NotEmpty()
                .Length(0, 150)
                .EmailAddress();        // .NET 5.0 : "^(.+)@(.+)$"

            RuleFor(x => x.Addresses)
                .NotNull()     
                .SetValidator(new AddressesVilidator());

            // 
            // Case 1. 전체 규칙 실행 판단(When)
            //
            // RuleFor(x => x.Phone)
            //     .NotEmpty()
            //     .Must(x => Regex.IsMatch(x, "^[2-9][0-9]{9}"))
            //     .When(x => x.Phone != null)                                     // 전체 규칙(NotEmpty, Must) 실행 조건이다.
            //     .WithMessage("The phone number is incorrect");

            //
            // Case 2. 현재 규칙(바로 앞에 정의된 규칙)만 실행 판단(When)
            //
            RuleFor(x => x.Phone)
                .NotEmpty()
                .Must(x => Regex.IsMatch(x, "^[2-9][0-9]{9}"))
                .When(x => x.Phone != null, ApplyConditionTo.CurrentValidator)  // Must 규칙만 실행 조건이다.
                .WithMessage("The phone number is incorrect");

            //
            // Case 3. 정규식 내장 함수(Matches)
            //
            RuleFor(x => x.Phone)
                .NotEmpty()
                .Matches("^[2-9][0-9]{9}")
                .WithMessage("The phone number is incorrect");

            //
            // 아직 구현 안된 Data Contract Valiation
            //
            //if (request == null)
            //    return BadRequest("Request cannot be null");
            // Email should be unique.
            // Return a list of errors, not just the first one
        }
    }

    public class AddressesVilidator : AbstractValidator<AddressDto[]>
    {
        public AddressesVilidator()
        {
            RuleFor(x => x)
                //.NotNull()                                      // 독립적 실행
                .Must(x => x?.Length >=1 && x.Length <= 3)      // 독립적 실행 : NULL 처리
                .WithMessage("The number of addresses must be between 1 and 3")
                .ForEach(x => 
                {
                    x.NotNull();
                    x.SetValidator(new AddressVilidator());
                });
        }
    }

    public class AddressVilidator : AbstractValidator<AddressDto>
    {
        public AddressVilidator()
        {
            RuleFor(x => x.Street).NotEmpty().Length(0, 100);
            RuleFor(x => x.City).NotEmpty().Length(0, 40);
            RuleFor(x => x.State).NotEmpty().Length(0, 2);
            RuleFor(x => x.ZipCode).NotEmpty().Length(0, 5);
        }
    }

    public class EditPersonalInfoRequestValidator : AbstractValidator<EditPersonalInfoRequest>
    {
        public EditPersonalInfoRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(0, 200);

            // RuleForEach(x => x.Addresses)
            //     .SetValidator(new AddressVilidator());
            
            // RuleFor(x => x.Addresses)
            //     .NotNull()                                      // 독립적 실행
            //     .Must(x => x?.Length >=1 && x.Length <= 3)      // 독립적 실행 : NULL 처리
            //     .WithMessage("The number of addresses must be between 1 and 3")
            //     .ForEach(x => 
            //     {
            //         x.NotNull();
            //         x.SetValidator(new AddressVilidator());
            //     });

            RuleFor(x => x.Addresses)
                .NotNull()      
                .SetValidator(new AddressesVilidator());
        }
    }
}