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
        readonly DbTopicTrennerContext _context;
        readonly IManageRuleService _ctrlRuleSession;
        readonly IControlTimeSession _ctrlTimeSession;

        public SessionRunController(DbTopicTrennerContext context, IManageRuleService ctrlRuleSession, IControlTimeSession controlTimeSession)
        {
            _context = context;
            _ctrlRuleSession = ctrlRuleSession;
            _ctrlTimeSession = controlTimeSession;
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
                int sessId = sessionR.SessionID;
                _ctrlRuleSession.StartRuleSession(sessId);
                _ctrlTimeSession.StartTimeService(sessId);
            }
        }

        // PUT: api/SessionRun/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, SessionRun sessionR)
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

            if (sessionR.Active)
            {
                SetAllInactiveActive();
                _ctrlRuleSession.StartRuleSession(sessionR.SessionID);
                _ctrlTimeSession.StartTimeService(sessionR.SessionID);
            }
            else if(!sessionR.Active)
            {
                _ctrlRuleSession.StopRuleSession(sessionR.SessionID);
                _ctrlTimeSession.StopTimeService(sessionR.SessionID);
            }

            oldSessionR.Active = sessionR.Active;
            if(sessionR.StopTime != null)
            {
                oldSessionR.StopTime = sessionR.StopTime;
            }
            _context.SaveChanges();
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

                if(sessionR.Active)
                {
                    _ctrlRuleSession.StopRuleSession(sessionR.SessionID);
                    _ctrlTimeSession.StopTimeService(sessionR.SessionID);
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
            foreach (SessionRun r in runs)
            {
                r.Active = false;
                _ctrlRuleSession.StopRuleSession(r.SessionID);
                if (r.StopTime == null)
                {
                    r.StopTime = DateTime.Now;
                }
            }
            _context.SaveChanges();
        }
    }
}
