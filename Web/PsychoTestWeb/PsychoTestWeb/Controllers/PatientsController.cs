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
        //private List<Patient> people = new List<Patient>
        //{
        //    new Patient {Name = "test1", Id = 1 },
        //    new Patient {Name = "test2", Id = 2 },
        //};
        private readonly Service db;
        List<Patient> people;
        public PatientsController(Service context)
        {
            db = context;
            people = db.GetPatients().ToList();
        }

        // GET: api/<PatientsController>
        [Authorize]
        [HttpGet]
        public IEnumerable<Patient> Get()
        {
            return people;
        }

        // GET api/<PatientsController>/62a1f08829de97df5563051f
        [Authorize]
        [HttpGet("{id}")]
        public Patient Get(string id)
        {
            return db.GetPatientById(id);
        }

        // GET api/<PatientsController>/name/value
        [Authorize]
        [HttpGet("name/{value}")]
        public IEnumerable<Patient> GetByName(string value)
        {
            return db.GetPatientsByName(value);
        }

        // POST api/<PatientsController>
        [Authorize]
        [HttpPost]
        public void Post([FromBody] Patient value)
        {
            db.CreatePatient(value);
        }

        // PUT api/<PatientsController>/62a1f08829de97df5563051f
        [Authorize]
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] Patient value)
        {
            db.UpdatePatient(id, value);
        }

        // DELETE api/<PatientsController>/62a1f08829de97df5563051f
        [Authorize]
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            db.RemovePatient(id);
        }
    }
}
