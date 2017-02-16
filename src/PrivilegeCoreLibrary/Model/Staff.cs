using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivilegeCoreLibrary.Model
{
    /// <summary>
    /// company staff
    /// </summary>
    [BsonDiscriminator(RootClass = true)]
    public class Staff
    {
        [BsonId]
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string tel { get; set; }
        public string company { get; set; }
    }
}
