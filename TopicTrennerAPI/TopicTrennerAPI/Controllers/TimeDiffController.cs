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

            return null;
        }

        // POST: api/TimeDiff
        [HttpPost]
        public void Post(TimeDiff sTimeDiff)
        {
            _context.TimeDiffs.Add(sTimeDiff);
            _context.SaveChanges();
            _ctrlTimeSession.ReloadTimeService(sTimeDiff.ID);
        }

        // PUT: api/TimeDiff/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, TimeDiff sTimeDiff)
        {
            if (id != sTimeDiff.ID)
            {
                return BadRequest();
            }
            var dbDiff = _context.TimeDiffs.Find(id);

            if (_context.SessionRuns.Where(s => s.ID == id).First().Active)
            {
                if(dbDiff.Diff > sTimeDiff.Diff)
                {
                    // if the Session is Active Time shift in the past is not alowed
                    return BadRequest();
                }
            }

            dbDiff.Diff = sTimeDiff.Diff;
            _context.Entry(dbDiff).State = EntityState.Modified;
            _context.SaveChanges();
            _ctrlTimeSession.ReloadTimeService(sTimeDiff.ID);
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
            _ctrlTimeSession.ReloadTimeService(TimeDiff.ID);

            return NoContent();
        }
    }
}
