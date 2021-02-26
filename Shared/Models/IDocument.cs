using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace Shared.Models
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        ObjectId Id { get; set; }

        DateTime CreatedAt { get; }
    }
}
