using Core.DTOs.Category;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryControllers : Controller
    {
        private readonly ICategoryService _category;
        public CategoryControllers(ICategoryService category)
        {
            _category = category;
        }

        [HttpGet("MainCategoryGetAsync")]
        public async Task<IActionResult> MainCategoryGetAsync()
        {
            return Ok(await _category.MainGetAllAsync());
        }

        [HttpGet("SubCategoryGetAsync")]
        public async Task<IActionResult> SubCategoryGetAsync()
        {
            return Ok(await _category.SubGetAllAsync());
        }

        [HttpGet("CategoryGetAsync")]
        public async Task<IActionResult> CategoryGetAllAsync()
        {
            return Ok(await _category.CategoryGetAllAsync());
        }

        [HttpGet("CategoryGetWithSub")]
        public async Task<IActionResult> CategoryGetWithSubAsync([FromQuery] string subName)
        {
            return Ok(await _category.CategoryGetWithSubAsync(subName));
        }

        // CRUD Main Category
        [HttpGet("MainCategoryByPage/{page}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> MainCategoryByPageAsync(int page)
        {
            var mainCategory = await _category.MainCategoryByPageAsync(page);
            if (mainCategory == null)
            {
                return NotFound();
            }
            return Ok(mainCategory);
        }

        [HttpGet("GetMainCategoryById/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetMainCategoryByIdAsync(int id)
        {
            var mainCategory = await _category.GetMainCategoryByIdAsync(id);
            if (mainCategory == null)
            {
                return NotFound();
            }
            return Ok(mainCategory);
        }

        [HttpGet("MainCategoryQuantity")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> MainCategoryQuantityAsync()
        {
            var quantity = await _category.MainCategoryQuantityAsync();
            return Ok(quantity);
        }

        [HttpPost("CreateMainCategory")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateMainCategoryAsync(CreateMainCategoryDTO createMainCategoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _category.CreateMainCategoryAsync(createMainCategoryDTO);
            return Ok();
        }

        [HttpPost("EditMainCategory")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditMainCategoryAsync(EditMainCategoryDTO editMainCategoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _category.EditMainCategoryAsync(editMainCategoryDTO);
            return Ok();
        }

        [HttpDelete("DeleteMainCategoryByID/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteMainCategoryByIDAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _category.DeleteMainCategoryByIDAsync(id);
            return Ok();
        }

        // CRUD Sub Category
        [HttpGet("SubCategoryByPage/{page}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> SubCategoryByPageAsync(int page)
        {
            var subCategory = await _category.SubCategoryByPageAsync(page);
            if (subCategory == null)
            {
                return NotFound();
            }
            return Ok(subCategory);
        }

        [HttpGet("GetSubCategoryById/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetSubCategoryByIdAsync(int id)
        {
            var subCategory = await _category.GetSubCategoryByIdAsync(id);
            if (subCategory == null)
            {
                return NotFound();
            }
            return Ok(subCategory);
        }

        [HttpGet("SubCategoryQuantity")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> SubCategoryQuantityAsync()
        {
            var quantity = await _category.SubCategoryQuantityAsync();
            return Ok(quantity);
        }

        [HttpPost("CreateSubCategory")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateSubCategoryAsync(CreateSubCategoryDTO createSubCategoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _category.CreateSubCategoryAsync(createSubCategoryDTO);
            return Ok();
        }

        [HttpPost("EditSubCategory")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditSubCategoryAsync(EditSubCategoryDTO editSubCategoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _category.EditSubCategoryAsync(editSubCategoryDTO);
            return Ok();
        }

        [HttpDelete("DeleteSubCategoryByID/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteSubCategoryByIDAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _category.DeleteSubCategoryByIDAsync(id);
            return Ok();
        }

        // CRUD Category
        [HttpGet("CategoryByPage/{page}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CategoryByPageAsync(int page)
        {
            var category = await _category.CategoryByPageAsync(page);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpGet("GetCategoryById/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetCategoryByIdAsync(int id)
        {
            var category = await _category.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpGet("CategoryQuantity")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CategoryQuantityAsync()
        {
            var quantity = await _category.CategoryQuantityAsync();
            return Ok(quantity);
        }

        [HttpPost("CreateCategory")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateCategoryAsync(CreateCategoryDTO createCategoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _category.CreateCategoryAsync(createCategoryDTO);
            return Ok();
        }

        [HttpPost("EditCategory")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditCategoryAsync(EditCategoryDTO editCategoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _category.EditCategoryAsync(editCategoryDTO);
            return Ok();
        }

        [HttpDelete("DeleteCategoryByID/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteCategoryByIDAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _category.DeleteCategoryByIDAsync(id);
            return Ok();
        }
    }
}
