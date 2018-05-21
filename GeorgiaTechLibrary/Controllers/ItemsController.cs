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
<<<<<<< HEAD
=======
using GeorgiaTechLibraryAPI.Models.APIModel;
>>>>>>> 861fa0c9cfcdb724630fbc72ea0dd46b7bd645d9

namespace GeorgiaTechLibraryAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Items")]
    public class ItemsController : Controller
    {
        private readonly IRepositoryAsync<Item> _repository;
<<<<<<< HEAD

        public ItemsController(LibraryContext context)
        {
=======
        private readonly LibraryContext _context;

        public ItemsController(LibraryContext context)
        {
            _context = context;
>>>>>>> 861fa0c9cfcdb724630fbc72ea0dd46b7bd645d9
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

<<<<<<< HEAD
            var item = await _repository.GetAsync(e => e.Id == id);
=======
            var item = await _repository.GetAsync(m => m.Id == id);
>>>>>>> 861fa0c9cfcdb724630fbc72ea0dd46b7bd645d9

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // PUT: api/Items/5
        [HttpPut("{id}")]
<<<<<<< HEAD
        public async Task<IActionResult> PutItem([FromRoute] Guid id, [FromBody] ItemInfo info)
=======
        public async Task<IActionResult> PutItem([FromRoute] Guid id, [FromBody] Item item)
>>>>>>> 861fa0c9cfcdb724630fbc72ea0dd46b7bd645d9
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

<<<<<<< HEAD
            try
            {
                Item item = ItemFactory.Get(info);
                await _repository.UpdateAsync(item);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await ItemExists(id)))
=======
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
>>>>>>> 861fa0c9cfcdb724630fbc72ea0dd46b7bd645d9
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

<<<<<<< HEAD
        // POST: api/Items
        [HttpPost("{ISBN}")]
        public async Task<IActionResult> PostItem([FromBody] ItemInfo info, [FromRoute] string ISBN)
=======
        // POST: api/Items/12ab34cd
        [HttpPost("{isbn}")]
        public async Task<IActionResult> PostItem([FromBody] ItemAPI item, [FromRoute] string isbn)
>>>>>>> 861fa0c9cfcdb724630fbc72ea0dd46b7bd645d9
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

<<<<<<< HEAD
            Item item = ItemFactory.Get(info, ISBN);
            await _repository.AddAsync(item);
=======
            Item itm = ItemFactory.Get(item.ItemInfo, isbn);
            await _repository.AddAsync(itm);
>>>>>>> 861fa0c9cfcdb724630fbc72ea0dd46b7bd645d9

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

<<<<<<< HEAD
            var item = await _repository.GetAsync(m => m.Id == id);
=======
            var item = await _context.Items.SingleOrDefaultAsync(m => m.Id == id);
>>>>>>> 861fa0c9cfcdb724630fbc72ea0dd46b7bd645d9
            if (item == null)
            {
                return NotFound();
            }

<<<<<<< HEAD
            await _repository.DeleteAsync(item);
=======
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
>>>>>>> 861fa0c9cfcdb724630fbc72ea0dd46b7bd645d9

            return Ok(item);
        }

<<<<<<< HEAD
        private async Task<bool> ItemExists(Guid id)
        {
            if (await _repository.GetAsync(i => i.Id == id) != null)
                return true;

            return false;
=======
        private bool ItemExists(Guid id)
        {
            return _context.Items.Any(e => e.Id == id);
>>>>>>> 861fa0c9cfcdb724630fbc72ea0dd46b7bd645d9
        }
    }
}