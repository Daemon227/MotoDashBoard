using DashBoard_MotoManager.Datas;
using DashBoard_MotoManager.Helpers;
using DashBoard_MotoManager.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DashBoard_MotoManager.Controllers
{
    public class UserController : Controller
    {
        private readonly MotoWebsiteContext _db;
        private readonly ILogger<UserController> _logger;

        public UserController(MotoWebsiteContext db, ILogger<UserController> logger) 
        {
            _db = db; 
            _logger = logger;
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(SignUpVM model)
        {
            var user = new User
            {
                UserId = MyTool.GenarateRandomKey(),
                Username = model.Username,
                Password = model.Password,
                Role = "admin"
            };
            _db.Add(user);
            try
            {
                var result = _db.SaveChanges();
                if (result > 0)
                {
                    return RedirectToAction("SignIn", "User");
                }
                else
                {
                    _logger.LogError("Không lưu được, không có hàng nào bị ảnh hưởng.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lưu dữ liệu vào cơ sở dữ liệu.");
                throw; // Ném lại lỗi để dễ dàng kiểm tra khi debug
            }
            return View();
        }

        [HttpGet]
        public IActionResult SignIn(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInVM model,string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                var user = _db.Users.SingleOrDefault(u =>
                u.Username == model.Username);
                if (user == null)
                {
                    ModelState.AddModelError("Loi", "Sai Tai khoan");
                }
                else 
                {
                    if (user.Password != model.Password)
                    {
                        ModelState.AddModelError("Loi", "Sai mat khau");
                    }
                    else
                    {
                        var claim = new List<Claim> {
                            new Claim(ClaimTypes.Name, model.Username),
                            new Claim(ClaimTypes.Role, "admin")
                        };
                        var claimIdentity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return Redirect("/");
                        }
                    }
                }
                
            }
            else
            {
                _logger.LogError("Loi con cax");
            }
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("SignIn", "User");
        }
    }
}
