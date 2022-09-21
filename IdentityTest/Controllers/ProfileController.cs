using IdentityTest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;

namespace IdentityTest.Controllers
{
	public class ProfileController : Controller
	{
		private readonly UserManager<IdenTestUser> userManager;
		private readonly IPasswordHasher<IdenTestUser> passwordHasher;
		private readonly IdenTestDbContext IdenTestDb;

		public ProfileController(UserManager<IdenTestUser> userMngr, IPasswordHasher<IdenTestUser> passwordHash, IdenTestDbContext ITDbContext)
		{
			userManager = userMngr;
			passwordHasher = passwordHash;
			IdenTestDb = ITDbContext;
		}

		public async Task<IActionResult> Index(string Username)
		{
			UserView users = new UserView();
			users.CurentUser = await userManager.GetUserAsync(HttpContext.User);
            users.PageUser = await userManager.FindByNameAsync(Username);

            users.PageUser.Posts = await IdenTestDb.Posts.Where(p => p.UserId == users.PageUser.Id).ToListAsync();

            if (users.PageUser != null && users.PageUser.Posts != null)
			{
				return View(users);
			}
			else
			{
				return NotFound();
			}
		}
	}
}
