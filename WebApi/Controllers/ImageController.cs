using Core.DTOs.Image;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : Controller
    {
        private readonly IImageService _image;
        public ImageController(IImageService image)
        {
            _image = image;
        }
        
        [HttpPost("CreateImage")]
        public async Task<IActionResult> CreateImage([FromForm] IFormFile ImageFile)
        {
            var result = await _image.CreateImageAsync(ImageFile);
            return Ok(result);
        }
        
        [HttpPost("DeleteImage")]
        public async Task<IActionResult> DeleteImage(ImageDTO imageDTO)
        {
            await _image.DeleteImageAsync(imageDTO);
            return Ok();
        }

        [HttpPost("CreateUserImage")]
        public async Task<IActionResult> CreateUserImageAsync([FromForm] IFormFile ImageFile)
        {
            var result = await _image.CreateUserImageAsync(ImageFile);
            return Ok(result);
        }
        
        [HttpPost("DeleteUserImage")]
        public async Task<IActionResult> DeleteUserImageAsync([FromForm] string image)
        {
            await _image.DeleteUserImageAsync(image);
            return Ok();
        }
                
        [HttpPost("CreateCategoryImage")]
        public async Task<IActionResult> CreateCategoryImageAsync([FromForm] IFormFile ImageFile)
        {
            var result = await _image.CreateCategoryImageAsync(ImageFile);
            return Ok(result);
        }
        
        [HttpPost("DeleteCategoryImage")]
        public async Task<IActionResult> DeleteCategoryImageAsync([FromForm] string ImagePath)
        {
            await _image.DeleteCategoryImageAsync(ImagePath);
            return Ok();
        }
    }
}
