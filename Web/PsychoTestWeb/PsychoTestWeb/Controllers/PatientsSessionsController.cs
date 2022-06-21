using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PsychoTestWeb.Models;
using Microsoft.AspNetCore.Authorization;

namespace PsychoTestWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsSessionsController : ControllerBase
    {
        private readonly Service db;
        public PatientsSessionsController(Service context)
        {
            db = context;
        }

        //генерация уникальной ссылки на привязку
        // GET api/<PatientsSessionsController>/generateUrl/62a1f08829de97df5563051f
        [Authorize]
        [HttpGet("generateUrl/{id}")]
        public async Task<string> GenerateUrl(string id)
        {
            Patient p = await db.GetPatientById(id);
            p.token = db.GenerateToken();
            await db.UpdatePatient(id, p);
            return "/api/PatientsSessions/authentication/" + p.token;
        }

        //получение уникальной ссылки на привязку
        // GET api/<PatientsSessionsController>/getUrl/62a1f08829de97df5563051f
        [Authorize]
        [HttpGet("getUrl/{id}")]
        public async Task<string> GetUrl(string id)
        {
            Patient p = await db.GetPatientById(id);
            if (p.token != null)
                return "/api/PatientsSessions/authentication/" + p.token;
            else
            {
                p.token = db.GenerateToken();
                await db.UpdatePatient(id, p);
                return "/api/PatientsSessions/authentication/" + p.token;
            }
        }

        //привязка по ссылке
        // GET api/<PatientsSessionsController>/authentication/{token}
        [HttpGet("authentication/{token}")]
        public async Task<string> Authentication(string token)
        {
            Patient p = await db.AuthenticationPatient(token);
            if (p != null)
            {
                //перезаписываем токен, тем самым обеспечивая сгорание ссылки
                p.token = db.GenerateToken();
                await db.UpdatePatient(p.id, p);
                return p.token;
            }
            else return null;
        }

        // POST api/<PatientsSessionsController>
        [HttpPost]
        public async Task Post([FromBody] TestsResult value)
        {
            await db.ProcessingResults(value);
        }
    }
}
