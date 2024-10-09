using Labb_2_Avancerad_fullstackutveckling.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Labb_2_Avancerad_fullstackutveckling.Controllers
{
    [Route("/adminportal/reservation")]
    public class BookingController : Controller
    {
        private readonly HttpClient _client;
        private string _baseUri = "https://localhost:7081/api/Booking";

        public BookingController(HttpClient client)
        {
            _client = client;
        }

        [Authorize]
        public async Task<IActionResult> IndexBooking()
        {
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync($"{_baseUri}");
            var json = await response.Content.ReadAsStringAsync();
            var bookingList = JsonConvert.DeserializeObject<List<Booking>>(json);

            return View(bookingList);
        }

        [HttpGet("/adminportal/reservation/details")]
        [Authorize]
        public async Task<IActionResult> DetailsBooking(int bookingId)
        {
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync($"{_baseUri}/{bookingId}");
            var json = await response.Content.ReadAsStringAsync();
            var booking = JsonConvert.DeserializeObject<Booking>(json);

            return View(booking);
        }

        [HttpGet("/adminportal/reservation/add")]
        [Authorize]
        public IActionResult CreateBooking()
        {
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return View();
        }

        [Authorize]
        [HttpPost("/adminportal/reservation/add")]
        public async Task<IActionResult> CreateBooking(Booking booking)
        {
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(booking);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{_baseUri}/Create", content);

            return RedirectToAction("IndexBooking");
        }

        [HttpGet("/adminportal/reservation/edit")]
        [Authorize]
        public async Task<IActionResult> EditBooking(int bookingId)
        {
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync($"{_baseUri}/{bookingId}");
            var json = await response.Content.ReadAsStringAsync();
            var booking = JsonConvert.DeserializeObject<Booking>(json);

            return View(booking);
        }

        [Authorize]
        [HttpPost("/adminportal/reservation/edit")]
        public async Task<IActionResult> EditBooking(Booking booking)
        {
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(booking);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _client.PutAsync($"{_baseUri}/Update", content);

            return RedirectToAction("IndexBooking");
        }

        [Authorize]
        [HttpPost("/adminportal/reservation/delete")]
        public async Task<IActionResult> DeleteBooking(int bookingId)
        {
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.DeleteAsync($"{_baseUri}/Delete/{bookingId}");

            return RedirectToAction("IndexBooking");
        }
    }
}
