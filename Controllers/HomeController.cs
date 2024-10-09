using Labb_2_Avancerad_fullstackutveckling.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Diagnostics;

namespace Labb_2_Avancerad_fullstackutveckling.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _client;
        private string _baseUri = "https://localhost:7081/api";

        public HomeController(HttpClient client)
        {
            _client = client;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/about")]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet("/menu")]
        public async Task<IActionResult> Menu()
        {
            var response = await _client.GetAsync($"{_baseUri}/MenuItem");
            var json = await response.Content.ReadAsStringAsync();
            var menuItemList = JsonConvert.DeserializeObject<List<MenuItem>>(json);

            return View(menuItemList);
        }

        [HttpGet("/adminportal")]
        [Authorize]
        public IActionResult Admin()
        {
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return View();
        }
    }
}
