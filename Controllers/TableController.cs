using Labb_2_Avancerad_fullstackutveckling.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Labb_2_Avancerad_fullstackutveckling.Controllers
{
    public class TableController : Controller
    {
        private readonly HttpClient _client;
        private string _baseUri = "https://localhost:7081/api/Table";

        public TableController(HttpClient client)
        {
            _client = client;

        }
        // Utveckla och incorporera
        public async Task<IActionResult> AvailableTables(string dateTime)
        {
            var response = await _client.GetAsync($"{_baseUri}/Availability/{dateTime}");
            var json = await response.Content.ReadAsStringAsync();
            var listofAvailableTables = JsonConvert.DeserializeObject<List<Table>>(json);

            return View(listofAvailableTables);
        }
    }
}
