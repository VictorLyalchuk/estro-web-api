using Core.DTOs.Category;

namespace Core.Interfaces
{
    public interface ICategoryService
    {
        Task<List<MainCategoryDTO>> MainGetAllAsync();
        Task<List<SubCategoryDTO>> SubGetAllAsync();
        Task<List<CategoryDTO>> CategoryGetAllAsync();
        Task<List<CategoryDTO>> CategoryGetWithSubAsync(string subName);

        // Main Category CRUD
        Task <List<MainCategoryDTO>>MainCategoryByPageAsync(int page);
        Task<MainCategoryDTO> GetMainCategoryByIdAsync(int id);
        Task<int> MainCategoryQuantityAsync();
        Task CreateMainCategoryAsync(CreateMainCategoryDTO createMainCategoryDTO);
        Task EditMainCategoryAsync(EditMainCategoryDTO editMainCategoryDTO);
        Task DeleteMainCategoryByIDAsync(int id);

        // Sub Category CRUD
        Task<List<SubCategoryDTO>> SubCategoryByPageAsync(int page);
        Task<SubCategoryDTO> GetSubCategoryByIdAsync(int id);
        Task <int> SubCategoryQuantityAsync();
        Task CreateSubCategoryAsync(CreateSubCategoryDTO createSubCategoryDTO);
        Task EditSubCategoryAsync(EditSubCategoryDTO editSubCategoryDTO);
        Task DeleteSubCategoryByIDAsync(int id);

        // Category CRUD
        Task<List<CategoryDTO>> CategoryByPageAsync(int page);
        Task <CategoryDTO> GetCategoryByIdAsync(int id);
        Task <int> CategoryQuantityAsync();
        Task CreateCategoryAsync(CreateCategoryDTO createCategoryDTO);
        Task EditCategoryAsync(EditCategoryDTO editCategoryDTO);
        Task DeleteCategoryByIDAsync(int id);
    }
}
