using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelProject.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileImageController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile File)
        {
            var fileName = Guid.NewGuid()+Path.GetExtension(File.FileName);

            var path = Path.Combine(Directory.GetCurrentDirectory(), "images/" + fileName);

            var stream = new FileStream(path, FileMode.Create);

            await File.CopyToAsync(stream);

            return Created("",File);

        }
    }
}
