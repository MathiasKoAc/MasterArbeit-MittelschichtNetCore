using System;
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
    public class SubjectRulesController : ControllerBase
    {
        readonly DbTopicTrennerContext _context;

        public SubjectRulesController(DbTopicTrennerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<SubjectRules> Get(int subjectId, EnumBindingTyp bindingTyp)
        {
            if (_context.SimpleRuleSubjects.Count() > 0)
            {
                var ruleSubjectList = _context.SimpleRuleSubjects.Where(s => s.SubjectID == subjectId && s.BindingTyp == bindingTyp).ToList();

                SubjectRules sRules = new SubjectRules();
                sRules.SubjectID = subjectId;
                sRules.BindingTyp = bindingTyp;
                sRules.SimpleRuleIDs = new List<int>();

                foreach(SimpleRuleSubject srs in ruleSubjectList)
                {
                    sRules.SimpleRuleIDs.Add(srs.SimpleRuleID);
                }

                return sRules;
            }

            return null;
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, SubjectRules subjectRules)
        {
            if(id != subjectRules.SubjectID)
            {
                return BadRequest();
            }

            var simpleRuleSubjectsDb = _context.SimpleRuleSubjects.Where(s => s.SubjectID == id).ToList();
            _context.SimpleRuleSubjects.RemoveRange(simpleRuleSubjectsDb);

            foreach(int ruleId in subjectRules.SimpleRuleIDs)
            {
                SimpleRuleSubject srs = new SimpleRuleSubject();
                srs.SimpleRuleID = ruleId;
                srs.SubjectID = id;
                srs.BindingTyp = subjectRules.BindingTyp;
                _context.SimpleRuleSubjects.Add(srs);
            }

            _context.SaveChanges();
            return NoContent();
        }
    }
}
