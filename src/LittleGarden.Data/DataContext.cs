using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using LittleGarden.Core.Entities;

namespace LittleGarden.Data
{
    public class DataContext<T> : IDataContext<T>
     where T:Entity
    {
        public async Task Save(T entity)
        {
            var collection = GetCollection<T>(); 

            lock (this)
            {
                collection.ReplaceOne(
                   filter: new BsonDocument("_id", entity._id),
                   options: new ReplaceOptions { IsUpsert = true },
                   replacement: entity);
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