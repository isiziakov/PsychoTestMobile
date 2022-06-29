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
        //получение теста по id без изображений
        // GET api/<TestsController>/c/62a2ee61e5ab646eb9231448
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWithoutImages(string id)
        {
            string test = await db.GetTestByIdWithoutImages(id);
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

        // POST api/<TestsController>/importTests
        [Authorize(Roles = "admin")]
        [Route("importTests")]
        [HttpPost]
        public async Task<IActionResult> PostTest([FromForm] IFormFile testFile, IFormFile normFile, List<IFormFile> images)
        {
            if (testFile != null && normFile != null)
            {
                var testRresult = new StringBuilder();
                using (var r = new StreamReader(testFile.OpenReadStream()))
                {
                    while (r.Peek() >= 0)
                        testRresult.AppendLine(r.ReadLine());
                }
                string testId = await db.ImportTestFile(testRresult.ToString());
                if (testId == null)
                {
                    return BadRequest(new { errorText = "Данный тест уже добавлен! \n" });
                }
                else
                {
                    var normRresult = new StringBuilder();
                    using (var r = new StreamReader(normFile.OpenReadStream()))
                    {
                        while (r.Peek() >= 0)
                            normRresult.AppendLine(r.ReadLine());
                    }
                    await db.ImportNormFile(normRresult.ToString(), testId);

                    foreach (IFormFile image in images)
                    {
                        using (Stream fs = image.OpenReadStream())
                        {
                            await db.ImportImage(fs, image.FileName);
                        }
                    }

                    return Ok();
                }
            }
            return BadRequest();
        }

        //УБРАТЬ!!!
        // POST api/<TestsController>/importImage
        [Route("importImage")]
        [HttpPost]
        public async Task<IActionResult> PostImages([FromForm] List<IFormFile> images)
        {
            if (images != null)
            { 
                foreach (IFormFile image in images)
                {
                    using (Stream fs = image.OpenReadStream())
                    {
                        await db.ImportImage(fs, image.FileName);
                    }
                }

                return Ok();
            }
            else
                return BadRequest();
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
