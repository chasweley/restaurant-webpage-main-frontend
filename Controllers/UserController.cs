using Labb_2_Avancerad_fullstackutveckling.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Labb_2_Avancerad_fullstackutveckling.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _client;
        private string _baseUri = "https://localhost:7081/api/User";

        public UserController(HttpClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Reservations";

            var response = await _client.GetAsync($"{_baseUri}");
            var json = await response.Content.ReadAsStringAsync();
            var bookingList = JsonConvert.DeserializeObject<List<Booking>>(json);

            return View(bookingList);
        }

        public IActionResult Create()
        {
            ViewData["Title"] = "New reservation";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (!ModelState.IsValid) { return View(booking); }

            var json = JsonConvert.SerializeObject(booking);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{_baseUri}/Create", content);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int bookingId)
        {
            var response = await _client.GetAsync($"{_baseUri}/{bookingId}");
            var json = await response.Content.ReadAsStringAsync();
            var booking = JsonConvert.DeserializeObject<Booking>(json);

            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Booking booking)
        {
            var json = JsonConvert.SerializeObject(booking);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _client.PutAsync($"{_baseUri}/Update", content);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int bookingId)
        {
            var response = await _client.DeleteAsync($"{_baseUri}/Delete/{bookingId}");

            return RedirectToAction("Index");
        }
    }
}
