using MongoDB.Driver;
using Microsoft.Extensions.Options;
using CRUDOpperationMongoDB1.Domain.Entities;
using CRUDOpperationMongoDB1.Models;

namespace CRUDOpperationMongoDB1.Data
{
    public interface IApplicationDbContext
    {
        IMongoCollection<Ticket> Tickets { get; }
        IMongoCollection<Customer> Customers { get; }
        IMongoCollection<Post> Post { get; }
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
        public IMongoCollection<Customer> Customers => _database.GetCollection<Customer>("Customers");
        public IMongoCollection<Post> Post => _database.GetCollection<Post>("Posts");


    }
}
