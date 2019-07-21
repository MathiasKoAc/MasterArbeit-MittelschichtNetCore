using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopicTrennerAPI.Data;
using TopicTrennerAPI.Interfaces;
using TopicTrennerAPI.Models;

namespace TopicTrennerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeController : ControllerBase
    {
        readonly DbTopicTrennerContext _context;
        readonly IFindSessionRun _ctrlSessionRun;

        public TimeController(DbTopicTrennerContext context, IFindSessionRun findSessionRun)
        {
            _context = context;
            _ctrlSessionRun = findSessionRun;
        }

        [HttpGet]
        public ActionResult<DateTime> Get()
        {
            int sessionRunId = _ctrlSessionRun.GetActivSessionRun();
            if(sessionRunId != int.MinValue)
            {
                TimeDiff diff = _context.TimeDiffs.Find(sessionRunId);

                if (diff != null)
                {
                    return DateTime.Now + diff.Diff;
                }
            }

            return DateTime.Now;
        }

        [HttpGet("{id}")]
        public ActionResult<DateTime> Get(int id)
        {
            if (_context.TimeDiffs.Count() > 0)
            {
                TimeDiff diff = _context.TimeDiffs.Find(id);

                if (diff != null)
                {
                    return DateTime.Now + diff.Diff;
                }
            }

            return DateTime.Now;
        }
    }
}
