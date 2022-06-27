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
        public string date { get; set; }
        public List<Scale> scales { get; set; }
        public string comment { get; set; }
        public PatientsResult()
        {
            scales = new List<Scale>();
        }
        public class Scale
        {
            public string? idTestScale { get; set; }
            public string? idNormScale { get; set; }
            public string? name { get; set; }
            public int? scores { get; set; }
            public int? gradationNumber { get; set; }
            public string? interpretation { get; set; }
        }
    }
}
