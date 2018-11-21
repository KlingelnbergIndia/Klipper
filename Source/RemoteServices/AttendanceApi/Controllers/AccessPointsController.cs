using System.Collections.Generic;
using System.Threading.Tasks;
using AttendanceApi.DataAccess.Interfaces;
using Common.Infrastructure;
using Models.Core.HR.Attendance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AttendanceApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AccessPointsController : ControllerBase
    {
        private readonly IAccessPointRepository _repository = null;

        public AccessPointsController(IAccessPointRepository repository)
        {
            _repository = repository;
        }

        #region HTTP methods

        // GET api/AccessPoints
        [NoCache]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_repository.GetAllAccessPoints().Result);
        }

        // GET api/AccessPoints/5
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

        // POST api/AccessPoints
        [HttpPost]
        public IActionResult Post([FromBody] AccessPoint value)
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

        // PUT api/AccessPoints/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] AccessPoint value)
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

        // DELETE api/AccessPoints/5
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
