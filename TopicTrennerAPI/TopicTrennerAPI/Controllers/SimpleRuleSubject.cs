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
    public class SimpleRuleSubjectController : ControllerBase
    {
        DbTopicTrennerContext _context;

        public SimpleRuleSubjectController(DbTopicTrennerContext context)
        {
            _context = context;
        }

        // GET: api/SimpleRuleSubject
        [HttpGet]
        public ActionResult<IEnumerable<SimpleRuleSubject>> Get()
        {
            var sub = _context.SimpleRuleSubjects;
            return sub;
        }

        // GET: api/SimpleRuleSubject/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SimpleRuleSubject>> Get(int id)
        {
            if (_context.SimpleRuleSubjects.Count() > 0)
            {
                var SimpleRuleSubject = await _context.SimpleRuleSubjects.FindAsync(id);//TodoItems.FindAsync(id);
                return SimpleRuleSubject;
            }

            return null;
        }

        // POST: api/SimpleRuleSubject
        [HttpPost]
        public void Post(SimpleRuleSubject sSimpleRuleSubject)
        {
            _context.SimpleRuleSubjects.Add(sSimpleRuleSubject);
            _context.SaveChangesAsync();
        }

        // PUT: api/SimpleRuleSubject/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, SimpleRuleSubject sSimpleRuleSubject)
        {
            if(id != sSimpleRuleSubject.ID)
            {
                return BadRequest();
            }

            _context.Entry(sSimpleRuleSubject).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var SimpleRuleSubject = await _context.SimpleRuleSubjects.FindAsync(id);

            if (SimpleRuleSubject == null)
            {
                return NotFound();
            }

            _context.SimpleRuleSubjects.Remove(SimpleRuleSubject);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
