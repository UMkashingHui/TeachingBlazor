
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TeachingWebApi.Config;
using TeachingWebApi.Models;

namespace TeachingWebApi.Utils.Data
{
    public class MongoDbContext : DbContext
    {
        public readonly IMongoDatabase _db;
        private readonly string _booksCollectionName;
        private readonly IOptions<TeachingAppDatabaseSettings> _setting;

        public MongoDbContext(IOptions<TeachingAppDatabaseSettings> setting)
        {
            _setting = setting;
            var client = new MongoClient(_setting.Value.ConnectionString);
            _db = client.GetDatabase(_setting.Value.DatabaseName);
            _booksCollectionName = _setting.Value.BooksCollectionName;
        }

        public IMongoCollection<Book> Books =>
        _db.GetCollection<Book>(_booksCollectionName);

    }
}