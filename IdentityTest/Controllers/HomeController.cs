using IdentityTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace IdentityTest.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<IdenTestUser> userManager;
		private readonly IPasswordHasher<IdenTestUser> passwordHasher;
		private readonly IdenTestDbContext IdenTestDb;

		public HomeController(ILogger<HomeController> logger, UserManager<IdenTestUser> userMngr, IPasswordHasher<IdenTestUser> passwordHash, IdenTestDbContext ITDbContext)
		{
			_logger = logger;
			userManager = userMngr;
			passwordHasher = passwordHash;
			IdenTestDb = ITDbContext;
		}

		public async Task<IActionResult> Index()
		{
			IdenTestUser user = await userManager.GetUserAsync(HttpContext.User);
			user.Posts = await IdenTestDb.Posts.Where(p => p.UserId == user.Id).ToListAsync();

			return View(user);
		}

		public async Task<IActionResult> CreatePost()
		{
			IdenTestUser user = await userManager.GetUserAsync(HttpContext.User);

			return View(user);
		}

		[HttpPost]
		public async Task<IActionResult> CreatePost(string Content, IFormFile Media)
		{
			IdenTestUser user = await userManager.GetUserAsync(HttpContext.User);
			Post post = new Post();

			if (!string.IsNullOrEmpty(Content))
			{
				post.PostContent = Content;
			}

			if (Media != null)
			{
				post.Media =  Convert.ToBase64String(await GetBytes(Media));
			}

			if (!string.IsNullOrEmpty(post.PostContent) || post.Media != null)
			{
				post.UserId = user.Id;
				IdenTestDb.Posts.Add(post);
				await IdenTestDb.SaveChangesAsync();
			}

			return View(user);
		}

		public async Task<IActionResult> Update()
		{
			IdenTestUser user = await userManager.GetUserAsync(HttpContext.User);

			if (user != null)
			{
				return View(user);
			}
			else
			{
				return RedirectToAction("Index");
			}
		}

		[HttpPost]
		public async Task<IActionResult> Update(string email, string password)
		{
			IdenTestUser user = await userManager.GetUserAsync(HttpContext.User);

			if (user != null)
			{
				if (!string.IsNullOrEmpty(email))
				{
					user.Email = email;
				}
				else
				{
					ModelState.AddModelError("", "Email cannot be empty");
				}

				if (!string.IsNullOrEmpty(password))
					user.PasswordHash = passwordHasher.HashPassword(user, password);
				else
					ModelState.AddModelError("", "Password cannot be empty");

				if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
				{
					IdentityResult result = await userManager.UpdateAsync(user);
					if (result.Succeeded)
						return RedirectToAction("Index");
					else
						Errors(result);
				}
			}
			else
				ModelState.AddModelError("", "User Not Found");
			return View(user);

		}

		public async Task<IActionResult> AllUsers()
		{
			IdenTestUser user = await userManager.GetUserAsync(HttpContext.User);
			
			return View(user);
		}

		public async Task<IActionResult> GetAllUsers()
		{
			IdenTestUser user =  await userManager.GetUserAsync(HttpContext.User);
			IQueryable<IdenTestUser> userList = userManager.Users;

			return PartialView("UsersTable", userList);
		}

		private void Errors(IdentityResult result)
		{
			foreach (IdentityError error in result.Errors)
				ModelState.AddModelError("", error.Description);
		}

		//[HttpPost]
		//public async Task<IActionResult> Delete(string id)
		//{
		//    IdenTestUser user = await userManager.FindByIdAsync(id);
		//    if (user != null)
		//    {
		//        IdentityResult result = await userManager.DeleteAsync(user);
		//        if (result.Succeeded)
		//            return RedirectToAction("Index");
		//        else
		//            Errors(result);
		//    }
		//    else
		//        ModelState.AddModelError("", "User Not Found");
		//    return View("Index", userManager.Users);
		//}

		private async Task<byte[]> GetBytes(IFormFile formFile)
		{
			await using var memoryStream = new MemoryStream();
			await formFile.CopyToAsync(memoryStream);
			return memoryStream.ToArray();
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