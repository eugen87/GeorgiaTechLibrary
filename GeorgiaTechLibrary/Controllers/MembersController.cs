using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Models.Members;
using GeorgiaTechLibraryAPI.Models.Repositories;
using GeorgiaTechLibraryAPI.Models.Factories.Members;
using GeorgiaTechLibraryAPI.Models.APIModel;

namespace GeorgiaTechLibraryAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Members")]
    public class MembersController : Controller
    {
        private readonly IRepositoryAsync<Member> _repository;
        private readonly DbContext _context;

        public MembersController(LibraryContext context)
        {
            _context = context;
            _repository = new RepositoryAsync<Member>(context);
        }

        // GET: api/Members
        [HttpGet]
        public async Task<IEnumerable<Member>> GetMembers()
        {
            return await _repository.GetAsync();
        }

        // GET: api/Members/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMember([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var member = await _repository.GetAsync(m => m.Ssn == id);

            if (member == null)
            {
                return NotFound();
            }

            return Ok(member);
        }

        // PUT: api/Members/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember([FromBody] MemberAPI memberAPI)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Member member = MemberFactory.Get(memberAPI, MemberEnum.Student);
                await _repository.UpdateAsync(member);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await MemberExists(memberAPI.Ssn)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Members
        [HttpPost("{loanType}")]
        public async Task<IActionResult> PostMember([FromBody] MemberAPI memberAPI, [FromRoute] int loanType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Member member = MemberFactory.Get(memberAPI, (MemberEnum)loanType);
   
            await _repository.AddAsync(member);

            return CreatedAtAction("GetMember", new { id = member.Ssn }, member);
        }

        // DELETE: api/Members/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var member = await _repository.GetAsync(m => m.Ssn == id);
            if (member == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(member);
            await _context.SaveChangesAsync();

            return Ok(member);
        }

        private async Task<bool> MemberExists(long id)
        {
            if (await _repository.GetAsync(e => e.Ssn == id) != null)
                return true;

            return false;
        }
    }
}