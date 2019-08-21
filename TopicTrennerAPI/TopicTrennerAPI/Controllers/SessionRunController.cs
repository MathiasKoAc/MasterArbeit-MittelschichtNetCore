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
        readonly IManageEventService _manageEventService;

        public SessionRunController(DbTopicTrennerContext context, IManageRuleService ctrlRuleSession,
                                    IManageTimeService controlTimeSession, IManageLogService logService,
                                    IManageEventService eventService)
        {
            _context = context;
            _manageRuleSevice = ctrlRuleSession;
            _manageTimeSevice = controlTimeSession;
            _manageLogService = logService;
            _manageEventService = eventService;
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
            if(sessionR.ID != 0 && _context.SessionRuns.Find(sessionR.ID) != null)
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

            if (sessionR.Active)
            {
                var runs = _context.SessionRuns;
                foreach (SessionRun r in runs)
                {
                    if (r.Active)
                    {
                        r.Active = false;
                        StopSessionRun(r.ID);
                        if (r.StopTime == null)
                        {
                            r.StopTime = DateTime.Now;
                        }
                    }

                    if(r.ID == sessionR.ID)
                    {
                        r.Beschreibung = sessionR.Beschreibung;
                        r.SessionID = sessionR.SessionID;
                        r.StartTime = sessionR.StartTime;
                        r.Active = sessionR.Active;
                    }                        
                }
                StartSessionRun(sessionR.ID);
            }
            else
            {
                _context.Entry(sessionR).State = EntityState.Modified;
                StopSessionRun(sessionR.ID);
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
                
                _context.SessionRuns.Remove(sessionR);
                _context.SaveChanges();
            }

            return NoContent();
        }

        private void StartSessionRun(int id)
        {
            _manageRuleSevice.StartRuleSession(id);
            _manageTimeSevice.StartTimeService(id);
            _manageLogService.StartLogService(id);
            _manageEventService.StartEventService(id);
        }

        private void StopSessionRun(int id)
        {
            _manageRuleSevice.StopRuleSession(id);
            _manageTimeSevice.StopTimeService(id);
            _manageLogService.StopLogService(id);
            _manageEventService.StopEventService(id);
        }

        private void SetAllInactiveActive()
        {
            var runs = _context.SessionRuns.Where(sr => sr.Active);
            foreach (SessionRun r in runs)
            {
                r.Active = false;
                StopSessionRun(r.ID);
                if (r.StopTime == null)
                {
                    r.StopTime = DateTime.Now;
                }
            }
        }
    }
}
