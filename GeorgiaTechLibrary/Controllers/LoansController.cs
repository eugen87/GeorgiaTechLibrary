using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeorgiaTechLibrary.Models;
using GeorgiaTechLibraryAPI.Models.Repositories;
using GeorgiaTechLibraryAPI.Models.APIModel;
using GeorgiaTechLibrary.Models.Items;
using GeorgiaTechLibrary.Models.Members;

namespace GeorgiaTechLibraryAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Loans")]
    public class LoansController : Controller
    {
        private readonly IRepositoryAsync<Loan> _repository;
        private readonly IRepositoryAsync<Member> _memberRepo;
        private readonly IRepositoryAsync<Item> _itemRepo;

        public LoansController(DbContext context)
        {
            _repository = new RepositoryAsync<Loan>(context);
            _memberRepo = new MemberRepositoryAsync<Member>(context);
            _itemRepo = new ItemRepositoryAsync<Item>(context);
        }

        // GET: api/Loans
        [HttpGet]
        public async Task<IEnumerable<Loan>> GetLoans()
        {
            return await _repository.GetAsync();
        }

        // GET: api/Loans/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoan([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loan = await _repository.GetAsync(e => e.LoanID == id);

            if (loan.Count() == 0)
            {
                return NotFound();
            }

            return Ok(loan);
        }
 

        // POST: api/Loans
        [HttpPost]
        public async Task<IActionResult> PostLoan([FromBody] LoanAPI loanApi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Item item = (await _itemRepo.GetAsync(i => i.Id == loanApi.ItemId)).FirstOrDefault();
            Member member = (await _memberRepo.GetAsync(m => m.Ssn == loanApi.MemberSsn)).FirstOrDefault();

            if (item != null && member != null)
            {
                if (item.RentStatus != RentStatus.AVAILABLE)
                {
                    return BadRequest();
                }

                if (member.Loans.Where(l => l.IsReturned == false).Count() + 1 > member.LoanRule.BookLimit)
                {
                    return BadRequest();
                }

                item.RentStatus = RentStatus.UNAVAILABLE;
                await _itemRepo.UpdateAsync(item);

                Loan loan = new Loan(loanApi.ItemId, loanApi.MemberSsn);
                await _repository.AddAsync(loan);

                return CreatedAtAction("GetLoan", new { id = loan.LoanID }, loan);
            }
            return BadRequest();
        }

        [HttpPost("{condition}")]
        public async Task<IActionResult> ReturnLoan([FromBody] LoanAPI loanApi, [FromRoute] int condition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Item item = (await _itemRepo.GetAsync(i => i.Id == loanApi.ItemId)).FirstOrDefault();
            Member member = (await _memberRepo.GetAsync(m => m.Ssn == loanApi.MemberSsn)).FirstOrDefault();

            if (item != null && member != null)
            {
                if (item.RentStatus != RentStatus.UNAVAILABLE && item.ItemStatus == ItemStatus.RENTABLE)
                {
                    return BadRequest();
                }

                var cond = (ItemCondition)condition;
                if (cond == ItemCondition.OK || cond == ItemCondition.DAMAGED)
                {
                    item.RentStatus = RentStatus.AVAILABLE;
                    item.ItemCondition = (ItemCondition)condition;
                    await _itemRepo.UpdateAsync(item);
                }
                else
                {
                    item.ItemCondition = (ItemCondition)condition;
                    item.ItemStatus = ItemStatus.NONRENTABLE;
                    await _itemRepo.UpdateAsync(item);
                }

                var loan = (await _repository.GetAsync(l => l.MemberSsn == loanApi.MemberSsn && l.ItemId == loanApi.ItemId && l.IsReturned == false)).FirstOrDefault();
                loan.IsReturned = true;
                await _repository.UpdateAsync(loan);

                return CreatedAtAction("ReturnedLoan", new { id = loan.LoanID }, loan);
            }
            return BadRequest();
        }



        private async Task<bool> LoanExists(int id)
        {
            if (await _repository.GetAsync(i => i.LoanID == id) != null)
                return true;

            return false;
        }
    }
}