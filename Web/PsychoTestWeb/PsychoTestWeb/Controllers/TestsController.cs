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
    public class TestsController : ControllerBase
    {
        private readonly Service db;
        List<Test> tests;
        public TestsController(Service context)
        {
            db = context;
            tests = db.GetTests().ToList();
        }

        // GET: api/<TestsController>
        [Authorize]
        [HttpGet]
        public IEnumerable<Test> Get()
        {
            return tests;
        }

        // GET api/<TestsController>/62a2ee61e5ab646eb9231448
        [Authorize]
        [HttpGet("{id}")]
        public Test Get(string id)
        {
            return tests.FirstOrDefault(x => x.id == id);
        }

        // POST api/<TestsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TestsController>/62a2ee61e5ab646eb9231448
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TestsController>/62a2ee61e5ab646eb9231448
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
