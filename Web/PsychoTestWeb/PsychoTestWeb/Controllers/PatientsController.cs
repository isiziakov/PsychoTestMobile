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
    public class PatientsController : ControllerBase
    {
        // тестовые данные вместо использования базы данных
        private List<Patient> people = new List<Patient>
        {
            new Patient {Name = "test1", Id = 1 },
            new Patient {Name = "test2", Id = 2 },
        };


        // GET: api/<PatientsController>
        [Authorize]
        [HttpGet]
        public IEnumerable<Patient> Get()
        {
            return people;
        }

        // GET api/<PatientsController>/5
        [Authorize]
        [HttpGet("{id}")]
        public Patient Get(int id)
        {
            return people.FirstOrDefault(x => x.Id == id);
        }

        // GET api/<PatientsController>/name/value
        [Authorize]
        [HttpGet("name/{value}")]
        public IEnumerable<Patient> Get(string value)
        {
            return people.FindAll(x => x.Name.Contains(value) == true);
        }

        // POST api/<PatientsController>
        [Authorize]
        [HttpPost]
        public void Post([FromBody] Patient value)
        {
        }

        // PUT api/<PatientsController>/5
        [Authorize]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Patient value)
        {
        }

        // DELETE api/<PatientsController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
