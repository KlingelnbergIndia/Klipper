using EmployeeApi.DataAccess.Interfaces;
using Models.Core.Employment;
using Common.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
	//[Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _repository = null;

        public EmployeesController(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        #region HTTP methods

        // GET api/employees
        [NoCache]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_repository.GetAllEmployees().Result);
        }

        // GET api/employees/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_repository.Exists(id).Result)
            {
                return NotFound();
            }
            var value = _repository.Get(id).Result;
            if(value == null)
            {
                return NotFound();
            }
            return Ok(value);
        }

        // POST api/employees
        [HttpPost]
        public IActionResult Post([FromBody] Employee value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(!_repository.Add(value).Result)
            {
                return StatusCode(500, "A problem while handling your request!");
            }
            return CreatedAtAction("Get", new { id = value.ID }, value);
        }

        // PUT api/employees/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Employee value)
        {
            if(value == null || id < 0)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(!_repository.Exists(id).Result)
            {
                return NotFound();
            }
            value.ID = id;
            if(_repository.Update(id, value).Result)
            {
                return StatusCode(202, "Updated Successfully!");
            }
            return NoContent();
        }

        // DELETE api/employees/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_repository.Exists(id).Result)
            {
                return NotFound();
            }
            if (_repository.Remove(id).Result)
            {
                return Ok();
            }
            return NoContent();
        }

        #endregion

    }
}
