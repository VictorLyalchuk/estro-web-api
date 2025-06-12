using AutoMapper;
using Core.DTOs.Category;
using Core.Entities.Category;
using Core.Interfaces;
using Core.Specification;

namespace Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<MainCategory> _mainCategoryRepository;
        private readonly IRepository<SubCategory> _subCategoryRepository;
        private readonly IRepository<CategoryEntity> _categoryRepository;
        private readonly IFilesService _filesService;
        public CategoryService(IRepository<MainCategory> mainCategoryRepository, IRepository<CategoryEntity> categoryRepository, IMapper mapper, IRepository<SubCategory> subCategoryRepository, IFilesService filesService)
        {
            _mainCategoryRepository = mainCategoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _filesService = filesService;
        }

        public async Task<List<MainCategoryDTO>> MainGetAllAsync()
        {
            var categories = await _mainCategoryRepository.GetListBySpec(new MainCategorySpecification.All());
            return _mapper.Map<List<MainCategoryDTO>>(categories);
        }
        public async Task<List<SubCategoryDTO>> SubGetAllAsync()
        {
            var categories = await _subCategoryRepository.GetListBySpec(new SubCategorySpecification.All());
            return _mapper.Map<List<SubCategoryDTO>>(categories);
        }
        public async Task<List<CategoryDTO>> CategoryGetAllAsync()
        {
            var categories = await _categoryRepository.GetListBySpec(new CategorySpecification.GetAllAsync());
            return _mapper.Map<List<CategoryDTO>>(categories);
        }
        public async Task<List<CategoryDTO>> CategoryGetWithSubAsync(string subName)
        {
            var categories = await _categoryRepository.GetListBySpec(new CategorySpecification.GetBySubNameAsync(subName));
            return _mapper.Map<List<CategoryDTO>>(categories);
        }

        // CRUD Main Category
        public async Task<List<MainCategoryDTO>> MainCategoryByPageAsync(int page)
        {
            var category = await _mainCategoryRepository.GetListBySpec(new CategorySpecification.MainCategoryByPageAsync(page));
            if (category != null)
            {
                return _mapper.Map<List<MainCategoryDTO>>(category);
            }
            else
            {
                return null;
            }
        }
        public async Task<MainCategoryDTO> GetMainCategoryByIdAsync(int id)
        {
            var category = await _mainCategoryRepository.GetByIDAsync(id);
            if (category != null)
            {
                return _mapper.Map<MainCategoryDTO>(category);
            }
            else
            {
                return null;
            }
        }
        public async Task<int> MainCategoryQuantityAsync()
        {
            var category = await _mainCategoryRepository.GetAsync();
            return category.Count();
        }
        public async Task CreateMainCategoryAsync(CreateMainCategoryDTO createMainCategoryDTO)
        {
            var category = _mapper.Map<MainCategory>(createMainCategoryDTO);
            await _mainCategoryRepository.InsertAsync(category);
            await _mainCategoryRepository.SaveAsync();
        }
        public async Task EditMainCategoryAsync(EditMainCategoryDTO editMainCategoryDTO)
        {
            var category = _mapper.Map<MainCategory>(editMainCategoryDTO);
            await _mainCategoryRepository.UpdateAsync(category);
            await _mainCategoryRepository.SaveAsync();
        }
        public async Task DeleteMainCategoryByIDAsync(int id)
        {
            var category = await _mainCategoryRepository.GetByIDAsync(id);
            if (category != null)
            {
                if (category.ImagePath != null)
                {
                    await _filesService.DeleteUserImageAsync(category.ImagePath!);
                }
                await _mainCategoryRepository.DeleteAsync(category);
                await _mainCategoryRepository.SaveAsync();
            }
        }

        // CRUD Sub Category
        public async Task<List<SubCategoryDTO>> SubCategoryByPageAsync(int page)
        {
            var category = await _subCategoryRepository.GetListBySpec(new CategorySpecification.SubCategoryByPageAsync(page));
            if (category != null)
            {
                return _mapper.Map<List<SubCategoryDTO>>(category);
            }
            else
            {
                return null;
            }
        }
        public async Task<SubCategoryDTO> GetSubCategoryByIdAsync(int id)
        {
            var category = await _subCategoryRepository.GetByIDAsync(id);
            if (category != null)
            {
                return _mapper.Map<SubCategoryDTO>(category);
            }
            else
            {
                return null;
            }
        }
        public async Task<int> SubCategoryQuantityAsync()
        {
            var category = await _subCategoryRepository.GetAsync();
            return category.Count();
        }
        public async Task CreateSubCategoryAsync(CreateSubCategoryDTO createSubCategoryDTO)
        {
            var category = _mapper.Map<SubCategory>(createSubCategoryDTO);
            await _subCategoryRepository.InsertAsync(category);
            await _subCategoryRepository.SaveAsync();
        }
        public async Task EditSubCategoryAsync(EditSubCategoryDTO editSubCategoryDTO)
        {
            var category = _mapper.Map<SubCategory>(editSubCategoryDTO);
            await _subCategoryRepository.UpdateAsync(category);
            await _subCategoryRepository.SaveAsync();
        }
        public async Task DeleteSubCategoryByIDAsync(int id)
        {
            var category = await _subCategoryRepository.GetByIDAsync(id);
            if (category != null)
            {
                if (category.ImagePath != null)
                {
                    await _filesService.DeleteUserImageAsync(category.ImagePath!);
                }
                await _subCategoryRepository.DeleteAsync(category);
                await _subCategoryRepository.SaveAsync();
            }
        }
        
        // CRUD Category
        public async Task<List<CategoryDTO>> CategoryByPageAsync(int page)
        {
            var category = await _categoryRepository.GetListBySpec(new CategorySpecification.CategoryByPageAsync(page));
            if (category != null)
            {
                return _mapper.Map<List<CategoryDTO>>(category);
            }
            else
            {
                return null;
            }
        }
        public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIDAsync(id);
            if (category != null)
            {
                return _mapper.Map<CategoryDTO>(category);
            }
            else
            {
                return null;
            }
        }
        public async Task<int> CategoryQuantityAsync()
        {
            var category = await _categoryRepository.GetAsync();
            return category.Count();
        }
        public async Task CreateCategoryAsync(CreateCategoryDTO createCategoryDTO)
        {
            var category = _mapper.Map<CategoryEntity>(createCategoryDTO);
            await _categoryRepository.InsertAsync(category);
            await _categoryRepository.SaveAsync();
        }
        public async Task EditCategoryAsync(EditCategoryDTO editCategoryDTO)
        {
            var category = _mapper.Map<CategoryEntity>(editCategoryDTO);
            await _categoryRepository.UpdateAsync(category);
            await _categoryRepository.SaveAsync();
        }
        public async Task DeleteCategoryByIDAsync(int id)
        {
            var category = await _categoryRepository.GetByIDAsync(id);
            if (category != null)
            {
                if (category.ImagePath != null)
                {
                    await _filesService.DeleteUserImageAsync(category.ImagePath!);
                }
                await _categoryRepository.DeleteAsync(category);
                await _categoryRepository.SaveAsync();
            }
        }
    }
}
