using Newtonsoft.Json.Linq;
using PsychoTestAndroid.DataBase.Entity;

namespace PsychoTestAndroid.Model
{
    public class Norm
    {
        public string Id { get; set; }

        public JObject Data { get; set; }

        public Norm()
        {

        }

        public Norm(DbScale scale)
        {
            Id = scale.TestId;
            Data = JObject.Parse(scale.Scales);
        }
    }
}