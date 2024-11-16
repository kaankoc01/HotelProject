using HotelProject.WebUI.Dtos.ContactDto;
using HotelProject.WebUI.Dtos.SendMessageDto;
using HotelProject.WebUI.Models.Staff;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace HotelProject.WebUI.Controllers
{
    public class AdminContactController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminContactController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

       
        public async Task<IActionResult> Inbox()
        {
            
            var client = _httpClientFactory.CreateClient();

            // API'den personel listesini getirmek için GET isteği gönderiliyor.
            var responseMessage = await client.GetAsync("http://localhost:5272/api/Contact");

            // Eğer istek başarılıysa (200 OK), veriler JSON formatında okunup deserialize ediliyor.
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<InboxContactDto>>(jsonData);
                return View(values); 
            }

         
            return View();
        }


        public async Task<IActionResult> Sendbox()
        {

            var client = _httpClientFactory.CreateClient();

            // API'den personel listesini getirmek için GET isteği gönderiliyor.
            var responseMessage = await client.GetAsync("http://localhost:5272/api/SendMessage");

            // Eğer istek başarılıysa (200 OK), veriler JSON formatında okunup deserialize ediliyor.
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultSendboxDto>>(jsonData);
                return View(values);
            }
            return View();
        }
        [HttpGet]
       
        public IActionResult AddSendMessage()
        {
            return View();
        }

        [HttpPost]
     
        public async Task<IActionResult> AddSendMessage(CreateSendMessage createSendMessage)
        {
            createSendMessage.SenderMail = "admin@gmail.com";
            createSendMessage.SenderName = "admin";
            createSendMessage.Date = DateTime.Parse(DateTime.Now.ToShortDateString());
            var client = _httpClientFactory.CreateClient();

            // Kullanıcıdan gelen veriyi JSON formatına dönüştür.
            var jsonData = JsonConvert.SerializeObject(createSendMessage);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // API'ye yeni personel eklemek için POST isteği gönderiliyor.
            var responseMessage = await client.PostAsync("http://localhost:5272/api/SendMessage", content);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("SendBox");
            }
            return View();
        }



        public PartialViewResult SideBarAdminContactPartial()
        {
            return PartialView();
        }
        public PartialViewResult SideBarAdminContactCategoryPartial()
        {
            return PartialView();
        }

        public async Task<IActionResult> MessageDetailsBySendbox(int id)
        {

            var client = _httpClientFactory.CreateClient();

            // Güncellenecek personel verisini API'den almak için GET isteği gönderiliyor.
            var responseMessage = await client.GetAsync($"http://localhost:5272/api/SendMessage/{id}");

            // Eğer istek başarılıysa JSON verisi deserialize edilip modele atanıyor.
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<GetMessageByIdDto>(jsonData);
                return View(values); // Güncelleme formuna model gönderiliyor.
            }

            // Başarısız bir istek durumunda boş bir view döndürülüyor.
            return View();
        }

        public async Task<IActionResult> MessageDetailsByInbox(int id)
        {

            var client = _httpClientFactory.CreateClient();

            // Güncellenecek personel verisini API'den almak için GET isteği gönderiliyor.
            var responseMessage = await client.GetAsync($"http://localhost:5272/api/Contact/{id}");

            // Eğer istek başarılıysa JSON verisi deserialize edilip modele atanıyor.
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<InboxContactDto>(jsonData);
                return View(values); // Güncelleme formuna model gönderiliyor.
            }

            // Başarısız bir istek durumunda boş bir view döndürülüyor.
            return View();
        }
    }
}
