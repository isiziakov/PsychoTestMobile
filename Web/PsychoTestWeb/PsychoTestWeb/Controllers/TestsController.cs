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
using MongoDB.Bson;
using Newtonsoft.Json;

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

        //получение всех тестов в формате id-название-заголовок-инструкция
        // GET: api/<TestsController>/view
        [Route("view")]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            IEnumerable<Test> list = await db.GetTestsView();
            if (list != null)
                return Ok(list);
            else return NoContent();
        }

        //получение теста по id
        // GET api/<TestsController>/62a2ee61e5ab646eb9231448
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            string test = await db.GetTestById(id);
            if (test != null)
                return Ok(test);
            else return NoContent();
        }

        //получение всех тестов пациента в формате id-название-заголовок-инструкция
        // GET api/<TestsController>/
        [HttpGet]
        public async Task<IActionResult> GetTestsByPatientId()
        {
            string token;
            if (this.HttpContext.Request.Headers["Authorization"].ToString() == null)
                return Unauthorized();
            else

            {
                token = this.HttpContext.Request.Headers["Authorization"].ToString();
                Patient patient = await db.GetPatientByToken(token);
                if (patient == null)
                    return Forbid();
                else
                    return Ok(await db.GetTestsByPatientToken(patient));
            }
        }

        // POST api/<TestsController>
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] IFormFile value)
        {
            if (value != null)
            {
                var result = new StringBuilder();
                using (var r = new StreamReader(value.OpenReadStream()))
                {
                    while (r.Peek() >= 0)
                        result.AppendLine(r.ReadLine());
                }
                await db.ImportFile(result.ToString());
                return Ok();
            }
            else return BadRequest();
        }

        // DELETE api/<TestsController>/62a2ee61e5ab646eb9231448
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await db.RemoveTest(id);
        }
    }
}
