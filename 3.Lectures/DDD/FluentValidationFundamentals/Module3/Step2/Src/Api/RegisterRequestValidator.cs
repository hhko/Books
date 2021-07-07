using System.Text.RegularExpressions;
using FluentValidation;

namespace Api
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            //
            // Case 2. Validation 실패 정보 1개(첫 실패)
            //
            //CascadeMode = CascadeMode.Stop;       // Class 설정

            RuleFor(x => x.Name)
                //.NotNull()                        // NULL
                .NotEmpty()                         // NULL and Empty
                .Length(0, 200);

            RuleFor(x => x.Email)
                .NotEmpty()
                .Length(0, 150)
                .EmailAddress();                    // .NET 5.0 : "^(.+)@(.+)$"

            RuleFor(x => x.Addresses)
                .NotNull()     
                .SetValidator(new AddressesVilidator());

            //
            // Case 1. Validation 실패 정보 N개(모든 실패)
            //
            RuleFor(x => x.Phone)
                //.Cascade(CascadeMode.Stop)        // Rule Chine 설정
                .NotEmpty()
                .Must(x => Regex.IsMatch(x, "^[2-9][0-9]{9}"))      // NotEmpty가 실패하면 Must는 실행하지 않는다.
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