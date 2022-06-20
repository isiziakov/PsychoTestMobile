using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace PsychoTestWeb.Models
{
    public class User
    {

        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string name { get; set; }
    }
}
