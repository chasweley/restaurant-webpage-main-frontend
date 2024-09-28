using Labb_2_Avancerad_fullstackutveckling.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Labb_2_Avancerad_fullstackutveckling.Controllers
{
    public class MenuController : Controller
    {
        private readonly HttpClient _client;
        private string baseUri = "https://localhost:7127/api/MenuItem";
        
        public MenuController(HttpClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Current menu";

            var response = await _client.GetAsync($"{baseUri}");
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
            var response = await _client.PostAsync($"{baseUri}/Create", content);

            // RedirectToAction("Index", "Home") för annan controller
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _client.GetAsync($"{baseUri}/Update/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var menuItem = JsonConvert.DeserializeObject<MenuItem>(json);

            return View(menuItem);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MenuItem menuItem)
        {
            var json = JsonConvert.SerializeObject(menuItem);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _client.PutAsync($"{baseUri}/Update", content);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _client.DeleteAsync($"{baseUri}/Delete/{id}");

            return RedirectToAction("Index");
        }
    }
}
