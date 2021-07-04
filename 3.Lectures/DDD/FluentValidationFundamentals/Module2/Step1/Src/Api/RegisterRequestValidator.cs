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

            RuleFor(x => x.Address)
                .NotEmpty()
                .Length(0, 150);

            //
            // 아직 구현 안된 Data Contract Valiation
            //
            //if (request == null)
            //    return BadRequest("Request cannot be null");
            // Email should be unique.
            // Return a list of errors, not just the first one
        }
    }
}