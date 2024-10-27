﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mongo.Entities
{
    public class User
    {
        [BsonId]
		[BsonGuidRepresentation(GuidRepresentation.Standard)]
        public required Guid Id { get; set; }
        public required string Name { get; set; }
    }
}