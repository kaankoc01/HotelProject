using HotelProject.WebUI.Models.Staff;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace HotelProject.WebUI.Controllers
{
    public class StaffController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        // IHttpClientFactory'yi dependency injection ile aldık.
        // Bu, daha güvenli ve yönetilebilir HttpClient örnekleri oluşturmamıza olanak tanır.
        public StaffController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Tüm personel listesini gösteren "Index" action metodu.
        public async Task<IActionResult> Index()
        {
            // HttpClient örneği oluşturuluyor.
            var client = _httpClientFactory.CreateClient();

            // API'den personel listesini getirmek için GET isteği gönderiliyor.
            var responseMessage = await client.GetAsync("http://localhost:5272/api/Staff");

            // Eğer istek başarılıysa (200 OK), veriler JSON formatında okunup deserialize ediliyor.
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<StaffViewModel>>(jsonData);
                return View(values); // Personel listesini view'e gönderiyoruz.
            }

            // Başarısız bir istek durumunda boş bir view döndürülüyor.
            return View();
        }

        [HttpGet]
        // Yeni bir personel ekleme formunu döndüren GET metodu.
        public IActionResult AddStaff()
        {
            return View();
        }

        [HttpPost]
        // Yeni personel ekleme işlemini gerçekleştiren POST metodu.
        public async Task<IActionResult> AddStaff(AddStaffViewModel addStaffViewModel)
        {
            // HttpClient örneği oluşturuluyor.
            var client = _httpClientFactory.CreateClient();

            // Kullanıcıdan gelen veriyi JSON formatına dönüştür.
            var jsonData = JsonConvert.SerializeObject(addStaffViewModel);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // API'ye yeni personel eklemek için POST isteği gönderiliyor.
            var responseMessage = await client.PostAsync("http://localhost:5272/api/Staff", content);

            // Eğer istek başarılıysa Index action'una yönlendiriliyor.
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            // Başarısız bir istek durumunda aynı view döndürülüyor.
            return View();
        }

        // Belirli bir personeli silen action metodu.
        public async Task<IActionResult> DeleteStaff(int id)
        {
            // HttpClient örneği oluşturuluyor.
            var client = _httpClientFactory.CreateClient();

            // API'ye belirli bir personeli silmek için DELETE isteği gönderiliyor.
            var responseMessage = await client.DeleteAsync($"http://localhost:5272/api/Staff/{id}");

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
        public async Task<IActionResult> UpdateStaff(int id)
        {
            // HttpClient örneği oluşturuluyor.
            var client = _httpClientFactory.CreateClient();

            // Güncellenecek personel verisini API'den almak için GET isteği gönderiliyor.
            var responseMessage = await client.GetAsync($"http://localhost:5272/api/Staff/{id}");

            // Eğer istek başarılıysa JSON verisi deserialize edilip modele atanıyor.
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateStaffViewModel>(jsonData);
                return View(values); // Güncelleme formuna model gönderiliyor.
            }

            // Başarısız bir istek durumunda boş bir view döndürülüyor.
            return View();
        }

        [HttpPost]
        // Güncelleme işlemini gerçekleştiren POST metodu.
        public async Task<IActionResult> UpdateStaff(UpdateStaffViewModel updateStaffViewModel)
        {
            // HttpClient örneği oluşturuluyor.
            var client = _httpClientFactory.CreateClient();

            // Kullanıcıdan gelen güncelleme verisini JSON formatına dönüştür.
            var jsonData = JsonConvert.SerializeObject(updateStaffViewModel);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // API'ye PUT isteği gönderiliyor. Güncellenmiş veri ile personel kaydı güncelleniyor.
            var responseMessage = await client.PutAsync("http://localhost:5272/api/Staff/", stringContent);

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
