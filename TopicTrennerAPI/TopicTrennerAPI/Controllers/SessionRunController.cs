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
        readonly IManageRuleService _manageRuleSevice;
        readonly IManageTimeService _manageTimeSevice;
        readonly IManageLogService _manageLogService;

        public SessionRunController(DbTopicTrennerContext context, IManageRuleService ctrlRuleSession, IManageTimeService controlTimeSession, IManageLogService logService)
        {
            _context = context;
            _manageRuleSevice = ctrlRuleSession;
            _manageTimeSevice = controlTimeSession;
            _manageLogService = logService;
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
        public IActionResult Post(SessionRun sessionR)
        {
            //TODO umbauen rückgabe von START/STOP einbauen
            if(sessionR.ID != null && _context.SessionRuns.Find(sessionR.ID) != null)
            {
                return BadRequest();
            }

            if (sessionR.Active)
            {
                SetAllInactiveActive();
            }
            _context.SessionRuns.Add(sessionR);
            _context.SaveChanges();

            if(sessionR.Active)
            {
                StartSessionRun(sessionR.ID);
            }
            return NoContent();
        }

        // PUT: api/SessionRun/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, SessionRun sessionR)
        {
            //TODO umbauen rückgabe von START/STOP einbauen

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
                StartSessionRun(sessionR.ID);
            }
            else if(!sessionR.Active)
            {
                StopSessionRun(sessionR.ID);
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
            //TODO umbauen rückgabe von START/STOP einbauen

            if (_context.SessionRuns.Count() > 0)
            {
                var sessionR = await _context.SessionRuns.FindAsync(id);
                if(sessionR == null)
                {
                    return NotFound();
                }

                if(sessionR.Active)
                {
                    StopSessionRun(sessionR.ID);
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

        private void StartSessionRun(int id)
        {
            _manageRuleSevice.StartRuleSession(id);
            _manageTimeSevice.StartTimeService(id);
            _manageLogService.StartLogService(id);
        }

        private void StopSessionRun(int id)
        {
            _manageRuleSevice.StopRuleSession(id);
            _manageTimeSevice.StopTimeService(id);
            _manageLogService.StopLogService(id);
        }

        private void SetAllInactiveActive()
        {
            var runs = _context.SessionRuns.Where(sr => sr.Active == true);
            foreach (SessionRun r in runs)
            {
                r.Active = false;
                StopSessionRun(r.ID);
                if (r.StopTime == null)
                {
                    r.StopTime = DateTime.Now;
                }
            }
            _context.SaveChanges();
        }
    }
}
