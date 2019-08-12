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
    public class EventMessageController : ControllerBase
    {
        readonly DbTopicTrennerContext _context;
        readonly IManageEventService _eventManager;

        public EventMessageController(DbTopicTrennerContext context, IManageEventService eventService)
        {
            _context = context;
            _eventManager = eventService;
        }

        // GET: api/EventMessage
        [HttpGet]
        public ActionResult<IEnumerable<EventMessage>> Get()
        {
            var sub = _context.Events;
            return sub;
        }

        // GET: api/EventMessage/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventMessage>> Get(int id)
        {
            if (_context.Events.Count() > 0)
            {
                var EventMessage = await _context.Events.FindAsync(id);
                return EventMessage;
            }

            return null;
        }

        // POST: api/EventMessage
        [HttpPost]
        public IActionResult Post(EventMessage sEventMessage)
        {
            if (sEventMessage.ID != 0 && _context.Events.Find(sEventMessage.ID) != null)
            {
                return BadRequest();
            }

            _context.Events.Add(sEventMessage);
            _context.SaveChanges();

            ReloadEventService(sEventMessage.SessionId);
            return NoContent();
        }

        // PUT: api/EventMessage/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventMessage sEventMessage)
        {
            if(id != sEventMessage.ID)
            {
                return BadRequest();
            }

            _context.Entry(sEventMessage).State = EntityState.Modified;
            _context.SaveChanges();

            ReloadEventService(sEventMessage.SessionId);
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var EventMessage = await _context.Events.FindAsync(id);

            if (EventMessage == null)
            {
                return NotFound();
            }

            _context.Events.Remove(EventMessage);
            await _context.SaveChangesAsync();

            ReloadEventService(EventMessage.SessionId);
            return NoContent();
        }

        private void ReloadEventService(int SessionId)
        {
            var query = _context.SessionRuns.Where(s => s.SessionID == SessionId);
            if(query.Count() > 0) {
                SessionRun sesRun = query.First();
                if (sesRun != null && sesRun.Active)
                {
                    _eventManager.ReloadEventService(sesRun.ID);
                }
            }
        }
    }
}
