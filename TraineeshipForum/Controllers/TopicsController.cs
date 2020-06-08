using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TraineeshipForum.Data;
using TraineeshipForum.Models;
using TraineeshipForum.Models.Actions.WithTopics;
using TraineeshipForum.Models.Entities;
using TraineeshipForum.Models.Pages;
using TraineeshipForum.Services_Interfaces.Categories;
using TraineeshipForum.Services_Interfaces.Topics;

namespace TraineeshipForum.Controllers
{
    public class TopicsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITopic _topicService;
        private readonly ICategory _categoryService;

        private static UserManager<IdentityUser> _userManager;

        public TopicsController(ApplicationDbContext context, ITopic topicService, ICategory categoryService, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _topicService = topicService;
            _categoryService = categoryService;
            _userManager = userManager;
        }

        // GET: Topics
        public IActionResult Index()
        {
            var model = CreateCategoryPage();
            return View(model);
        }

        public CategoryPage CreateCategoryPage()
        {
            var topic = _topicService.GetAllTopics().Select(topic => new TopicListing //not sure about select
            {
                CategoryId = topic.Category.Id,
                TopicId = topic.Id,
                CategoryTitle = topic.Category.Title,
                AuthorName = topic.User.UserName,
                TopicTitle = topic.Title,
                PostCount = topic.Posts.Count(),
                DateCreated = topic.Created.ToString()
            });

            return new CategoryPage()
            {
                Topics = topic
            };
        }

        public IActionResult TopicsByCategory(int id)
        {
            var model = CreateTopicsByCategory(id);
            return View(model);
        }

        public CategoryPage CreateTopicsByCategory(int id)
        {
            var category = _categoryService.GetById(id);
            var topics = category.Topics;
            var categories = _categoryService.GetAll(); //1.06

            var topicListing = topics.Select(topic => new TopicListing
            {
                TopicId = topic.Id,
                AuthorName = topic.User.UserName,
                DateCreated = topic.Created.ToString(),
                PostCount = topic.Posts.Count(),
                TopicTitle = topic.Title
            });

            var categoryListing = categories.Select(category => new CategoryListing //1.06
            {
                Title = category.Title,   //1.06
                Id = category.Id
            });
            return new CategoryPage()
            {
                Topics = topicListing,
                Category = CreateCategoryListing(category),
                Categories = categoryListing //1.06
            };
        }

        public CategoryListing CreateCategoryListing(Category category)
        {
            return new CategoryListing
            {
                Id = category.Id,
                Title = category.Title,
                Description = category.Description
            };
        }

        // GET: Topics/Create
        [Authorize]
        public IActionResult Create(int id)
        {
            var model = new NewTopic
            {
                CategoryId = id,
            };
            return View(model);
        }

        // POST: Topics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title,Created,UserId")] Topic topic, int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = _userManager.GetUserId(User);
                    var user = _userManager.FindByIdAsync(userId).Result;
                    var category = _categoryService.GetById(id);

                    topic.Category = category;
                    topic.User = user;
                    topic.Created = DateTime.Now;

                    _context.Add(topic);
                    _context.SaveChanges();

                    return RedirectToAction("PostsByTopic", "Posts", new { id = topic.Id, categoryId = id });
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again...");
            }
            return View(topic);
        }

        // GET: Topics/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id, int categoryId)
        {
            if (id == null)
            {
                return Content(HttpStatusCode.NotFound, "Topic does not exist");
            }
            var topic = await _context.Topics.FindAsync(id);
            var model = new NewTopic
            {
                CategoryId = categoryId,
                Title = topic.Title,
            };

            if (topic == null)
            {
                return NotFound();
            }

            return View(model);
        }

        private IActionResult Content(HttpStatusCode notFound, string v)
        {
            throw new NotImplementedException();
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTopicAsync(int? id, int categoryId)
        {
            if (id == null)
            {
                return NotFound();
            }
            var topicToUpdate = _context.Topics.Find(id);
            if (await TryUpdateModelAsync(topicToUpdate,
                "",
                t => t.Title))
            {
                try
                {
                    _context.SaveChanges();
                    return RedirectToAction("TopicsByCategory", "Topics", new { id = categoryId });
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes.");
                }
            }
            return View(topicToUpdate);
        }

        // GET: Topics/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int categoryId, int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed";
            }
            var topic = await _context.Topics.FindAsync(id);
            var model = new DeleteTopic
            {
                CategoryId = categoryId,
                TopicTitle = topic.Title
            };

            if (topic == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int CategoryId)
        {
            try
            {
                var topicToDelete = await _context.Topics.FindAsync(id);

                _context.Entry(topicToDelete).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id, saveChangesError = true });
            }
            return RedirectToAction("TopicsByCategory", "Topics", new { id = CategoryId });
        }
    }
}
