using CRUDOpperationMongoDB1.Application.Command.Tickets;
using FluentValidation;
namespace CRUDOpperationMongoDB1.Application.Handler.CommandHandlers
{
    // ke thua thu vien fluentvalidation abstractValidatior<T>
    // T la update --> dung de update

    public class UpdateTicketCommandValidator : AbstractValidator<UpdateTicketCommand>
    {
        // controctor goi 6 ham
        public UpdateTicketCommandValidator()
        {
            ValidateId();
            ValidateAddresses();
            ValidateDates();
            ValidateQuantity();
            ValidateCustomerInfo();
            ValidateEnums();
        }
        // uleFor(x => x.Id): tao obj kiem tra thuoc tinh id trong updateTicketCommand
        //NotEmpty(). : id ko dc de trong
        //WithMessage : thong bao
        public void ValidateId()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id khong duoc de trong");
        }
        // from and to: kiem tra co null ko va neu ko null thi ko vuot qua 200 ky tu
        //.NotEmpty() : kiem tra xem co bo trong hay ko
        //  .MaximumLength: keim tra do dai co vuot qua so luong ky tu ko
        //MESSAGE: thong bao
        public void ValidateAddresses()
        {
            RuleFor(x => x.FromAddress).NotEmpty().WithMessage("Dia chi di khong duoc de trong.")
                .MaximumLength(200).WithMessage("Dia chi di toi da 200 ky tu.");

            RuleFor(x => x.ToAddress)
                .NotEmpty().WithMessage("Dia chi den khong duoc de trong")
                .MaximumLength(200).WithMessage("Dia chi den toi da 200 ky tu.");
        }
        // .GreaterThanOrEqualTo(DateTime.Today).: kiem tra bgat di co lon hon hoac bang nagy hien tai khong 
        // khong te cap nhat ve khi ngay trong qua khu
        //.GreaterThanOrEqualTo : kiem tra ngay den phai lon hon ngay di
        public void ValidateDates()
        {
            RuleFor(x => x.FromDate).NotEmpty().WithMessage("Ngay di khong duoc de trong.")
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Ngay di khong duoc nho hon ngay hien tai.");


            RuleFor(x => x.ToDate).GreaterThanOrEqualTo(x => x.FromDate)
            .WithMessage("ToDate phai lon hon hoac bang FromDate");
        }
        // Quantity: kiem tra so luong ve phai lon hon 0. nguoc lai bao loi
        public void ValidateQuantity()
        {
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("So luong phai lon hon 0.");
        }
        // CustomerName: ten khach hang ko dc de trong .NotEmpty() va  khong qua 100 ky tu   .MaximumLength(100)
        // CustomerPhone: kiem tra sdt co null ko .NotEmpty(). va phai khop voi chinh quy regax sdt tu 10 -15 so
        public void ValidateCustomerInfo()
        {
            RuleFor(x => x.CustomerName).NotEmpty().WithMessage("Ten khach khong duoc de trong")
            .MaximumLength(100).WithMessage("Ten khach hang toi da 100 ky tu.");
            RuleFor(x => x.CustomerPhone).NotEmpty().WithMessage("So dien thoai khong duoc de trong")
                .Matches(@"^\d{ 10,15}$").WithMessage("So dien thoai phai tu 10 den 15 chu so");
        }
        //kiem tra status va ticketype co hop le cua enum khong
        private void ValidateEnums()
        {
            RuleFor(x => x.TicketType).IsInEnum().WithMessage("Loai ve khong hop le");
            RuleFor(x => x.Status).IsInEnum().WithMessage("Trang thai ve khong hop le");
        }
       
    }
}
