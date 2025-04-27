using CRUDOpperationMongoDB1.Application.Command.Tickets;
using FluentValidation;

namespace CRUDOpperationMongoDB1.Application
{
    public class DeleteTicketCommandValidator : AbstractValidator<DeleteTicketCommand>
    {
        public DeleteTicketCommandValidator()
        {
            // NotEmpty: laoi bo khoang trang
            // Matches: kiem tra id nhap xem co giong voi object id trong mogo khong
            RuleFor(x => x.Id).NotEmpty().WithMessage("Ticket ID canot be empty").Matches("^[a-fA-F0-9]{24}$").WithMessage("Invalid MongoDB object format.");
        }
    }
}
