using Labb_2_Avancerad_fullstackutveckling.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Labb_2_Avancerad_fullstackutveckling.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _client;

        public AccountController(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://localhost:7081");
        }
        [HttpGet("/adminportal/login")]
        public IActionResult Login()
        {
            var token = HttpContext.Request.Cookies["jwtToken"];

            if (token != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return RedirectToAction("Admin", "Home");
            }

            return View();
        }

        [HttpPost("/adminportal/login")]
        public async Task<IActionResult> Login(LoginViewModel loginAdmin)
        {
            var response = await _client.PostAsJsonAsync("/api/Account/Login", loginAdmin);

            if (!response.IsSuccessStatusCode)
            { 
                return View(loginAdmin);
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<TokenResponse>(jsonResponse);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token.Token);

            var claims = jwtToken.Claims.ToList();

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = jwtToken.ValidTo
            });

            HttpContext.Response.Cookies.Append("jwtToken", token.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = jwtToken.ValidTo
            });

            return RedirectToAction("Admin", "Home");
        }

        [HttpPost("/adminportal/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Response.Cookies.Delete("jwtToken");

            return RedirectToAction("Login", "Account");
        }
    }
}
