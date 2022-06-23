using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PsychoTestWeb.Models
{
    public class PatientsResult
    {
        public string test { get; set; }
        public int result { get; set; }
        public string comment { get; set; }
    }
}
