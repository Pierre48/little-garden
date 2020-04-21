using MongoDB.Bson;

namespace LittleGarden.Core.Entities
{
    public class Interest: IEntity
    {
        public ObjectId _id { get; set; }
        public string Name { get; set; }
    }
}