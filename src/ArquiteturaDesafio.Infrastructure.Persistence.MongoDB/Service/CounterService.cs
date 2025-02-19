using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArquiteturaDesafio.Infrastructure.Persistence.MongoDB.Model;
namespace ArquiteturaDesafio.Infrastructure.Persistence.MongoDB.Service
{
    public class CounterService
    {
        private readonly IMongoCollection<Counter> _countersCollection;

        public CounterService(IMongoDatabase database)
        {
            _countersCollection = database.GetCollection<Counter>("Counters");
        }

        public async Task<int> GetNextSequenceValueAsync(string collectionName)
        {
            var filter = Builders<Counter>.Filter.Eq(c => c.Id, collectionName);
            var update = Builders<Counter>.Update.Inc(c => c.SequenceValue, 1);
            var options = new FindOneAndUpdateOptions<Counter>
            {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = true // Cria o documento se não existir
            };

            var counter = await _countersCollection.FindOneAndUpdateAsync(filter, update, options);
            return counter.SequenceValue;
        }
    }
}
