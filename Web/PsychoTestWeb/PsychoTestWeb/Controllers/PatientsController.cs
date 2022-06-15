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
        public PatientsController(Service context)
        {
            db = context;
        }

        // GET: api/<PatientsController>
        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<Patient>> Get()
        {
            return await db.GetPatients();
        }

        // GET api/<PatientsController>/62a1f08829de97df5563051f
        [Authorize]
        [HttpGet("{id}")]
        public async Task<Patient> Get(string id)
        {
            return await db.GetPatientById(id);
        }

        // GET api/<PatientsController>/name/value
        [Authorize]
        [HttpGet("name/{value}")]
        public async Task<IEnumerable<Patient>> GetByName(string value)
        {
            return await db.GetPatientsByName(value);
        }

        // POST api/<PatientsController>
        [Authorize]
        [HttpPost]
        public async Task Post([FromBody] Patient value)
        {
            await db.CreatePatient(value);
        }

        // PUT api/<PatientsController>/62a1f08829de97df5563051f
        [Authorize]
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] Patient value)
        {
            await db.UpdatePatient(id, value);
        }

        // DELETE api/<PatientsController>/62a1f08829de97df5563051f
        [Authorize]
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await db.RemovePatient(id);
        }
    }
}
