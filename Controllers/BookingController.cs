using Labb_2_Avancerad_fullstackutveckling.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Labb_2_Avancerad_fullstackutveckling.Controllers
{
    public class BookingController : Controller
    {
        private readonly HttpClient _client;
        private string baseUri = "https://localhost:7127/api/Booking";

        public BookingController(HttpClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Reservations";

            // Anropar APIets endpoint för att hämta alla filmer
            var response = await _client.GetAsync($"{baseUri}");

            // läser av JSON som en string från bodyn
            var json = await response.Content.ReadAsStringAsync();

            // Omvandlar JSON till ett objekt av typen List<Movie>();
            var menuItemList = JsonConvert.DeserializeObject<List<MenuItem>>(json);

            // returnera listan till vyn som tar emot det för att arbeta med datan.
            return View(menuItemList);
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

            // konvertera objektet till en JSON-string
            var json = JsonConvert.SerializeObject(booking);

            // lägger till JSON-stringen till body-delen av vår begäran och sätter dess header 	 till typen JSON
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            //skickar POST-förfrågan till servern tillsammans med bodyn.
            var response = await _client.PostAsync($"{baseUri}/Create", content);

            // Skickar användaren till en action när operationen är färdig.
            // I det här fallet skickas användaren till "Index".
            // vill vi välja en annan controller ä nuvarande kan vi lägga till namnet så här:
            // RedirectToAction("Index", "Home");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _client.GetAsync($"{baseUri}/Update/{id}");

            var json = await response.Content.ReadAsStringAsync();

            var booking = JsonConvert.DeserializeObject<Booking>(json);

            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Booking booking)
        {
            var json = JsonConvert.SerializeObject(booking);

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
