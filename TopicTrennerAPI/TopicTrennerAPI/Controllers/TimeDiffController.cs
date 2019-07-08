using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopicTrennerAPI.Data;
using TopicTrennerAPI.Models;

namespace TopicTrennerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeDiffController : ControllerBase
    {
        DbTopicTrennerContext _context;

        public TimeDiffController(DbTopicTrennerContext context)
        {
            _context = context;
        }

        // GET: api/TimeDiff
        [HttpGet]
        public ActionResult<TimeDiff> Get()
        {
            var sub = _context.TimeDiffs.Where(t => t.SessionRun.Active == true);
            return sub.Count() > 0 ? sub.First() : null;
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
            _context.SaveChangesAsync();
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

            return NoContent();
        }
    }
}
