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
    public class SessionRuleController : ControllerBase
    {
        DbTopicTrennerContext _context;

        public SessionRuleController(DbTopicTrennerContext context)
        {
            _context = context;
        }

        // GET: api/SessionRule
        [HttpGet]
        public ActionResult<IEnumerable<SessionSimpleRule>> Get()
        {
            var sub = _context.SessionSimpleRules;
            return sub;
        }

        // GET: api/SessionRule/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SessionSimpleRule>> Get(int id)
        {
            if (_context.SessionSimpleRules.Count() > 0)
            {
                var SessionRule = await _context.SessionSimpleRules.FindAsync(id);//TodoItems.FindAsync(id);
                return SessionRule;
            }

            return null;
        }

        // POST: api/SessionRule
        [HttpPost]
        public void Post(SessionSimpleRule sSessionRule)
        {
            _context.SessionSimpleRules.Add(sSessionRule);
            _context.SaveChangesAsync();
        }

        // PUT: api/SessionRule/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, SessionSimpleRule sSessionRule)
        {
            if(id != sSessionRule.ID)
            {
                return BadRequest();
            }

            _context.Entry(sSessionRule).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var SessionRule = await _context.SessionSimpleRules.FindAsync(id);

            if (SessionRule == null)
            {
                return NotFound();
            }

            _context.SessionSimpleRules.Remove(SessionRule);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
