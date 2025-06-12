using Core.DTOs.UserInfo;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BagController : Controller
    {
        private readonly IBagService _bagService;
        private readonly IBagItemsService _bagItemsService;
        public BagController(IBagService bagService, IBagItemsService bagItemsService)
        {
            _bagService = bagService;
            _bagItemsService = bagItemsService;
        }

        [HttpPost("CreateBag")]
        public async Task<IActionResult> CreateBagAsync(BagDTO bagDTO)
        {
            await _bagService.CreateAsync(bagDTO);
            return Ok();
        }

        [HttpDelete("DeleteBag")]
        public async Task<IActionResult> DeleteBagAsync(BagDTO bagDTO)
        {
            await _bagService.DeleteBagByIdAsync(bagDTO.UserEmail);
            return Ok();
        }

        [HttpGet("GetBagByEmail/{email}")]
        public async Task<IActionResult> GetBagByEmailAsync(string email)
        {
            var result = await _bagService.GetBagByUserEmailAsync(email);
            if (result == null)
            {
                return Ok();
            }
            return Ok(result);
        }

        [HttpGet("GetCountBagByEmail/{email}")]
        public async Task<IActionResult> GetCountBagByEmailAsync(string email)
        {
            var result = await _bagService.GetCountBagByEmailAsync(email);
            if (result == null)
            {
                return Ok();
            }
            return Ok(result);
        }

        [HttpGet("GetBagItemsByEmail/{email}")]
        public async Task<IActionResult> GetBagItemsByEmailAsync(string email)
        {
            var result = await _bagItemsService.GetBagItemsByBagEmailAsync(email);
            if (result == null)
            {
                return Ok();
            }
            return Ok(result);
        }

        [HttpDelete("DeleteBagItem/{id}")]
        public async Task<IActionResult> DeleteBagItemAsync(int id)
        {
            await _bagService.DeleteItemByID(id);
            return Ok();
        }

        [HttpPost("SetIncrease/{id}")]
        public async Task<IActionResult> IncreaseAsync(int id)
        {
            await _bagService.IncreaseAsync(id);
            return Ok();
        }

        [HttpPost("SetDecrease/{id}")]
        public async Task<IActionResult> DecreaseAsync(int id)
        {
            await _bagService.DecreaseAsync(id);
            return Ok();
        }
    }
}