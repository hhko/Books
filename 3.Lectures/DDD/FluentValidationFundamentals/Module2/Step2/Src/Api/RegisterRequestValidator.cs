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

            //
            // Simple -> Complex properties
            //
            // RuleFor(x => x.Address)
            //     .NotEmpty()
            //     .Length(0, 150);

            // // Case 1. Inline 버그 有 : Address가 NULL일 때
            // RuleFor(x => x.Address.Street).NotEmpty().Length(0, 100);
            // RuleFor(x => x.Address.City).NotEmpty().Length(0, 40);
            // RuleFor(x => x.Address.State).NotEmpty().Length(0, 2);
            // RuleFor(x => x.Address.ZipCode).NotEmpty().Length(0, 5);

            // // Case 2. Inline 버그 無 : Address가 NULL일 때
            // RuleFor(x => x.Address).NotNull();
            // RuleFor(x => x.Address.Street).NotEmpty().Length(0, 100).When(x => x.Address != null);
            // RuleFor(x => x.Address.City).NotEmpty().Length(0, 40).When(x => x.Address != null);
            // RuleFor(x => x.Address.State).NotEmpty().Length(0, 2).When(x => x.Address != null);
            // RuleFor(x => x.Address.ZipCode).NotEmpty().Length(0, 5).When(x => x.Address != null);

            // Case 3. Sub-Validator : SetValidator
            RuleFor(x => x.Address).NotNull().SetValidator(new AddressVilidator());
            

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
}