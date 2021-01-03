using System.Threading.Tasks;
using McuServerApi.core;
using Microsoft.AspNetCore.Mvc;

namespace McuServerApi.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageSaver imageSaver;

        public ImageController(IImageSaver imageSaver)
        {
            this.imageSaver = imageSaver;
        }

        [HttpPost]
        [Route("image/report")]
        public async Task<IActionResult> SaveImage([FromBody] ImageInfo imageInfo)
        {
            await imageSaver.SaveImage(imageInfo);
            return Ok();
        }
    }
}