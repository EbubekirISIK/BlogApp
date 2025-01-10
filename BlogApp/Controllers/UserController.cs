using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        public UserController(IUserRepository userRepository, IPostRepository postRepository)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;   
        }
        public IActionResult Login()
        {
            if(User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Posts");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isUser = await _userRepository.Users.FirstOrDefaultAsync(x => x.Email == model.Email && x.Password ==model.Password);
                if (isUser !=null)
                {
                    var userClaims = new List<Claim>();
                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier, isUser.UserId.ToString()));
                    userClaims.Add(new Claim(ClaimTypes.Name, isUser.UserName ?? "")); 
                    userClaims.Add(new Claim(ClaimTypes.GivenName, isUser.Name ?? ""));
                    userClaims.Add(new Claim(ClaimTypes.UserData, isUser.Images ?? ""));


                    if (isUser.Email == "info@sadikturan.com") 
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                    }

                    var claimsIdantity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties { IsPersistent = true };

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdantity), authProperties);
                    return RedirectToAction("Index", "Posts");
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya şifre yanlış");
                }
            }
            
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }

        public IActionResult UserCreate(UserCreateViewModel model)
        {
            if (ModelState.IsValid) 
            {
                var users = _userRepository.Users.FirstOrDefault(x => x.Email == model.Email || x.UserName == model.UserName);
                if (users == null) 
                {
                    User user = new User
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        Password = model.Password,
                        Name = model.Name,
                        Images = "avatar3.png"
                    };
                    _userRepository.CreatePost(user);
                    return RedirectToAction("Login");
                }
                
                return View(model);
            }

            return View(model);
        }

        public IActionResult Profile(string profil) 
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var posts = _postRepository.Posts.Include(x =>x.User).Where(x => x.User.UserName == profil).ToList();
            return View(posts);
        }
        
    }
}
