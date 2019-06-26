﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TopicTrennerAPI.Models;
using TopicTrennerAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace TopicTrennerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        DbTopicTrennerContext _context;

        public SubjectController(DbTopicTrennerContext context)
        {
            _context = context;
        }

        // GET: api/Subject
        [HttpGet]
        public ActionResult<IEnumerable<Subject>> Get()
        {
            var sub = _context.Subjects;
            return sub;
        }

        // GET: api/Subject/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Subject>> Get(int id)
        {
            if (_context.Subjects.Count() > 0)
            {
                var Subject = await _context.Subjects.FindAsync(id);//TodoItems.FindAsync(id);
                return Subject;
            }

            return null;
        }

        // POST: api/Subject
        [HttpPost]
        public void Post(Subject sSubject)
        {
            _context.Subjects.Add(sSubject);
            _context.SaveChangesAsync();
        }

        // PUT: api/Subject/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Subject sSubject)
        {
            if(id != sSubject.ID)
            {
                return BadRequest();
            }

            _context.Entry(sSubject).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var Subject = await _context.Subjects.FindAsync(id);

            if (Subject == null)
            {
                return NotFound();
            }

            _context.Subjects.Remove(Subject);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}