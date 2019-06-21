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
    public class RuleController : ControllerBase
    {
        DbTopicTrennerContext _context;

        public RuleController(DbTopicTrennerContext context)
        {
            _context = context;
        }

        // GET: api/Rule
        [HttpGet]
        public ActionResult<IEnumerable<SimpleRule>> Get()
        {
            var todoItem = _context.Rules;

            return todoItem;
        }

        // GET: api/Rule/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SimpleRule>> Get(int id)
        {
            if (_context.Rules.Count() > 0)
            {
                var rule = await _context.Rules.FindAsync(id);//TodoItems.FindAsync(id);
                return rule;
            }

            return null;
        }

        // POST: api/Rule
        [HttpPost]
        public void Post(SimpleRule sRule)
        {
            _context.Rules.Add(sRule);
            _context.SaveChangesAsync();
        }

        // PUT: api/Rule/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, SimpleRule sRule)
        {
            if(id != sRule.ID)
            {
                return BadRequest();
            }

            _context.Entry(sRule).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var rule = await _context.Rules.FindAsync(id);

            if (rule == null)
            {
                return NotFound();
            }

            _context.Rules.Remove(rule);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
