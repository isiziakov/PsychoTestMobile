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
        public async Task<IEnumerable<Test>> Get()
        {
            return await db.GetTestsView();
        }

        //получение всех тестов
        // GET: api/<TestsController>
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<string> GetTests()
        {
            return JsonConvert.SerializeObject(await db.GetTests());
        }

        //получение теста по id
        // GET api/<TestsController>/62a2ee61e5ab646eb9231448
        [HttpGet("{id}")]
        public async Task<string> Get(string id)
        {
            return await db.GetTestById(id);
        }

        //получение всех тестов пациента в формате id-название-заголовок-инструкция
        // GET api/<TestsController>/patient/e6tpm5eFvntJKtu1Eg1hm6hTpRi5cK0A70GgN7DEQaE
        [HttpGet("patient/{token}")]
        public async Task<IEnumerable<Test>> GetTestsByPatientId(string token)
        {
            return await db.GetTestsByPatientToken(token);
        }

        // POST api/<TestsController>
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task Post([FromForm] IFormFile value)
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
            }

        }

        // PUT api/<TestsController>/62a2ee61e5ab646eb9231448
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
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
