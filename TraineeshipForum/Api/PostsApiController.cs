using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TraineeshipForum.Data;
using TraineeshipForum.Models.Actions.WithPosts;
using TraineeshipForum.Models.Entities;
using TraineeshipForum.Services.Categories;
using TraineeshipForum.Services.Posts;
using TraineeshipForum.Services.Topics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TraineeshipForum.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ITopic _topicService;
        private readonly ICategory _categoryService;
        private readonly IPost _postService;

        private static UserManager<ApplicationUser> _userManager;

        public PostsApiController(ApplicationDbContext context, ITopic topicService, ICategory categoryService, UserManager<ApplicationUser> userManager, IPost postService)
        {
            _context = context;
            _topicService = topicService;
            _categoryService = categoryService;
            _userManager = userManager;
            _postService = postService;
        }
        // GET: api/<PostsApiController>
        [HttpGet]
        public IEnumerable<Post> Get()
        {
            return _context.Posts.AsEnumerable();
        }

        // GET api/<PostsApiController>/5
        [HttpGet("{id}")]
        public Post Get(int id)
        {
            return _postService.GetById(id);
        }

        // POST api/<PostsApiController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewPost post)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var topic = _topicService.GetById(post.TopicId);
                    var user = _userManager.FindByIdAsync(post.User.Id).Result;

                    post.User = user;
                    post.Topic = topic;

                    if (_context.Posts.Any(p => p.Content == post.Content))
                    {
                        return BadRequest("Post with this content already exist");
                    }

                    await _postService.Add(post);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return BadRequest("Unable to save changes. Try again...");
            }
            return CreatedAtAction(nameof(Get), new { id = post.Id }, post);
        }

        // PUT api/<PostsApiController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] NewPost post)
        {
            var postToUpdate = _postService.GetById(id);
            if (_context.Posts.Any(p => p.Content == post.Content))
            {
                return BadRequest("Post with this content already exist");
            }
            try
            {
                postToUpdate.Content = post.Content;
                await _context.SaveChangesAsync();
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return BadRequest("Unable to save changes.");
            }
            return CreatedAtAction(nameof(Get), new { id = postToUpdate.Id }, postToUpdate);
        }

        // DELETE api/<PostsApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var postToDelete = _postService.GetById(id);
            if (postToDelete == null)
            {
                return NotFound();
            }
            try
            {
                await _postService.Delete(id);
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
