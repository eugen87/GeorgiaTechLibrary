using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Models.Employees;
using GeorgiaTechLibraryAPI.Models.Repositories;

namespace GeorgiaTechLibraryAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Employees")]
    public class EmployeesController : Controller
    {
        private readonly IRepositoryAsync<Employee> _repository;

        public EmployeesController(DbContext context)
        {
            _repository = new RepositoryAsync<Employee>(context);
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await _repository.GetAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _repository.GetAsync(e => e.Ssn == id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee([FromRoute] long id, [FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.Ssn)
            {
                return BadRequest();
            }

            try
            {
                await _repository.UpdateAsync(employee);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await EmployeeExists(id)))
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

        // POST: api/Employees
        [HttpPost]
        public async Task<IActionResult> PostEmployee([FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.AddAsync(employee);

            return CreatedAtAction("GetEmployee", new { id = employee.Ssn }, employee);
        }

        [HttpPost("{name}")]
        public async Task<IActionResult> PostsEmployee([FromBody] string name)
        {
            return null;
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _repository.GetAsync(e => e.Ssn == id);
            if (employee == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(employee);

            return Ok(employee);
        }

        private async Task<bool> EmployeeExists(long id)
        {
            if (await _repository.GetAsync(e => e.Ssn == id) != null)
                return true;

            return false;
        }
    }
}