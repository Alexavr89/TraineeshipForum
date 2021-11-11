using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TraineeshipForum.Data;
using TraineeshipForum.Models.Entities;
using TraineeshipForum.Services.User;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TraineeshipForum.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesApiController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUser _userService;
        private readonly ApplicationDbContext _context;

        public ProfilesApiController(UserManager<ApplicationUser> userManager, IApplicationUser userService, ApplicationDbContext context)
        {
            _userManager = userManager;
            _userService = userService;
            _context = context;
        }
        // GET: api/<ProfileApiController>
        [HttpGet]
        public IEnumerable<ApplicationUser> Get()
        {
            return _userService.GetAll();
        }

        // GET api/<ProfileApiController>/5
        [HttpGet("{id}")]
        public ApplicationUser Get(string id)
        {
            return _userService.GetById(id);
        }

        // POST api/<ProfileApiController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ApplicationUser user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_context.ApplicationUsers.Any(c => c.UserName == user.UserName))
                    {
                        ModelState.AddModelError("Title", "Category with this title already exists");
                    }
                    await _userManager.CreateAsync(user);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return BadRequest("Unable to save changes. Try again...");
            }
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        // PUT api/<ProfileApiController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] ApplicationUser appuser)
        {
            var user = _userService.GetById(id);
            if (_context.Users.Any(t => t.UserName == appuser.UserName))
            {
                return BadRequest("Topic with this title already exists");
            }
            user.UserName = appuser.UserName;
            await _context.SaveChangesAsync();
            return Ok();
        }

        // DELETE api/<ProfileApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = _userService.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            try
            {
                await _userManager.DeleteAsync(user);
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
