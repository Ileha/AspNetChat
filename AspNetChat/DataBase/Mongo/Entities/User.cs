using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AspNetChat.DataBase.Mongo.Entities
{
    public class User
    {
        [BsonId]
		[BsonGuidRepresentation(GuidRepresentation.Standard)]
        public required Guid Id { get; set; }
        public required string Name { get; set; }
    }
}
