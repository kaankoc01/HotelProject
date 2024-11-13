using HotelProject.WebUI.Dtos.RoomDto;
using HotelProject.WebUI.Models.Staff;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace HotelProject.WebUI.Controllers
{
    public class AdminRoomController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminRoomController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        // Tüm personel listesini gösteren "Index" action metodu.
        public async Task<IActionResult> Index()
        {
            // HttpClient örneği oluşturuluyor.
            var client = _httpClientFactory.CreateClient();

            // API'den personel listesini getirmek için GET isteği gönderiliyor.
            var responseMessage = await client.GetAsync("http://localhost:5272/api/Room");

            // Eğer istek başarılıysa (200 OK), veriler JSON formatında okunup deserialize ediliyor.
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultRoomDto>>(jsonData);
                return View(values); // Personel listesini view'e gönderiyoruz.
            }

            // Başarısız bir istek durumunda boş bir view döndürülüyor.
            return View();
        }

        [HttpGet]
        // Yeni bir personel ekleme formunu döndüren GET metodu.
        public IActionResult AddRoom()
        {
            return View();
        }

        [HttpPost]
        // Yeni personel ekleme işlemini gerçekleştiren POST metodu.
        public async Task<IActionResult> AddRoom(CreateRoomDto createRoomDto)
        {
            // HttpClient örneği oluşturuluyor.
            var client = _httpClientFactory.CreateClient();

            // Kullanıcıdan gelen veriyi JSON formatına dönüştür.
            var jsonData = JsonConvert.SerializeObject(createRoomDto);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // API'ye yeni personel eklemek için POST isteği gönderiliyor.
            var responseMessage = await client.PostAsync("http://localhost:5272/api/Room", content);

            // Eğer istek başarılıysa Index action'una yönlendiriliyor.
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            // Başarısız bir istek durumunda aynı view döndürülüyor.
            return View();
        }

        // Belirli bir personeli silen action metodu.
        public async Task<IActionResult> DeleteRoom(int id)
        {
            // HttpClient örneği oluşturuluyor.
            var client = _httpClientFactory.CreateClient();

            // API'ye belirli bir personeli silmek için DELETE isteği gönderiliyor.
            var responseMessage = await client.DeleteAsync($"http://localhost:5272/api/Room/{id}");

            // Eğer istek başarılıysa Index action'una yönlendiriliyor.
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            // Başarısız bir istek durumunda aynı view döndürülüyor.
            return View();
        }

        [HttpGet]
        // Güncellenecek personel verisini getirip forma dolduran GET metodu.
        public async Task<IActionResult> UpdateRoom(int id)
        {
            // HttpClient örneği oluşturuluyor.
            var client = _httpClientFactory.CreateClient();

            // Güncellenecek personel verisini API'den almak için GET isteği gönderiliyor.
            var responseMessage = await client.GetAsync($"http://localhost:5272/api/Room/{id}");

            // Eğer istek başarılıysa JSON verisi deserialize edilip modele atanıyor.
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateRoomDto>(jsonData);
                return View(values); // Güncelleme formuna model gönderiliyor.
            }

            // Başarısız bir istek durumunda boş bir view döndürülüyor.
            return View();
        }

        [HttpPost]
        // Güncelleme işlemini gerçekleştiren POST metodu.
        public async Task<IActionResult> UpdateRoom(UpdateRoomDto updateRoomDto)
        {
            // HttpClient örneği oluşturuluyor.
            var client = _httpClientFactory.CreateClient();

            // Kullanıcıdan gelen güncelleme verisini JSON formatına dönüştür.
            var jsonData = JsonConvert.SerializeObject(updateRoomDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // API'ye PUT isteği gönderiliyor. Güncellenmiş veri ile personel kaydı güncelleniyor.
            var responseMessage = await client.PutAsync("http://localhost:5272/api/Room/", stringContent);

            // Eğer istek başarılıysa Index action'una yönlendiriliyor.
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            // Başarısız bir istek durumunda aynı view döndürülüyor.
            return View();
        }
    }
}
