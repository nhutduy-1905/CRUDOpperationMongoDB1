using MediatR;

namespace CRUDOpperationMongoDB1.Application.Queries
{
    public class ExportTicketsPaginatedQuery : IRequest<byte[]>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 25;
    }
}
