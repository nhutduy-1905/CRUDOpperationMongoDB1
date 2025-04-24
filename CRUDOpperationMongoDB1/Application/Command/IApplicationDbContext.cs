using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;
using CRUDOpperationMongoDB1.Domain.Entities;
using CRUDOpperationMongoDB1.Models;
using Microsoft.Extensions.Options;

namespace CRUDOpperationMongoDB1.Data
{
    public interface IApplicationDbContext
    {
        IMongoCollection<Ticket> Tickets { get; }
    }


    public class ApplicationDbContext : IApplicationDbContext
    {
        private readonly IMongoDatabase _database;

        public ApplicationDbContext(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Ticket> Tickets => _database.GetCollection<Ticket>("Tickets");
    }
}