using FluentValidation;

namespace Api
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            //if (string.IsNullOrWhiteSpace(request.Name))
            //    return BadRequest("Name cannot be empty");
            //if (request.Name.Length > 200)
            //    return BadRequest("Name is too long");

            RuleFor(x => x.Name)
                //.NotNull()        // NULL
                .NotEmpty()         // NULL and Empty
                .Length(0, 200);

            //if (string.IsNullOrWhiteSpace(request.Email))
            //    return BadRequest("Email cannot be empty");
            //if (request.Email.Length > 150)
            //    return BadRequest("Email is too long");
            //if (!Regex.IsMatch(request.Email, @"^(.+)@(.+)$"))
            //    return BadRequest("Email is invalid");

            RuleFor(x => x.Email)
                .NotEmpty()
                .Length(0, 150)
                .EmailAddress();        // .NET 5.0 : "^(.+)@(.+)$"

            //if (string.IsNullOrWhiteSpace(request.Address))
            //    return BadRequest("Address cannot be empty");
            //if (request.Address.Length > 150)
            //    return BadRequest("Address is too long");

            // 배열 객체 Validation
            RuleFor(x => x.Addresses)
                .NotNull()
                .Must(x => x.Length >=1 && x.Length <= 3)
                .WithMessage("The number of addresses must be between 1 and 3");

            // 배열 개별 객체 Validation
            RuleForEach(x => x.Addresses)
                .SetValidator(new AddressVilidator());
            

            //
            // 아직 구현 안된 Data Contract Valiation
            //
            //if (request == null)
            //    return BadRequest("Request cannot be null");
            // Email should be unique.
            // Return a list of errors, not just the first one
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

            RuleForEach(x => x.Addresses)
                .SetValidator(new AddressVilidator());
        }
    }
}