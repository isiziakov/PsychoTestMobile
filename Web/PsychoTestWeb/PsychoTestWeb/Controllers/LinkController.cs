using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PsychoTestWeb.Models;
using Microsoft.AspNetCore.Authorization;
using System.Web;


namespace PsychoTestWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkController : ControllerBase
    {
        private readonly Service db;
        public LinkController(Service context)
        {
            db = context;
        }

        //генерация уникальной ссылки на привязку
        // GET api/<LinkController>/generateUrl/62a1f08829de97df5563051f
        [Authorize]
        [HttpGet("generateUrl/{id}")]
        public async Task<string> GenerateUrl(string id)
        {
            Patient p = await db.GetPatientById(id);
            p.token = db.GenerateToken();
            await db.UpdatePatient(id, p);
            return "ptest://https://" + this.HttpContext.Request.Host + "/api/link/t=" + p.token;
        }

        //получение уникальной ссылки на привязку
        // GET api/<LinkController>/getUrl/62a1f08829de97df5563051f
        [Authorize]
        [HttpGet("getUrl/{id}")]
        public async Task<string> GetUrl(string id)
        {
            Patient p = await db.GetPatientById(id);
            if (p.token == null)
            {
                p.token = db.GenerateToken();
                await db.UpdatePatient(id, p);
            }
            return "ptest://https://" + this.HttpContext.Request.Host + "/api/link/t=" + p.token;
        }

        //привязка по ссылке
        // GET api/<LinkController>/t={token}
        [HttpGet("{token}")]
        public async Task<IActionResult> Authentication(string token)
        {
            Patient p = await db.AuthenticationPatient(token.Remove(0, 2));
            if (p != null)
            {
                //перезаписываем токен, тем самым обеспечивая сгорание ссылки
                p.token = db.GenerateToken();
                await db.UpdatePatient(p.id, p);
                var domainName = this.HttpContext.Request.Host;
                var msg = new { token = p.token, domainName = "https://" + domainName + "/" };
                return Ok(msg);
            }
            else return null;
        }

        // POST api/<LinkController>
        [HttpPost]
        public async Task Post([FromBody] TestsResult value)
        {
            await db.ProcessingResults(value);
        }
    }
}
