using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopicTrennerAPI.Data;
using TopicTrennerAPI.Interfaces;
using TopicTrennerAPI.Models;

namespace TopicTrennerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeDiffController : ControllerBase
    {
        readonly DbTopicTrennerContext _context;
        readonly IManageTimeService _ctrlTimeSession;

        public TimeDiffController(DbTopicTrennerContext context, IManageTimeService controlTimeSession)
        {
            _context = context;
            _ctrlTimeSession = controlTimeSession;
        }

        // GET: api/TimeDiff
        [HttpGet]
        public ActionResult<IEnumerable<TimeDiff>> Get()
        {
            return _context.TimeDiffs;
        }

        // GET: api/TimeDiff/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TimeDiff>> Get(int id)
        {
            if (_context.TimeDiffs.Count() > 0)
            {
                var TimeDiff = await _context.TimeDiffs.FindAsync(id);//TodoItems.FindAsync(id);
                return TimeDiff;
            }
            //var td = new TimeDiff();
            //td.ID = 1;
            //td.Diff = 
            return null;
        }

        // POST: api/TimeDiff
        [HttpPost]
        public void Post(TimeDiff sTimeDiff)
        {
            _context.TimeDiffs.Add(sTimeDiff);
            _context.SaveChanges();
            ReloadTimeService(sTimeDiff.ID);
        }

        // PUT: api/TimeDiff/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, TimeDiff sTimeDiff)
        {
            if (id != sTimeDiff.ID)
            {
                return BadRequest();
            }

            _context.Entry(sTimeDiff).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            ReloadTimeService(sTimeDiff.ID);
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var TimeDiff = await _context.TimeDiffs.FindAsync(id);

            if (TimeDiff == null)
            {
                return NotFound();
            }

            _context.TimeDiffs.Remove(TimeDiff);
            await _context.SaveChangesAsync();
            ReloadTimeService(TimeDiff.ID);

            return NoContent();
        }

        private void ReloadTimeService(int sessionRunId)
        {
            var sr = _context.SessionRuns.Where(s => s.ID == sessionRunId).First();
            _ctrlTimeSession.ReloadTimeService(sr.SessionID);
        }
    }
}
