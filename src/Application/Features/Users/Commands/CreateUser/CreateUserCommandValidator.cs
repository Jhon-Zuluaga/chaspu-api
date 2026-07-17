
using System.Data;
using FluentValidation;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("The email address is not in a valid format.")
            .MustAsync(async (email, cancellationToken) =>
                !await _userRepository.EmailExistsAsync(email, cancellationToken))
            .WithMessage("A user with this email address already exists.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("The password must be at least 6 characters long.");

        RuleFor(x => x.Role)
            .IsInEnum().WithMessage("The role is invalid.");
    }
}