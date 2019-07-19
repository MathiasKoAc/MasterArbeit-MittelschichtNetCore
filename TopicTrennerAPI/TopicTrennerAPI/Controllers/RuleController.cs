using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopicTrennerAPI.Models;
using TopicTrennerAPI.Data;
using TopicTrennerAPI.Interfaces;

namespace TopicTrennerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RuleController : ControllerBase
    {
        //Controller for Model typ SimpleRule

        readonly DbTopicTrennerContext _context;
        readonly IManageRuleService _ctrlRuleSession;

        public RuleController(DbTopicTrennerContext context, IManageRuleService ctrlRuleSession)
        {
            _context = context;
            _ctrlRuleSession = ctrlRuleSession;
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
        public IActionResult Post(SimpleRule sRule)
        {
            if (sRule.ID != 0 && _context.Rules.Find(sRule.ID) != null)
            {
                return BadRequest();
            }

            sRule.InTopic = sRule.InTopic?.ToLower();
            sRule.OutTopic = sRule.OutTopic?.ToLower();

            _context.Rules.Add(sRule);
            _context.SaveChanges();

            ReloadSessionRun(sRule.SessionID);
            return NoContent();
        }

        // PUT: api/Rule/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, SimpleRule sRule)
        {
            if (id != sRule.ID)
            {
                return BadRequest();
            }

            sRule.InTopic = sRule.InTopic?.ToLower();
            sRule.OutTopic = sRule.OutTopic?.ToLower();

            _context.Entry(sRule).State = EntityState.Modified;
            _context.SaveChanges();

            ReloadSessionRun(sRule.SessionID);
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
            var sessionId = rule.SessionID;

            _context.Rules.Remove(rule);
            await _context.SaveChangesAsync();

            ReloadSessionRun(sessionId);
            return NoContent();
        }

        private void ReloadSessionRun(int SessionId)
        {
            SessionRun sesRun = _context.SessionRuns.Where(s => s.Session.ID == SessionId).First();
            if (sesRun != null && sesRun.Active)
            {
                _ctrlRuleSession.ReloadRules(sesRun.ID);
            }
        }
    }
}
