using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace sharedCookieRedis.Helper
{
    /// <summary>
    /// 處理各項Authorization行為
    /// </summary>
    public static class AuthorizeManagement
    {
        private static IHttpContextAccessor _contextAccessor;
        private static HttpContext HttpContext => _contextAccessor.HttpContext;

        /// <summary>
        /// 於 StartUp 注入 HttpContext
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 當前登入的使用者
        /// </summary>
        public static AppUser CurrentUser => new AppUser(User);

        /// <summary>
        /// 目前HTTP要求中的安全性資訊(User)
        /// </summary>
        private static IPrincipal User => HttpContext.User;

        /// <summary>
        /// 登出使用者
        /// </summary>
        public static async Task SignOutAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }

        /// <summary>
        /// 登入使用者
        /// </summary>
        public static async Task SignInAsync(string loginId)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, loginId),
                },
                CookieAuthenticationDefaults.AuthenticationScheme);


            AuthenticationProperties authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(15),
                IsPersistent = true,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity), 
                authProperties);
        }
    }

    /// <summary>
    /// App使用者
    /// </summary>
    public sealed class AppUser : ClaimsPrincipal
    {
        public AppUser(IPrincipal claimsPrincipal) : base(claimsPrincipal)
        {
        }

        public string LoginId => this.FindFirst(ClaimTypes.NameIdentifier).Value;

    }

}
