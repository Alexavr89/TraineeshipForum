﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TraineeshipForum.Data;
using TraineeshipForum.Models.Actions.WithTopics;
using TraineeshipForum.Models.Entities;
using TraineeshipForum.Services.Categories;
using TraineeshipForum.Services.Topics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TraineeshipForum.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ITopic _topicService;
        private readonly ICategory _categoryService;
        private static UserManager<ApplicationUser> _userManager;
        public TopicsApiController(ApplicationDbContext context, ITopic topicService, ICategory categoryService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _topicService = topicService;
            _categoryService = categoryService;
            _userManager = userManager;
        }
        // GET: api/<TopicsApiController>
        [HttpGet]
        public IEnumerable<Topic> GetAll()
        {
            return _topicService.GetAllTopics();
        }

        // GET api/<TopicsApiController>/5
        [HttpGet("{id}")]
        public Topic Get(int id)
        {
            return _topicService.GetById(id);
        }

        // POST api/<TopicsApiController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewTopic topic)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var category = _categoryService.GetById(topic.CategoryId);
                    var user = _userManager.FindByIdAsync(topic.User.Id);

                    topic.Category = category;
                    topic.User = await user;

                    if (_context.Topics.Any(t => t.Title == topic.Title))
                    {
                        return BadRequest("Topic with this title already exists");
                    }
                    await _topicService.Add(topic);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return BadRequest("Unable to save changes.");
            }
            return CreatedAtAction(nameof(Get), new { id = topic.Id }, topic);
        }


        // PUT api/<TopicsApiController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] NewTopic topic)
        {
            var topicToUpdate = _topicService.GetById(id);
            if (_context.Topics.Any(t => t.Title == topic.Title))
            {
                return BadRequest("Topic with this title already exists");
            }
            topicToUpdate.Title = topic.Title;
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = topicToUpdate.Id }, topicToUpdate);
        }

        // DELETE api/<TopicsApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var topicToDelete = _topicService.GetById(id);
            if (topicToDelete == null)
            {
                return NotFound();
            }
            try
            {
                await _topicService.Delete(id);
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
