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
    public class SubjectRuleController : ControllerBase
    {
        readonly DbTopicTrennerContext _context;

        public SubjectRuleController(DbTopicTrennerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<SimpleRuleSubject>> Get()
        {
            var sub = _context.SimpleRuleSubjects.ToList();
            return sub;
        }

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

        [HttpPost]
        public IActionResult Post(SimpleRuleSubject sSimpleRuleSubject)
        {
            if (sSimpleRuleSubject.ID != 0 && _context.SimpleRuleSubjects.Find(sSimpleRuleSubject.ID) != null)
            {
                return BadRequest();
            }

            _context.SimpleRuleSubjects.Add(sSimpleRuleSubject);
            _context.SaveChangesAsync();

            return NoContent();
        }

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
