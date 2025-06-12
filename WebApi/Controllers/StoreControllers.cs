using Core.DTOs.Store;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreControllers : Controller
    {
        private readonly IStoreService _store;
        private readonly IAddressService _address;
        public StoreControllers(IStoreService store, IAddressService address)
        {
            _store = store;
            _address = address;
        }

        [HttpGet("StoreAll")]
        public async Task<IActionResult> StoreAll()
        {
            var stores = await _store.GetAllAsync();
            if (stores == null)
            {
                return NotFound();
            }
            return Ok(stores);
        }

        [HttpGet("getCountry")]
        public async Task<IActionResult> GetCountry()
        {
            var country = await _address.GetCountry();
            if (country == null)
            {
                return NotFound();
            }
            return Ok(country);
        }

        [HttpGet("getCity")]
        public async Task<IActionResult> GetCity()
        {
            var city = await _address.GetCity();
            if (city == null)
            {
                return NotFound();
            }
            return Ok(city);
        }

        // CRUD Store
        [HttpGet("StoreByPage/{page}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> StoreByPageAsync(int page)
        {
            var stores = await _store.StoreByPageAsync(page);
            if (stores == null)
            {
                return NotFound();
            }
            return Ok(stores);
        }

        [HttpGet("StoreQuantity")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> StoreQuantityAsync()
        {
            var quantity = await _store.StoreQuantityAsync();
            return Ok(quantity);
        }
        
        [HttpGet("StoreById/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetStoreByIDAsync(int id)
        {
            var store = await _store.GetStoreByIDAsync(id);
            if (store == null)
            {
                return NotFound();
            }
            return Ok(store);
        }
        
        [HttpPost("CreateStore")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateAsync(CreateStoreDTO createStoreDTO)
        {
            await _store.CreateAsync(createStoreDTO);
            return Ok();
        }
        
        [HttpPost("EditStore")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditAsync(EditStoreDTO editStoreDTO)
        {
            await _store.EditAsync(editStoreDTO);
            return Ok();
        }

        [HttpDelete("DeleteStore/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteStoreAsync(int id)
        {
            await _store.DeleteAsync(id);
            return Ok();
        }
    }
}
