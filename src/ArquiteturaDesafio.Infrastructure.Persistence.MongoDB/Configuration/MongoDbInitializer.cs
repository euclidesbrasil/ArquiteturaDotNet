using ArquiteturaDesafio.Core.Domain.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Infrastructure.Persistence.MongoDB.Configuration
{
    public class MongoDbInitializer
    {
        private readonly IMongoDatabase _database;

        public MongoDbInitializer(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task InitializeAsync()
        {
            // Verifica se a coleção "Carts" já existe
            var collections = await _database.ListCollectionNamesAsync();
            var collectionList = await collections.ToListAsync();

            if (!collectionList.Contains("DailyBalanceReport"))
            {
                // Cria a coleção "DailyBalanceReport"
                await _database.CreateCollectionAsync("DailyBalanceReport");
            }
        }
    }
}
