using Ambev.Core.Domain.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.Infrastructure.Persistence.MongoDB.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IMongoClient client)
        {
            _database = client.GetDatabase("AMBEV");
        }

        public IMongoCollection<Cart> Carts => _database.GetCollection<Cart>("Carts");
    }
}
