using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using LittleGarden.Core.Entities;
using Pump.Core.Metrics;
using Ppl.Core.Docker;

namespace LittleGarden.Data
{
    public class DataContext<T> : IDataContext<T>
     where T : Entity
    {
        private readonly IMetricsServer metrics;

        public ContainerParameters Parameters { get; }

        public  DataContext(IMetricsServer metrics, ContainerParameters parameters)
            {
            this.metrics = metrics;
            Parameters = parameters;
        }
        public async Task Save(T entity)
        {
            var collection = GetCollection(); 

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
            var collection = GetCollection();            
            var filter = Builders<T>.Filter.Eq(field, value);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAll(PageConfig page)
        {
            var collection = GetCollection();
            return await collection.Find(_ => true)
                .Skip((page.Page-1)*page.PageSize)
                .Limit(page.PageSize)
                .ToListAsync();
        }

        private IMongoCollection<T> GetCollection()
        {
                        var client = new MongoClient(Parameters.GetStringParameter("MongoDBConnectionString")
            );
            var database = client.GetDatabase("LittleGarden");
            return database.GetCollection<T>(typeof(T).Name);
        }
    }
}