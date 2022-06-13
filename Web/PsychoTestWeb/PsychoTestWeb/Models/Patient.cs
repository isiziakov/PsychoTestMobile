using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PsychoTestWeb.Models
{
    public class Patient
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string name { get; set; }
        public List<string> tests { get; set; }
        public List<Result> results { get; set; }
    }
}
