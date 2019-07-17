using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TopicTrennerAPI.Models;
using TopicTrennerAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace TopicTrennerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionRunLogController : ControllerBase
    {
        readonly DbTopicTrennerContext _context;

        public SessionRunLogController(DbTopicTrennerContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public ActionResult<ICollection<Log>> Get(int id)
        {
            if (_context.Logs.Count() > 0)
            {
                var Logs = _context.Logs.Where(l => l.SessionRunID == id).ToList();
                return Logs;
            }

            return new List<Log>();
        }
    }
}
