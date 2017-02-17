using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivilegeCoreLibrary.Model
{
    [BsonDiscriminator(RootClass = true)]
    public class Promotion
    {
        [BsonId]
        public string id { get; set; }
        public string company { get; set; }
        public string name { get; set; }
        public string display_name { get; set; }
        public string description { get; set; }
        public string condition { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string[] image_main { get; set; }
        public string image_small { get; set; }
    }
}
