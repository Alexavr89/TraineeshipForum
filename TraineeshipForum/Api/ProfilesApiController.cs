using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TraineeshipForum.Models.Entities;
using TraineeshipForum.Services.Upload;
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
        private readonly IUpload _uploadService;
        private readonly IConfiguration _configuration;

        public ProfilesApiController(UserManager<ApplicationUser> userManager, IApplicationUser userService, IUpload uploadService, IConfiguration configuration)
        {
            _userManager = userManager;
            _userService = userService;
            _uploadService = uploadService;
            _configuration = configuration;
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
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProfileApiController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProfileApiController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
