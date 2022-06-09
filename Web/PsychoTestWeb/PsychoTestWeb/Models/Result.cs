using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PsychoTestWeb.Models
{
    public class Result
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string test { get; set; }
        public string result { get; set; }
        public string comment { get; set; }
    }
}
