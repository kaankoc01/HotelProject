using HotelProject.WebUI.Dtos.GuestDto;
using HotelProject.WebUI.Models.Staff;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace HotelProject.WebUI.Controllers
{
    public class GuestController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GuestController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        // Tüm personel listesini gösteren "Index" action metodu.
        public async Task<IActionResult> Index()
        {

            var client = _httpClientFactory.CreateClient();

            // API'den personel listesini getirmek için GET isteği gönderiliyor.
            var responseMessage = await client.GetAsync("http://localhost:5272/api/Guest");

            // Eğer istek başarılıysa (200 OK), veriler JSON formatında okunup deserialize ediliyor.
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultGuestDto>>(jsonData);
                return View(values);
            }

            return View();
        }

        [HttpGet]
        // Yeni bir personel ekleme formunu döndüren GET metodu.
        public IActionResult AddGuest()
        {
            return View();
        }

        [HttpPost]
        // Yeni personel ekleme işlemini gerçekleştiren POST metodu.
        public async Task<IActionResult> AddGuest(CreateGuestDto createGuestDto)
        {
            if (ModelState.IsValid)
            {


                var client = _httpClientFactory.CreateClient();

                // Kullanıcıdan gelen veriyi JSON formatına dönüştür.
                var jsonData = JsonConvert.SerializeObject(createGuestDto);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // API'ye yeni personel eklemek için POST isteği gönderiliyor.
                var responseMessage = await client.PostAsync("http://localhost:5272/api/Guest", content);


                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View();
            }
            else
            {
                return View();
            }

        }

        // Belirli bir personeli silen action metodu.
        public async Task<IActionResult> DeleteGuest(int id)
        {

            var client = _httpClientFactory.CreateClient();

            // API'ye belirli bir personeli silmek için DELETE isteği gönderiliyor.
            var responseMessage = await client.DeleteAsync($"http://localhost:5272/api/Guest/{id}");


            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }


            return View();
        }

        [HttpGet]
        // Güncellenecek personel verisini getirip forma dolduran GET metodu.
        public async Task<IActionResult> UpdateGuest(int id)
        {

            var client = _httpClientFactory.CreateClient();

            // Güncellenecek personel verisini API'den almak için GET isteği gönderiliyor.
            var responseMessage = await client.GetAsync($"http://localhost:5272/api/Guest/{id}");

            // Eğer istek başarılıysa JSON verisi deserialize edilip modele atanıyor.
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateGuestDto>(jsonData);
                return View(values);
            }


            return View();
        }

        [HttpPost]
        // Güncelleme işlemini gerçekleştiren POST metodu.
        public async Task<IActionResult> UpdateGuest(UpdateGuestDto updateGuestDto)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient();

                // Kullanıcıdan gelen güncelleme verisini JSON formatına dönüştür.
                var jsonData = JsonConvert.SerializeObject(updateGuestDto);
                StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // API'ye PUT isteği gönderiliyor. Güncellenmiş veri ile personel kaydı güncelleniyor.
                var responseMessage = await client.PutAsync("http://localhost:5272/api/Guest/", stringContent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                return View();
            }
            return View();
        }
    }
}
