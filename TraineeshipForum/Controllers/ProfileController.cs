﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TraineeshipForum.Models;
using TraineeshipForum.Models.Entities;
using TraineeshipForum.Services_Interfaces.Upload;
using TraineeshipForum.Services_Interfaces.User;

namespace TraineeshipForum.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUser _userService;
        private readonly IUpload _uploadService;
        private readonly IConfiguration _configuration;

        public ProfileController(UserManager<ApplicationUser> userManager, IApplicationUser userService, IUpload uploadService, IConfiguration configuration)
        {
            _userManager = userManager;
            _userService = userService;
            _uploadService = uploadService;
            _configuration = configuration;
        }

        [Authorize]
        public IActionResult Detail(string id)
        {
            var user = _userService.GetById(id);
            var userRoles = _userManager.GetRolesAsync(user).Result;

            var model = new Profile()
            {
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                DateJoined = user.MemberSince,
                IsAdmin = userRoles.Contains("Admin"),  // remove this
                IsActive = user.IsActive  // remove this
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            var userId = _userManager.GetUserId(User);
            var connectionString = _configuration.GetConnectionString("AzureStorageAccountConnectionString");
            var container = _uploadService.GetBlobContainer(connectionString);

            var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            var filename = Path.Combine(parsedContentDisposition.FileName.Trim('"'));

            var blockBlob = container.GetBlockBlobReference(filename);

            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());
            await _userService.SetProfileImage(userId, blockBlob.Uri);

            return RedirectToAction("Detail", "Profile", new { id = userId });
        }
        public IActionResult Index()
        {
            var profiles = _userService.GetAll()
                .OrderByDescending(user => user.UserName)
                .Select(u => new Profile
                {
                    Email = u.Email,
                    ProfileImageUrl = u.ProfileImageUrl,
                    DateJoined = u.MemberSince,
                    IsActive = u.IsActive, //remove this function
                }); 

            var model = new ProfileListing
            {
                Profiles = profiles
            };

            return View(model);
        }
        public IActionResult Deactivate(string userId) //remove this method
        {
            var user = _userService.GetById(userId);
            _userService.Deactivate(user);
            return RedirectToAction("Index", "Profile");
        }
    }
}