using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using LittleGarden.Core.Entities;
using Pump.Core.Metrics;

namespace LittleGarden.Data
{
    public class DataContext<T> : IDataContext<T>
     where T : Entity
    {
        private readonly IMetricsServer metrics;

        public  DataContext(IMetricsServer metrics)
            {
            this.metrics = metrics;
        }
        public async Task Save(T entity)
        {
            var collection = GetCollection<T>(); 

            lock (this)
            {
                var result = collection.ReplaceOne(
                   filter: new BsonDocument("_id", entity._id),
                   options: new ReplaceOptions { IsUpsert = true },
                   replacement: entity);
                metrics.Inc($"pump_data_{typeof(T).Name}_updatedrecords", 1);
            }
        }

        public async Task<T> GetOne(string field, object value)
        {
            var collection = GetCollection<T>();            
            var filter = Builders<T>.Filter.Eq(field, value);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }
        private IMongoCollection<T> GetCollection<T>()
        {
                        var client = new MongoClient(
                "mongodb://root:example@localhost/"
            );
            var database = client.GetDatabase("LittleGarden");
            return database.GetCollection<T>(typeof(T).Name);
        }
    }
}