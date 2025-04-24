using MediatR;
using CRUDOpperationMongoDB1.Domain.Entities;
using System.Collections.Generic;
namespace CRUDOpperationMongoDB1.Application.Queries
{
    public class GetAllTicketsQuery : IRequest<IEnumerable<Ticket>>
    {

    }
}
