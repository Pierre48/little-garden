using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LittleGarden.Core.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using Ppl.Core.Container;
using Pump.Core.Metrics;

namespace LittleGarden.Data
{
    public class DataContext<T> : IDataContext<T>
        where T : IEntity
    {
        private readonly IMetricsServer _metrics;

        public DataContext(IMetricsServer metrics, ContainerParameters parameters)
        {
            this._metrics = metrics;
            Parameters = parameters;
        }

        private ContainerParameters Parameters { get; }

        public async Task Save(T entity, Expression<Func<T,bool>> filter)
        {
            
            /// All locks are needed ???
            var collection = GetCollection();
                T document = default(T);
            lock(this)
                document = collection.Find(filter).FirstOrDefault();
            if (document == null)
            {
                lock(this)
                collection.InsertOne(entity, new InsertOneOptions { });
                _metrics.Inc($"pump_data_{typeof(T).Name}_inserted_records", 1);
            }
            else
            {
                entity._id = document._id;
                collection.ReplaceOne<T>(filter,entity);
                _metrics.Inc($"pump_data_{typeof(T).Name}_updated_records", 1);
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
                .Skip((page.Page - 1) * page.PageSize)
                .Limit(page.PageSize)
                .ToListAsync();
        }

        public async Task<bool> Create(T entity, Expression<Func<T,bool>> filter)
        {
            var collection = GetCollection();
            T document = default(T);
            lock(this)
                document = collection.Find(filter).FirstOrDefault();
            if (document == null)
            {
                lock(this)
                    collection.InsertOne(entity, new InsertOneOptions { });
                _metrics.Inc($"pump_data_{typeof(T).Name}_inserted_records", 1);
                return true;
            }
            return false;
        }

        public async Task<string[]> GetIds(Expression<Func<T, bool>> filter)
        {
            var collection = GetCollection();
            var result = await collection.FindAsync(filter);
            var list = new List<String>();
            while (result.MoveNext())
            {
                list.AddRange(result.Current.Select(x => (x._id.ToString())));
            } 

            return list.ToArray();
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