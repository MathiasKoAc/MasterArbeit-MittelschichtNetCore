using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TopicTrennerAPI.Models;
using TopicTrennerAPI.Data;

namespace TopicTrennerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        readonly DbTopicTrennerContext _context;

        public LogController(DbTopicTrennerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Log>> Get()
        {
            //TODO ggf platzt es hier weil zu viele Daten
            var sub = _context.Logs;
            return sub;
        }

        [HttpGet("{sessionRunId}")]
        public ActionResult<IEnumerable<Log>> Get(int sessionRunId)
        {
            if (_context.Logs.Count() > 0)
            {
                var Log = _context.Logs.Where(l => l.SessionRunID == sessionRunId).OrderByDescending(l => l.ID).ToList();
                return Log;
            }

            return null;
        }

    }
}
