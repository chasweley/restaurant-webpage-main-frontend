using Labb_2_Avancerad_fullstackutveckling.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Labb_2_Avancerad_fullstackutveckling.Controllers
{
    [Route("/adminportal/menu")]
    public class MenuController : Controller
    {
        private readonly HttpClient _client;
        private string _baseUri = "https://localhost:7081/api/MenuItem";
        
        public MenuController(HttpClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> IndexMenu()
        {
            var response = await _client.GetAsync($"{_baseUri}");
            var json = await response.Content.ReadAsStringAsync();
            var menuItemList = JsonConvert.DeserializeObject<List<MenuItem>>(json);

            return View(menuItemList);
        }

        [HttpGet("/adminportal/menu/details")]
        [Authorize]
        public async Task<IActionResult> DetailsMenu(int menuItemId)
        {
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync($"{_baseUri}/{menuItemId}");
            var json = await response.Content.ReadAsStringAsync();
            var menuItem = JsonConvert.DeserializeObject<MenuItem>(json);
            return View(menuItem);
        }

        [HttpGet("/adminportal/menu/add")]
        [Authorize]
        public IActionResult CreateMenu()
        {
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return View();
        }

        [HttpPost("/adminportal/menu/add")]
        [Authorize]
        public async Task<IActionResult> CreateMenu(MenuItem menuItem)
        {
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (!ModelState.IsValid) { return View(menuItem); }

            var json = JsonConvert.SerializeObject(menuItem);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{_baseUri}/Create", content);

            return RedirectToAction("IndexMenu");
        }

        [HttpGet("/adminportal/menu/edit")]
        [Authorize]
        public async Task<IActionResult> EditMenu(int menuItemId)
        {
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync($"{_baseUri}/{menuItemId}");
            var json = await response.Content.ReadAsStringAsync();
            var menuItem = JsonConvert.DeserializeObject<MenuItem>(json);

            return View(menuItem);
        }

        [HttpPost("/adminportal/menu/edit")]
        [Authorize]
        public async Task<IActionResult> EditMenu(MenuItem menuItem)
        {
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(menuItem);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _client.PutAsync($"{_baseUri}/Update", content);

            return RedirectToAction("IndexMenu");
        }

        [HttpPost("/adminportal/menu/delete")]
        [Authorize]
        public async Task<IActionResult> DeleteMenu(int menuItemId)
        {
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.DeleteAsync($"{_baseUri}/Delete/{menuItemId}");

            return RedirectToAction("IndexMenu");
        }
    }
}
