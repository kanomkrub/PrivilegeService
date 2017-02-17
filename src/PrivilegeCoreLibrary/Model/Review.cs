using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivilegeCoreLibrary.Model
{
    [BsonDiscriminator(RootClass = true)]
    public class Review
    {
        [BsonId]
        public string id { get; set; }
    }
}
