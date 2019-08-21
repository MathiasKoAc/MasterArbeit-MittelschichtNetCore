using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TopicTrennerAPI.Models;
using TopicTrennerAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace TopicTrennerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagePurposeController : ControllerBase
    {
        readonly DbTopicTrennerContext _context;

        public MessagePurposeController(DbTopicTrennerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PurposeMessage>> Get()
        {
            var sub = _context.PurposeMessages;
            return sub;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PurposeMessage>> Get(int id)
        {
            if (_context.PurposeMessages.Count() > 0)
            {
                var PurposeMessage = await _context.PurposeMessages.FindAsync(id);
                return PurposeMessage;
            }

            return null;
        }

        // POST: api/Session
        [HttpPost]
        public IActionResult Post(PurposeMessage purposeMessage)
        {
            if (purposeMessage.ID != 0 && _context.PurposeMessages.Find(purposeMessage.ID) != null)
            {
                return BadRequest();
            }

            _context.PurposeMessages.Add(purposeMessage);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, PurposeMessage purposeMessage)
        {
            if(id != purposeMessage.ID)
            {
                return BadRequest();
            }

            _context.Entry(purposeMessage).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var purposeMessage = await _context.PurposeMessages.FindAsync(id);

            if (purposeMessage == null)
            {
                return NotFound();
            }

            _context.PurposeMessages.Remove(purposeMessage);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
