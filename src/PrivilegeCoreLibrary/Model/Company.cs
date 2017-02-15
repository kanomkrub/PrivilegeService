using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivilegeCoreLibrary.Model
{
    [BsonDiscriminator(RootClass = true)]
    public class Company
    {
        [BsonId]
        public string id { get; set; }
        public string name { get; set; }
        public string display_name { get; set; }
        public string description { get; set; }
        public byte[] logo { get; set; }
        public string url { get; set; }
        public string contact_addr { get; set; }
        public string contact_tel { get; set; }
        public string contact_line { get; set; }
        public string contact_facebook { get; set; }
        public string contact_twitter { get; set; }
        public string contact_email { get; set; }
    }
}
