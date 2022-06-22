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
        private readonly Service db;
        public PatientsController(Service context)
        {
            db = context;
        }

        //получение всех пациентов
        // GET: api/<PatientsController>
        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<Patient>> Get()
        {
            return await db.GetPatients();
        }

        //получение пациента по id
        // GET api/<PatientsController>/62a1f08829de97df5563051f
        [Authorize]
        [HttpGet("{id}")]
        public async Task<Patient> Get(string id)
        {
            return await db.GetPatientById(id);
        }

        //получение общего количества страниц с пациентами
        // GET: api/<PatientsController>/pageCount
        [Authorize]
        [Route("pageCount")]
        [HttpGet]
        public async Task<double> GetPagesCount()
        {
            return await db.GetPatientsPagesCount();
        }

        //получение списка пациентов на конкретной странице
        // GET api/<PatientsController>/page/3
        [Authorize]
        [HttpGet("page/{value}")]
        public async Task<IEnumerable<Patient>> GetWithCount(int value)
        {
            return await db.GetPatientsWithCount(value);
        }

        //получение пациентов с подстрокой value в имени
        // GET api/<PatientsController>/name/value
        [Authorize]
        [HttpGet("name/{value}")]
        public async Task<IEnumerable<Patient>> GetByName(string value)
        {
            return await db.GetPatientsByName(value);
        }

        //получение общего количества страниц с пациентами c фильтрацией по имени
        // GET: api/<PatientsController>/name/pageCount/value
        [Authorize]
        [HttpGet("name/pageCount/{value}")]
        public async Task<double> GetByNamePagesCount(string value)
        {
            return await db.GetPatientsByNamePagesCount(value);
        }

        //получение списка пациентов на конкретной странице с фильтрацией по имени
        // GET api/<PatientsController>/name/page/3/value
        [Authorize]
        [HttpGet("name/page/{pageValue}/{nameValue}")]
        public async Task<IEnumerable<Patient>> GetByNameWithCount(int pageValue, string nameValue)
        {
            return await db.GetPatientsByNameWithCount(pageValue, nameValue);
        }

        // POST api/<PatientsController>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Patient value)
        {
            string token = await db.CreatePatient(value);
            if (token != null)
            {
                var msg = new { message = "/api/PatientsSessions/authentication/" + token };
                return Ok(msg);
            }
            else return null;
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
