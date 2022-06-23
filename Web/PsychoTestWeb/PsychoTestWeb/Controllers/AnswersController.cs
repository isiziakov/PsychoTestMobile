using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PsychoTestWeb.Models;
using Microsoft.AspNetCore.Authorization;
using System.Web;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PsychoTestWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly Service db;
        public AnswersController(Service context)
        {
            db = context;
        }

        // POST api/<AnswersController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TestsResult value)
        {
            if (this.HttpContext.Request.Headers["Authorization"].ToString() == null)
                return Unauthorized();
            else
            {
                string token = this.HttpContext.Request.Headers["Authorization"].ToString();
                Patient patient = await db.GetPatientByToken(token);
                if (patient == null)
                    return Forbid();
                else
                {
                    //Рассчет баллов
                    await db.ProcessingResults(value, patient);

                    //расшифровка 
                    //...
                }
            }

            return Ok();
        }
    }
}
