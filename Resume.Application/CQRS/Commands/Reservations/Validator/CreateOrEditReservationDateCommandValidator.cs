using FluentValidation;
using Resume.Application.CQRS.Commands.Reservations;

public class CreateOrEditReservationDateCommandValidator
    : AbstractValidator<CreateOrEditReservationDateCommand>
{
    public CreateOrEditReservationDateCommandValidator()
    {
        // Validate that the ViewModel is not null
        RuleFor(x => x.ReservationVm)
            .NotNull().WithMessage("Reservation data is required.");

        // If the ViewModel is valid, check its internal rules
        When(x => x.ReservationVm != null, () =>
        {
            // Create scenario
            When(x => x.ReservationVm.Id == 0, () =>
            {
                RuleFor(x => x.ReservationVm.ReservationDate)
                    .NotEmpty().WithMessage("Reservation date is required for creating new entry.");
            });

            // Edit scenario
            When(x => x.ReservationVm.Id > 0, () =>
            {
                RuleFor(x => x.ReservationVm.Id)
                    .GreaterThan(0).WithMessage("Invalid reservation ID for edit.");

                RuleFor(x => x.ReservationVm.ReservationDate)
                    .NotEmpty().WithMessage("Reservation date is required for editing.");
            });

            // Example for an optional text field if you have one (e.g., reservation title)
            // RuleFor(x => x.ReservationVm.Title)
            //     .NotEmpty().WithMessage("Title is required.")
            //     .MaximumLength(100).WithMessage("Title must be less than 100 characters.");
        });
    }
}
