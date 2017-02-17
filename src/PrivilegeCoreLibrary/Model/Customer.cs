using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivilegeCoreLibrary.Model
{
    [BsonDiscriminator(RootClass = true)]
    public class Customer
    {
        [BsonId]
        public string id { get; set; }
        public string email { get; set; }
        public int point { get; set; }
        public object[] point_history { get; set; }
        public object[] coupon { get; set; }
        public string[] favourite { get; set; }
    }
}
