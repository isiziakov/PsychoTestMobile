using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PsychoTestWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Xml;
using System.IO;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PsychoTestWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly Service db;
        public TestsController(Service context)
        {
            db = context;
        }

        // GET: api/<TestsController>
        [Authorize]
        [HttpGet]
        public IEnumerable<Test> Get()
        {
            return db.GetTests().ToList();
        }

        // GET api/<PatientsController>/name/value
        [Authorize]
        [HttpGet("name/{value}")]
        public IEnumerable<Test> GetByName(string value)
        {
            return db.GetTestsByName(value);
        }

        // GET api/<TestsController>/62a2ee61e5ab646eb9231448
        [Authorize]
        [HttpGet("{id}")]
        public Test Get(string id)
        {
            return db.GetTests().ToList().FirstOrDefault(x => x.id == id);
        }

        // POST api/<TestsController>
        //[Authorize]
        [HttpPost]
        public void Post([FromForm] IFormFile value)
        {
            var result = new StringBuilder();
            using (var r = new StreamReader(value.OpenReadStream()))
            {
                while (r.Peek() >= 0)
                    result.AppendLine(r.ReadLine());
            }
            db.ImportFile(result.ToString());
        }

        // PUT api/<TestsController>/62a2ee61e5ab646eb9231448
        [Authorize]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TestsController>/62a2ee61e5ab646eb9231448
        [Authorize]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
