using Core.DTOs.Storage;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : Controller
    {
        private readonly IStorageService _storage;
        public StorageController(IStorageService storage)
        {
            _storage = storage;
        }
        
        [HttpPost("AddQuantityStorage")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddQuantityStorage(StorageDTO [] storagesDTO)
        {
            await _storage.AddQuantityStorageAsync(storagesDTO);
            return Ok();
        }
    }
}
