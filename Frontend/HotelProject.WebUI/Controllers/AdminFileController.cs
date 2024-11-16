using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace HotelProject.WebUI.Controllers
{
    public class AdminFileController : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {
            //akışı oluştur
            var stream = new MemoryStream();

            //dosyayı kopyala
            await file.CopyToAsync(stream);

            // akıştak dosyayı bytes olarak tutuyoruz
            var bytes = stream.ToArray();

            ByteArrayContent content = new ByteArrayContent(bytes);

            content.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            MultipartFormDataContent multipartformdataContent = new MultipartFormDataContent();

            multipartformdataContent.Add(content, "file", file.FileName);

            var httpclient = new HttpClient();
            var responseMessage = await httpclient.PostAsync("http://localhost:5272/api/FileProcess", multipartformdataContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return View();
            }
            return View();
        }
    }
}
