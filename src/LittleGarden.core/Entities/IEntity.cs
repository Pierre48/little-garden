using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LittleGarden.Core.Entities
{
    public interface IEntity
    {
        ObjectId _id { get; set; }
    }
}