using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using Simulation2.Models;
using Simulation2.Utilities.Enums;
using Simulation2.ViewModels.Account;
using System.Threading.Tasks;

namespace Simulation2.Controllers
{
    public class AccountController : Controller
    {
        public readonly UserManager<AppUser> _userManager;
        public readonly SignInManager<AppUser> _signInManager;
        public RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(); 
            }

            AppUser appUser = new AppUser
            {
                FirstName = registerVM.FirstName,
                Email = registerVM.Email,
                UserName = registerVM.UserName,
                LastName = registerVM.LastName,
            };

            IdentityResult result = await _userManager.CreateAsync(appUser, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(registerVM);
            }

            await _signInManager.SignInAsync(appUser, isPersistent: false);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            AppUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginVM.UserNameOrEmail || u.Email == loginVM.UserNameOrEmail);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "İstifadəçi adı və ya Email yanlışdır.");
                return View(loginVM);
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, isPersistent: false, lockoutOnFailure: true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Hesabınız bloklanıb. Zəhmət olmasa bir az gözləyin.");
                return View(loginVM);
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Şifrə yanlışdır.");
                return View(loginVM);
            }

            return RedirectToAction("Index", "Home");
        }


            public async Task<IActionResult> CreateRole()
        {
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
                }
            }
            return RedirectToAction(nameof(HomeController.Index), "Index");
        }
    }
}
