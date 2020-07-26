using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using TraineeshipForum.Data;
using TraineeshipForum.Models;
using TraineeshipForum.Models.Pages;
using TraineeshipForum.Services.Topics;

namespace TraineeshipForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ITopic _topicService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, ITopic topicService)
        {
            _logger = logger;
            _context = context;
            _topicService = topicService;
        }

        public IActionResult Index()
        {
            var model = CreateHomePage();
            return View(model);
        }

        public HomePage CreateHomePage()
        {
            var category = _context.Categories.ToList();
            var topic = _topicService.GetLatestTopics(3).Select(topic => new TopicListing
            {
                CategoryId = topic.Category.Id,
                TopicId = topic.Id,
                CategoryTitle = topic.Category.Title,
                TopicTitle = topic.Title,
                PostCount = topic.Posts.Count(),
                DateCreated = topic.Created.ToString(),
                LastPostCreated = topic.Posts.OrderByDescending(post => post.Created).FirstOrDefault().Created.ToString(),
                TimeFromLastPost = Math.Round((DateTime.Now - topic.Posts.OrderByDescending(post => post.Created).FirstOrDefault().Created).TotalDays, 0, MidpointRounding.AwayFromZero)
            });

            return new HomePage()
            {
                Categories = category,
                Topics = topic
            };
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
