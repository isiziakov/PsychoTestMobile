using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PsychoTestWeb.Models
{
    public class Patient
    {
        public Patient()
        {
            Tests = new HashSet<Test>();
            Results = new HashSet<Result>();
        }
        public string Name { get; set; }
        public int Id { get; set; }
        public virtual ICollection<Test> Tests { get; set; }
        public virtual ICollection<Result> Results { get; set; }

    }
}
