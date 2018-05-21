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

namespace GeorgiaTechLibraryAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Items")]
    public class ItemsController : Controller
    {
        private readonly IRepositoryAsync<Item> _repository;

        public ItemsController(DbContext context)
        {
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

            var item = await _repository.GetAsync(e => e.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // PUT: api/Items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem([FromRoute] Guid id, [FromBody] ItemInfo info)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Item item = ItemFactory.Get(info);
                await _repository.UpdateAsync(item);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await ItemExists(id)))
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

        // POST: api/Items
        [HttpPost("{ISBN}")]
        public async Task<IActionResult> PostItem([FromBody] ItemInfo info, [FromRoute] string ISBN)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Item item = ItemFactory.Get(info, ISBN);
            await _repository.AddAsync(item);

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

            var item = await _repository.GetAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(item);

            return Ok(item);
        }

        private async Task<bool> ItemExists(Guid id)
        {
            if (await _repository.GetAsync(i => i.Id == id) != null)
                return true;

            return false;
        }
    }
}