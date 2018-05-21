using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Models.Items;
using GeorgiaTechLibraryAPI.Models.Repositories;
using GeorgiaTechLibrary.Models.Factories.Items;
using GeorgiaTechLibraryAPI.Models.APIModel;

namespace GeorgiaTechLibraryAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Items")]
    public class ItemsController : Controller
    {
        private readonly IRepositoryAsync<Item> _repository;
        private readonly LibraryContext _context;

        public ItemsController(LibraryContext context)
        {
            _context = context;
            _repository = new RepositoryAsync<Item>(context);
        }

        // GET: api/Items
        [HttpGet]
        public async Task<IEnumerable<Item>> GetItems()
        {
            return await _repository.GetAsync();
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await _repository.GetAsync(m => m.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // PUT: api/Items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem([FromRoute] Guid id, [FromBody] Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item.Id)
            {
                return BadRequest();
            }

            //_context.Entry(item).State = EntityState.Modified;

            try
            {
                //Item itm = ItemFactory.Get();
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
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

        // POST: api/Items/12ab34cd
        [HttpPost("{isbn}")]
        public async Task<IActionResult> PostItem([FromBody] ItemAPI item, [FromRoute] string isbn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Item itm = ItemFactory.Get(item.ItemInfo, isbn);
            await _repository.AddAsync(itm);

            return CreatedAtAction("GetItem", new { id = item.Id }, item);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await _context.Items.SingleOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }

        private bool ItemExists(Guid id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}