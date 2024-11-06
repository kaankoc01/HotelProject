using HotelProject.WebUI.Dtos.ServiceDto;
using HotelProject.WebUI.Models.Staff;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace HotelProject.WebUI.Controllers
{
    public class ServiceController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public ServiceController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Tüm personel listesini gösteren "Index" action metodu.
        public async Task<IActionResult> Index()
        {
           
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("http://localhost:5272/api/Service");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultServiceDto>>(jsonData);
                return View(values); 
            }
            return View();
        }

        [HttpGet]
        // Yeni bir personel ekleme formunu döndüren GET metodu.
        public IActionResult AddService()
        {
            return View();
        }

        [HttpPost]
        // Yeni personel ekleme işlemini gerçekleştiren POST metodu.
        public async Task<IActionResult> AddService(CreateServiceDto createServiceDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var client = _httpClientFactory.CreateClient();

            var jsonData = JsonConvert.SerializeObject(createServiceDto);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PostAsync("http://localhost:5272/api/Service", content);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        // Belirli bir personeli silen action metodu.
        public async Task<IActionResult> DeleteService(int id)
        {
            var client = _httpClientFactory.CreateClient(); 
            var responseMessage = await client.DeleteAsync($"http://localhost:5272/api/Service/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        // Güncellenecek personel verisini getirip forma dolduran GET metodu.
        public async Task<IActionResult> UpdateService(int id)
        {

            var client = _httpClientFactory.CreateClient();

            // Güncellenecek personel verisini API'den almak için GET isteği gönderiliyor.
            var responseMessage = await client.GetAsync($"http://localhost:5272/api/Service/{id}");

            // Eğer istek başarılıysa JSON verisi deserialize edilip modele atanıyor.
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateServiceDto>(jsonData);
                return View(values); // Güncelleme formuna model gönderiliyor.
            }

            // Başarısız bir istek durumunda boş bir view döndürülüyor.
            return View();
        }

        [HttpPost]
        // Güncelleme işlemini gerçekleştiren POST metodu.
        public async Task<IActionResult> UpdateService(UpdateServiceDto updateServiceDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            // HttpClient örneği oluşturuluyor.
            var client = _httpClientFactory.CreateClient();

            // Kullanıcıdan gelen güncelleme verisini JSON formatına dönüştür.
            var jsonData = JsonConvert.SerializeObject(updateServiceDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // API'ye PUT isteği gönderiliyor. Güncellenmiş veri ile personel kaydı güncelleniyor.
            var responseMessage = await client.PutAsync("http://localhost:5272/api/Service/", stringContent);

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
