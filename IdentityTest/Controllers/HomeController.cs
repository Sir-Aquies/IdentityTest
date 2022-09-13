using IdentityTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Authorization;

namespace IdentityTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdenTestUser> userManager;
        private readonly IPasswordHasher<IdenTestUser> passwordHasher;

        public HomeController(ILogger<HomeController> logger, UserManager<IdenTestUser> userMngr, IPasswordHasher<IdenTestUser> passwordHash)
        {
            _logger = logger;
            userManager = userMngr;
            passwordHasher = passwordHash;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            IdenTestUser user = await userManager.GetUserAsync(HttpContext.User);
            string message = "Hello " + user.UserName;
            return View((object)message);
        }

        public IActionResult ReadAll()
        {
            return View(userManager.Users);
        }

        public ViewResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(UserModel user)  
        {
            if (ModelState.IsValid)
            {
                IdenTestUser itUser = new IdenTestUser
                {
                    UserName = user.Name,
                    Email = user.Email,

                };

                IdentityResult result = await userManager.CreateAsync(itUser, user.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(user);
        }

        public async Task<IActionResult> Update(string id)
        {
            IdenTestUser user = await userManager.FindByIdAsync(id);

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
        public async Task<IActionResult> Update(string id, string email, string password)
        {
            IdenTestUser user = await userManager.FindByIdAsync(id);

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