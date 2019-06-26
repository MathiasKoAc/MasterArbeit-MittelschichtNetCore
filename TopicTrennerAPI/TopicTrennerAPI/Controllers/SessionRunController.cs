using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TopicTrennerAPI.Models;
using TopicTrennerAPI.Data;
using TopicTrennerAPI.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace TopicTrennerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionRunController : ControllerBase
    {
        DbTopicTrennerContext _context;
        IControlRules _ctrlRuleSession;

        public SessionRunController(DbTopicTrennerContext context, IControlRules ctrlRuleSession)
        {
            _context = context;
            _ctrlRuleSession = ctrlRuleSession;
        }

        // GET: api/SessionRun
        [HttpGet]
        public ActionResult<IEnumerable<SessionRun>> Get()
        {
            var sub = _context.SessionRuns;
            return sub;
        }

        // GET: api/SessionRun/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SessionRun>> Get(int id)
        {
            if (_context.SessionRuns.Count() > 0)
            {
                var sessionR = await _context.SessionRuns.FindAsync(id);
                return sessionR;
            }

            return null;
        }

        // POST: api/SessionRun
        [HttpPost]
        public void Post(SessionRun sessionR)
        {
            if (sessionR.Active)
            {
                SetAllInactiveActive();
            }
            _context.SessionRuns.Add(sessionR);
            _context.SaveChanges();

            if(sessionR.Active)
            {
                _ctrlRuleSession.StartRuleSession(sessionR.SessionID);
            }
        }

        // PUT: api/SessionRun/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, SessionRun sessionR)
        {
            if (id != sessionR.ID)
            {
                //we dont change IDs
                return BadRequest();
            }

            if (_context.SessionRuns.Count() <= 0)
            {
                return NotFound();
            }

            var oldSessionR = _context.SessionRuns.Find(id);

            if (sessionR.Active && !oldSessionR.Active)
            {
                SetAllInactiveActive();
            }
            _context.Entry(sessionR).State = EntityState.Modified;
            _context.SaveChanges();

            if(sessionR.Active && !oldSessionR.Active)
            {
                _ctrlRuleSession.StartRuleSession(sessionR.SessionID);
            }
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.SessionRuns.Count() > 0)
            {
                var sessionR = await _context.SessionRuns.FindAsync(id);
                if(sessionR == null)
                {
                    return NotFound();
                }

                sessionR.Active = false;
                if(sessionR.StopTime == null)
                {
                    sessionR.StopTime = DateTime.Now;
                }
                _context.Entry(sessionR).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return null;
        }

        private void SetAllInactiveActive()
        {
            var runs = _context.SessionRuns.Where(sr => sr.Active == true);
            foreach(SessionRun r in runs)
            {
                r.Active = false;
                if(r.StopTime == null)
                {
                    r.StopTime = DateTime.Now;
                }
            }
            _context.Entry(runs).State = EntityState.Modified;
        }
    }
}
