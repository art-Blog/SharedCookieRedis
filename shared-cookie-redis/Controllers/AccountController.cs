using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sharedCookieRedis.Helper;
using SharedCookieRedis.Models;

namespace SharedCookieRedis.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            var name = HttpContext.User.Identity.IsAuthenticated
                ? AuthorizeManagement.CurrentUser.LoginId
                : "not login";

            ViewBag.name = name;
            return View();
        }

        public async Task<JsonResult> Login(LoginRequest form)
        {
            await AuthorizeManagement.SignOutAsync();
            if (form.LoginId != "test" || form.Password != "1234") return Json(new {result = false});
            
            await AuthorizeManagement.SignInAsync(form.LoginId);
            return Json(new {result = true});

        }
    }
}