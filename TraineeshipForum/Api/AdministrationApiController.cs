using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TraineeshipForum.Data;
using TraineeshipForum.Models.Actions.WithRoles;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TraineeshipForum.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrationApiController : ControllerBase
    {
        private static RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public AdministrationApiController(RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }
        // GET: api/<AdministrationApiController>
        [HttpGet]
        public IEnumerable<IdentityRole> GetAll()
        {
            return _roleManager.Roles.AsEnumerable();
        }

        // GET api/<AdministrationApiController>/5
        [HttpGet("{id}")]
        public Task<IdentityRole> Get(string id)
        {
            return _roleManager.FindByIdAsync(id);
        }

        // POST api/<AdministrationApiController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateRole role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityRole identityRole = new IdentityRole
                    {
                        Name = role.RoleName
                    };

                    await _roleManager.CreateAsync(identityRole);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return BadRequest("Unable to save changes. Try again...");
            }
            return Ok();
        }

        // PUT api/<AdministrationApiController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EditRole model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    role.Name = model.RoleName;
                    await _roleManager.UpdateAsync(role);
                    await _context.SaveChangesAsync();
                }
                catch (DataException)
                {
                    return BadRequest("Unable to save changes");
                }
                return CreatedAtAction(nameof(Get), new { id = model.Id }, model);
            }
        }

        // DELETE api/<AdministrationApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }
            try
            {
                await _roleManager.DeleteAsync(role);
                await _context.SaveChangesAsync();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return BadRequest("Unable to delete. Try again...");
            }
            return NoContent();
        }
    }
}
