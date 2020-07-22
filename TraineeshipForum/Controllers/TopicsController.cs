using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TraineeshipForum.Data;
using TraineeshipForum.Models;
using TraineeshipForum.Models.Actions;
using TraineeshipForum.Models.Actions.WithPosts;
using TraineeshipForum.Models.Actions.WithTopics;
using TraineeshipForum.Models.Entities;
using TraineeshipForum.Models.Pages;
using TraineeshipForum.Services_Interfaces.Categories;
using TraineeshipForum.Services_Interfaces.Posts;
using TraineeshipForum.Services_Interfaces.Topics;

namespace TraineeshipForum.Controllers
{
    public class TopicsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITopic _topicService;
        private readonly ICategory _categoryService;
        private readonly IPost _postService;

        private static UserManager<ApplicationUser> _userManager;

        public TopicsController(ApplicationDbContext context, ITopic topicService, ICategory categoryService, UserManager<ApplicationUser> userManager, IPost postService)
        {
            _context = context;
            _topicService = topicService;
            _categoryService = categoryService;
            _userManager = userManager;
            _postService = postService;
        }

        // GET: Topics
        public IActionResult Index()
        {
            var model = CreateCategoryPage();
            return View(model);
        }

        public CategoryPage CreateCategoryPage()
        {
            var topic = _topicService.GetAllTopics().Select(topic => new TopicListing
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
            var categories = _categoryService.GetAll();

            var topicListing = topics.Select(topic => new TopicListing
            {
                TopicId = topic.Id,
                AuthorName = topic.User.UserName,
                DateCreated = topic.Created.ToString(),
                PostCount = topic.Posts.Count(),
                TopicTitle = topic.Title
            });

            var categoryListing = categories.Select(category => new CategoryListing
            {
                Title = category.Title,
                Id = category.Id
            });
            return new CategoryPage()
            {
                Topics = topicListing,
                Category = CreateCategoryListing(category),
                Categories = categoryListing
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

        [Authorize]
        public IActionResult CreateTopicandPost(int id)
        {
            var model = new NewTopicandPost
            {
                CategoryId = id,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTopicandPost(int id, NewTopicandPost topicandpost, NewTopic topic, NewPost post)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var category = _categoryService.GetById(id);
                    var userId = _userManager.GetUserId(User);
                    var user = _userManager.FindByIdAsync(userId);

                    topic.Category = category;
                    topic.User = await user;
                    topic.CategoryId = id;

                    if (_context.Topics.Any(t => t.Title == topic.Title))
                    {
                        ModelState.AddModelError("Topic.Title", "Topic with this title already exists");
                        return View(topicandpost);
                    }
                    await _topicService.Add(topic);

                    post.User = await user;
                    post.Topic = topic;

                    if (_context.Posts.Any(p => p.Content == post.Content))
                    {
                        ModelState.AddModelError("Post.Content", "Post with this content already exist");
                        return View(topicandpost);
                    }
                    await _postService.Add(post);

                    return RedirectToAction("PostsByTopic", "Posts", new { id = topic.Id });
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes.");
            }
            return View(topicandpost);
        }

        // GET: Topics/Edit/5
        [Authorize]
        public IActionResult Edit(int id)
        {
            var topic = _topicService.GetById(id);
            var model = new NewTopic
            {
                CategoryId = topic.Category.Id,
                Title = topic.Title,
            };

            if (topic.User.UserName != User.Identity.Name)
            {
                return NotFound();
            }

            if (topic == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTopicAsync(int id, NewTopic topic)
        {
            var topicToUpdate = _topicService.GetById(id);
            if (await TryUpdateModelAsync(topicToUpdate,
                "",
                t => t.Title))
            {
                if (_context.Topics.Any(t => t.Title == topicToUpdate.Title))
                {
                    ModelState.AddModelError("Title", "Topic with this title already exists");
                    return View(topic);
                }
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("TopicsByCategory", "Topics", new { id = topicToUpdate.Category.Id });
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again...");
                }
            }
            return View(topicToUpdate);
        }

        // GET: Topics/Delete/5
        [Authorize]
        public IActionResult Delete(int id, bool? saveChangesError = false)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed";
            }

            var topic = _topicService.GetById(id);
            var model = new DeleteTopic
            {
                CategoryId = topic.Category.Id,
                TopicTitle = topic.Title
            };

            if (topic.User.UserName != User.Identity.Name)
            {
                return NotFound();
            }

            if (topic == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var topicToDelete = _topicService.GetById(id);

            try
            {
                await _topicService.Delete(id);
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id, saveChangesError = true });
            }

            return RedirectToAction("TopicsByCategory", "Topics", new { id = topicToDelete.Category.Id });
        }
    }
}
