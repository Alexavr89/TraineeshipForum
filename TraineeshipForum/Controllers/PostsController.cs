using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TraineeshipForum.Data;
using TraineeshipForum.Models;
using TraineeshipForum.Models.Actions.WithPosts;
using TraineeshipForum.Models.Pages;
using TraineeshipForum.Services_Interfaces.Categories;
using TraineeshipForum.Services_Interfaces.Topics;

namespace TraineeshipForum.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITopic _topicService;
        private readonly ICategory _categoryService;

        private static UserManager<IdentityUser> _userManager;

        public PostsController(ApplicationDbContext context, ITopic topicService, ICategory categoryService, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _topicService = topicService;
            _categoryService = categoryService;
            _userManager = userManager;
        }

        public IActionResult PostsByTopic(int id, int categoryId) //1.06
        {
            var model = CreatePostsByTopic(id, categoryId);

            return View(model);
        }

        public TopicPage CreatePostsByTopic(int id, int categoryId) //1.06
        {
            var topic = _topicService.GetById(id);
            var posts = topic.Posts;
            var category = _categoryService.GetById(categoryId); //1.06
            var postListing = posts.Select(post => new PostListing
            {
                AuthorName = post.User.UserName,
                Content = post.Content,
                DatePosted = post.Created.ToString(),
                PostId = post.Id,
                TopicId = topic.Id,
                CategoryId = category.Id
                //     AuthorImageURL = post                 // I will finish with avatars later
            });

            return new TopicPage()
            {
                Posts = postListing,
                TopicTitle = topic.Title,
                CategoryTitle = category.Title, //1.06
                CategoryId = category.Id,
                TopicId = topic.Id
            };
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Posts.ToListAsync());
        }

        // GET: Posts/Create
        [Authorize]
        public IActionResult Create(int id, int categoryId)
        {
            var model = new NewPost
            {
                CategoryId = categoryId,
                TopicId = id,
            };
            return View(model);
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Content,Created,UserId")] NewPost post, int id, int categoryId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = _userManager.GetUserId(User);
                    var user = _userManager.FindByIdAsync(userId).Result;
                    var topic = _topicService.GetById(id);

                    post.Topic = topic;
                    post.User = user;
                    post.Created = DateTime.Now;

                    _context.Add(post);
                    _context.SaveChanges();


                    return RedirectToAction("PostsByTopic", "Posts", new { id, categoryId });
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again...");
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id, int topicId, int categoryId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            var model = new NewPost
            {
                TopicId = topicId,
                CategoryId = categoryId,
                Content = post.Content
            };

            if (post == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPostAsync(int? id, int topicId, int categoryId)
        {
            if (id == null)
            {
                return NotFound();
            }
            var postToUpdate = _context.Posts.Find(id);
            if (await TryUpdateModelAsync(postToUpdate,
                "",
                p => p.Content))
            {
                try
                {
                    _context.SaveChanges();
                    return RedirectToAction("PostsByTopic", "Posts", new { id = topicId, categoryId });
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes.");
                }
            }
            return View(postToUpdate);
        }

        // GET: Posts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int topicId, int categoryId, int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed";
            }
            var post = await _context.Posts.FindAsync(id);
            var model = new DeletePost
            {
                CategoryId = categoryId,
                TopicId = topicId,
                PostContent = post.Content
            };

            if (post == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int TopicId, int CategoryId)
        {
            try
            {
                var postToDelete = await _context.Posts.FindAsync(id);

                _context.Entry(postToDelete).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id, saveChangesError = true });
            }
            return RedirectToAction("PostsByTopic", "Posts", new { id = TopicId, categoryId = CategoryId });
        }
    }
}
