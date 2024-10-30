using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mongo.Entities
{
    public class User
    {
  //       [BsonId]
		// [BsonGuidRepresentation(GuidRepresentation.Standard)]
        [Key]
        [BsonId]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public required Guid Id { get; set; }
        public required string Name { get; set; }
    }
}
