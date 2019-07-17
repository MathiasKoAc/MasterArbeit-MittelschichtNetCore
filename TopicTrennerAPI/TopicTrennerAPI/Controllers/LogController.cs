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

        [HttpGet("{id}")]
        public async Task<ActionResult<Log>> Get(int id)
        {
            if (_context.Logs.Count() > 0)
            {
                var Log = await _context.Logs.FindAsync(id);
                return Log;
            }

            return null;
        }

        // POST: api/Session
        [HttpPost]
        public IActionResult Post(Log l)
        {
            if(l.ID != 0 && _context.Logs.Find(l.ID) != null)
            {
                return BadRequest();
            }

            _context.Logs.Add(l);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Log l)
        {
            if(id != l.ID)
            {
                return BadRequest();
            }

            _context.Entry(l).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var l = await _context.Logs.FindAsync(id);

            if (l == null)
            {
                return NotFound();
            }

            _context.Logs.Remove(l);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
