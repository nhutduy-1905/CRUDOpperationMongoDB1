using CRUDOpperationMongoDB1.Application.Interfaces;
using CRUDOpperationMongoDB1.Application.Queries;
using MediatR;
using CRUDOpperationMongoDB1.Domain.Entities;

namespace CRUDOpperationMongoDB1.Application.Handler.QueryHandlers
{
    // GetAllTicketsQueryHandler: lop handler xu ly cau lenh query
    //GetAllTicketsQuery: class nay lay tat ca ve tu csdl thong qua ITicketRepository
    //  IRequestHandler<GetAllTicketsQuery, IEnumerable<Ticket>>: thuc hien <GetAllTicketsQuery
    // thuc hiencau lenh query va gioa tiep mediaTR thong qua  IRequestHandler xu ly yeu cau GetAllTicketsQuery
    // tra ve ket qua co kieu du lieu la  IEnumerable
    public class GetAllTicketsQueryHandler : IRequestHandler<GetAllTicketsQuery, IEnumerable<Ticket>>
    {
        // _ticketRepository: bien tham  chieu  ITicketRepository truy cap csdl tickets
        // readonly co the duoc gan gia tri mot lan
        private readonly ITicketRepository _ticketRepository;

        // Ha, khoi tao  nhan tham so :  ticketRepository, dung de khoi tao _ticketRepository
        public GetAllTicketsQueryHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }
        // async: phuong thuc xu ly chinh cua IRequestHandler no nhan hai tham so:
        // request: la tham so cua kieu GetAllTicketsQuery
        // Can: la doi tuong huy bo tac vu khi user huy yeu cau hoac het thoi gian
        public async Task<IEnumerable<Ticket>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
        {
            // await :giup chuong trinh ko bi chan va cho phep cac tac vu khac thuc thi trong khi cho ket qua
            //Truy van dong bo lay tat ca ve tu csdl thong qua phuong thuc getall cua Iticket
            return await _ticketRepository.GetAllTicketsAsync();
        }
    }

}
