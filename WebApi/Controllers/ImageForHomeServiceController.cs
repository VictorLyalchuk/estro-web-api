using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageForHomeServiceController : Controller
    {
        private readonly IImageForHomeService _imageForHome;
        public ImageForHomeServiceController(IImageForHomeService imageForHome)
        {
            _imageForHome = imageForHome;
        }
        
        [HttpGet("GetAllImageAsync")]
        public async Task<IActionResult> GetAllImageAsync()
        {
            var images = await _imageForHome.GetAllImageAsync();
            if (images != null)
            {
                return Ok(images);
            }
            return Ok();
        }
    }
}
