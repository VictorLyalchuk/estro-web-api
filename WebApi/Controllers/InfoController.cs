using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : Controller
    {
        private readonly IInfoService _info;
        public InfoController(IInfoService info)
        {
            _info = info;
        }
        
        [HttpGet("GetInfo")]
        public async Task<IActionResult> GetInfoAsync()
        {
            var filters = await _info.GetInfoAsync();
            if (filters == null)
            {
                return NotFound();
            }
            return Ok(filters);
        }
        
        [HttpGet("GetSeason")]
        public async Task<IActionResult> GetSeasonAsync()
        {
            var filters = await _info.GetSeason();
            if (filters == null)
            {
                return NotFound();
            }
            return Ok(filters);
        }
        
        [HttpGet("GetColors")]
        public async Task<IActionResult> GetColorsAsync()
        {
            var colors = await _info.GetColors();
            if (colors == null)
            {
                return NotFound();
            }
            return Ok(colors);
        }
        
        [HttpGet("GetMaterials")]
        public async Task<IActionResult> GetMaterialsAsync()
        {
            var materials = await _info.GetMaterials();
            if (materials == null)
            {
                return NotFound();
            }
            return Ok(materials);
        }
        
        [HttpGet("GetSizes")]
        public async Task<IActionResult> GetSizesAsync()
        {
            var sizes = await _info.GetSizes();
            if (sizes == null)
            {
                return NotFound();
            }
            return Ok(sizes);
        }
    }
}
