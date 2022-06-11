using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PsychoTestWeb.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PsychoTestWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly Service db;
        public ResultsController(Service context)
        {
            db = context;
        }

        // GET: api/<ResultsController>
        [Authorize]
        [HttpGet]
        public IEnumerable<Result> Get()
        {
            return db.GetResults().ToList();
        }

        // GET api/<ResultsController>/patientsResults/62a1f08829de97df5563051f
        [Authorize]
        [HttpGet("patientsResults/{patientId}")]
        public IEnumerable<Result> GetPatientsResults(string patientId)
        {
            return db.GetPatientsResults(patientId).ToList();
        }

        //GET api/<ResultsController>/62a1f08829de97df5563051f
        [Authorize]
        [HttpGet("{id}")]
        public Result Get(string id)
        {
            return db.GetResults().ToList().FirstOrDefault(x => x.id == id);
        }

        // PUT api/<ResultsController>/62a1f08829de97df5563051f
        [Authorize]
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] Result value)
        {
            db.UpdateResults(id, value);
        }

        // POST api/<ResultsController>/
        [Authorize]
        [HttpPost]
        public void Post(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ResultsController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
