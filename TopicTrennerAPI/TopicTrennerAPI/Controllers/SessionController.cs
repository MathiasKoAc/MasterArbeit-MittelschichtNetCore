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
    public class SessionController : ControllerBase
    {
        DbTopicTrennerContext _context;

        public SessionController(DbTopicTrennerContext context)
        {
            _context = context;
        }

        // GET: api/Session
        [HttpGet]
        public ActionResult<IEnumerable<Session>> Get()
        {
            var sub = _context.Sessions;
            return sub;
        }

        // GET: api/Session/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Session>> Get(int id)
        {
            if (_context.Sessions.Count() > 0)
            {
                var Session = await _context.Sessions.FindAsync(id);//TodoItems.FindAsync(id);
                return Session;
            }

            return null;
        }

        // POST: api/Session
        [HttpPost]
        public void Post(Session sSession)
        {
            try
            {
                _context.Sessions.Add(sSession);
                _context.SaveChanges();
            }
            catch (DbUpdateException eu) 
            {
                var streu = eu.ToString();
            }
        }

        // PUT: api/Session/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Session sSession)
        {
            if(id != sSession.ID)
            {
                return BadRequest();
            }

            _context.Entry(sSession).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var Session = await _context.Sessions.FindAsync(id);

            if (Session == null)
            {
                return NotFound();
            }

            _context.Sessions.Remove(Session);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
