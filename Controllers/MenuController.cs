using Labb_2_Avancerad_fullstackutveckling.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Labb_2_Avancerad_fullstackutveckling.Controllers
{
    public class MenuController : Controller
    {
        private readonly HttpClient _client;
        private string _baseUri = "https://localhost:7081/api/MenuItem";
        
        public MenuController(HttpClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Current menu";

            var response = await _client.GetAsync($"{_baseUri}");
            var json = await response.Content.ReadAsStringAsync();
            var menuItemList = JsonConvert.DeserializeObject<List<MenuItem>>(json);

            return View(menuItemList);
        }

        public IActionResult Create()
        {
            ViewData["Title"] = "New menu item";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MenuItem menuItem)
        {
            if (!ModelState.IsValid) { return View(menuItem); }

            var json = JsonConvert.SerializeObject(menuItem);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{_baseUri}/Create", content);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int menuItemId)
        { 
            var response = await _client.GetAsync($"{_baseUri}/{menuItemId}");
            var json = await response.Content.ReadAsStringAsync();
            var menuItem = JsonConvert.DeserializeObject<MenuItem>(json);

            return View(menuItem);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MenuItem menuItem)
        {
            var json = JsonConvert.SerializeObject(menuItem);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _client.PutAsync($"{_baseUri}/Update", content);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int menuItemId)
        {
            var response = await _client.DeleteAsync($"{_baseUri}/Delete/{menuItemId}");

            return RedirectToAction("Index");
        }
    }
}
