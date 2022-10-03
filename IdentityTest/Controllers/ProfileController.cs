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

			users.PageUser.Posts = await IdenTestDb.Posts.Where(p => p.UserId == users.PageUser.Id).Include(c => c.Comments).ThenInclude(c => c.User).ToListAsync();

			if (users.PageUser != null && users.PageUser.Posts != null)
			{
				return View(users);
			}
			else
			{
				return NotFound();
			}
		}

		public async Task<IActionResult> CreateComment(string CommentContent, int PostId, string puName)
		{
			IdenTestUser user = await userManager.GetUserAsync(HttpContext.User);

			if (user == null || string.IsNullOrEmpty(puName))
			{
				return RedirectToAction("Index", "Home");
			}

			Comments comment = new Comments();

			if (!string.IsNullOrEmpty(CommentContent) && PostId != 0) 
			{
				comment.CommentContent = CommentContent;
				comment.PostId = PostId;
				comment.UserId = user.Id;
				comment.CreatedDate = DateTime.Now;

				IdenTestDb.Comments.Add(comment);
				await IdenTestDb.SaveChangesAsync();

				return RedirectToAction("Index", new { Username = puName});
			}
			
			return RedirectToAction("Index", "Home");
		}
	}
}
