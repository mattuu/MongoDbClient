using Microsoft.Extensions.Options;
using MongoDbClient.ConsoleApp.Models;
using MongoDB.Driver;

namespace MongoDbClient.ConsoleApp
{
    public class NoteContext
    {
        private readonly IMongoDatabase _database;

        public NoteContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Customer> Notes => _database.GetCollection<Customer>("Customer");
    }
}