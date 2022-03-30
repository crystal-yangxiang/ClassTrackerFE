using ClassTrackerFE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace ClassTrackerFE.Controllers
{
    public class AuthController : Controller
    {

        private readonly HttpClient _client;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("ApiClient");
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
            
        }

        [HttpPost]
        public IActionResult Login(UserInfo user)
        {
            // Send the username and password to the API
            HttpResponseMessage result = _client.PostAsJsonAsync("Token/GenerateToken", user).Result;

            if(result.IsSuccessStatusCode)
            {
                // Retrieve the token from the result
                var token = result.Content.ReadAsStringAsync().Result;

                // Tidy up the token and save to the session
                HttpContext.Session.SetString("Token", token.Trim('"'));//trim get ride of the extra "" mark
                
                // Redirect to the Index Page
                return RedirectToAction("Index", "Home");
            }

            return View();
            
        }
    }
}
