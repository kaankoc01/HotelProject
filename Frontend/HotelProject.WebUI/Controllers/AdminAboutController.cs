using HotelProject.WebUI.Dtos.AboutDto;
using HotelProject.WebUI.Dtos.BookingDto;
using HotelProject.WebUI.Models.Staff;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace HotelProject.WebUI.Controllers
{
    public class AdminAboutController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminAboutController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("http://localhost:5272/api/About");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultAboutDto>>(jsonData);
                return View(values);
            }
            return View();
        }


        [HttpGet]
        // Güncellenecek personel verisini getirip forma dolduran GET metodu.
        public async Task<IActionResult> UpdateAbout(int id)
        {
           
            var client = _httpClientFactory.CreateClient();

            // Güncellenecek personel verisini API'den almak için GET isteği gönderiliyor.
            var responseMessage = await client.GetAsync($"http://localhost:5272/api/About/{id}");

            // Eğer istek başarılıysa JSON verisi deserialize edilip modele atanıyor.
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateAboutDto>(jsonData);
                return View(values); // Güncelleme formuna model gönderiliyor.
            }

            
            return View();
        }

        [HttpPost]
        
        public async Task<IActionResult> UpdateAbout(UpdateAboutDto updateAboutDto)
        {
            
            var client = _httpClientFactory.CreateClient();

            // Kullanıcıdan gelen güncelleme verisini JSON formatına dönüştür.
            var jsonData = JsonConvert.SerializeObject(updateAboutDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // API'ye PUT isteği gönderiliyor. Güncellenmiş veri ile personel kaydı güncelleniyor.
            var responseMessage = await client.PutAsync("http://localhost:5272/api/About/", stringContent);

            
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

         
            return View();
        }
    }
}
