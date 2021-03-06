﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TraineeshipForum.Data;
using TraineeshipForum.Models;
using TraineeshipForum.Models.Actions.WithPosts;
using TraineeshipForum.Models.Entities;
using TraineeshipForum.Models.Pages;
using TraineeshipForum.Services.Categories;
using TraineeshipForum.Services.Posts;
using TraineeshipForum.Services.Topics;

namespace TraineeshipForum.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITopic _topicService;
        private readonly ICategory _categoryService;
        private readonly IPost _postService;

        private static UserManager<ApplicationUser> _userManager;

        public PostsController(ApplicationDbContext context, ITopic topicService, ICategory categoryService, UserManager<ApplicationUser> userManager, IPost postService)
        {
            _context = context;
            _topicService = topicService;
            _categoryService = categoryService;
            _userManager = userManager;
            _postService = postService;
        }

        public IActionResult PostsByTopic(int id)
        {
            var model = CreatePostsByTopic(id);
            return View(model);
        }

        public TopicPage CreatePostsByTopic(int id)
        {
            var topic = _topicService.GetById(id);
            var posts = topic.Posts;
            var postListing = posts.Select(post => new PostListing
            {
                AuthorName = post.User.UserName,
                Content = post.Content,
                DatePosted = post.Created.ToString(),
                PostId = post.Id,
                AuthorImageURL = post.User.ProfileImageUrl,
                AuthorId = post.User.Id
            });

            return new TopicPage()
            {
                Posts = postListing,
                TopicTitle = topic.Title,
                CategoryTitle = topic.Category.Title,
                CategoryId = topic.Category.Id,
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
        public IActionResult Create(int id)
        {
            var model = new NewPost
            {
                TopicId = id,
            };
            return View(model);
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Content,Created,UserId")] NewPost post, int id)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var topic = _topicService.GetById(id);
                    var userId = _userManager.GetUserId(User);
                    var user = _userManager.FindByIdAsync(userId).Result;

                    post.User = user;
                    post.Topic = topic;
                    post.TopicId = id;


                    if (_context.Posts.Any(p => p.Content == post.Content))
                    {
                        ModelState.AddModelError("Content", "Post with this content already exist");
                        return View(post);
                    }

                    await _postService.Add(post);

                    return RedirectToAction("PostsByTopic", "Posts", new { id = post.Topic.Id });
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
        public IActionResult Edit(int id)
        {
            var post = _postService.GetById(id);
            var model = new NewPost
            {
                TopicId = post.Topic.Id,
                Content = post.Content
            };

            if (post.User.UserName != User.Identity.Name)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> EditPostAsync(int id, NewPost post)
        {
            var postToUpdate = _postService.GetById(id);
            if (await TryUpdateModelAsync(postToUpdate,
                "",
                p => p.Content))
            {
                if (_context.Posts.Any(p => p.Content == post.Content))
                {
                    ModelState.AddModelError("Content", "Post with this content already exist");
                    return View(post);
                }
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("PostsByTopic", "Posts", new { id = postToUpdate.Topic.Id });
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes.");
                }
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize]
        public IActionResult Delete(int id, bool? saveChangesError = false)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed";
            }

            var post = _postService.GetById(id);
            var model = new DeletePost
            {
                TopicId = post.Topic.Id,
                PostContent = post.Content,
                PostId = post.Id
            };

            if (post.User.UserName != User.Identity.Name)
            {
                return NotFound();
            }

            if (post == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var postToDelete = _postService.GetById(id);

            try
            {
                await _postService.Delete(id);
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id, saveChangesError = true });
            }

            return RedirectToAction("PostsByTopic", "Posts", new { id = postToDelete.Topic.Id });
        }
    }
}
