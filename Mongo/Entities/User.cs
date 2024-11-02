using MongoDB.Bson.Serialization.Attributes;

namespace Mongo.Entities
{
    public class User
    {
        [BsonId]
        public required Guid Id { get; set; }
        public required string Name { get; set; }
    }
}
