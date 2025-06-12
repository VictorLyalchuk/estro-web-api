using AutoMapper;
using Core.DTOs.Filter;
using Core.DTOs.Image;
using Core.DTOs.Product;
using Core.DTOs.Storage;
using Core.Entities.Product;
using Core.Interfaces;
using Core.Specification;

namespace Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<ProductEntity> _productRepository;
        private readonly IRepository<StorageEntity> _storageRepository;
        private readonly IRepository<ImageEntity> _imageRepository;
        private readonly IRepository<ProductSize> _productSizesRepository;
        private readonly IFilesService _filesService;
        private readonly IImageService _imageService;
        public ProductService(IMapper mapper, IRepository<ProductEntity> productRepository, IFilesService filesService, IImageService imageService, IRepository<StorageEntity> storageRepository, IRepository<ImageEntity> imageRepository, IRepository<ProductSize> productSizesRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _storageRepository = storageRepository;
            _imageRepository = imageRepository;
            _filesService = filesService;
            _imageService = imageService;
            _productSizesRepository = productSizesRepository;
        }
        public async Task<List<ProductDTO>?> GetProductByCategoryIdAsync(int id)
        {
            var products = await _productRepository.GetListBySpec(new ProductSpecification.GetProductByCategoryId(id));
            return _mapper.Map<List<ProductDTO>>(products);
        }
        public async Task<List<ProductDTO>>? GetProductByURLNameAsync(string urlName)
        {
            var products = await _productRepository.GetListBySpec(new ProductSpecification.GetProductByUrlName(urlName));
            return _mapper.Map<List<ProductDTO>>(products);
        }
        public async Task<List<ProductDTO>>? FilterProductsAsync(FilterDTO? filterDTO)
        {
            var products = await _productRepository.GetListBySpec(new ProductSpecification.FilterProducts(filterDTO));
            return _mapper.Map<List<ProductDTO>>(products);
        }
        public async Task<ProductDTO>? GetProductByIDAsync(int id)
        {
            var product = await _productRepository.GetItemBySpec(new ProductSpecification.GetProductById(id));
            return _mapper.Map<ProductDTO>(product);
        }
        public async Task<List<ProductDTO>> GetProductByPageAsync(int page, string text)
        {
            var products = await _productRepository.GetListBySpec(new ProductSpecification.ProductByPage(page, text));
            return _mapper.Map<List<ProductDTO>>(products);
        }
        public async Task<ProductDTOEdit> GetEditProductById(int id)
        {
            var product = await _productRepository.GetItemBySpec(new ProductSpecification.GetProductById(id));
            var allProductSizes = await _productSizesRepository.GetAsync();

            var availableSizes = product.Storages
                .Select(storage => storage.Size)
                .Distinct()
                .ToList();

            var sizesDTO = allProductSizes
                 .Where(size => availableSizes.Contains(size.Value))
                 .Select(size => new ProductSizeDTO
                 {
                     Id = size.Id,
                     Name_en = size.Name_en,
                     Name_es = size.Name_es,
                     Name_uk = size.Name_uk,
                     Name_fr = size.Name_fr,
                     Value = size.Value,
                     SortOrder = size.SortOrder,
                 })
                 .ToList();

            var productDTO = _mapper.Map<ProductDTOEdit>(product);
            productDTO.Sizes = sizesDTO;

            return productDTO;
        }
        public async Task<int> ProductQuantity(string text)
        {
            //var products = await _productRepository.GetAsync();
            var products = await _productRepository.GetListBySpec(new ProductSpecification.ProductQuantityByPage(text));
            return products.Count();
        }
        public async Task<int> ProductQuantityByFiltersAsync(FilterDTO filterDTO)
        {
            var products = await _productRepository.GetListBySpec(new ProductSpecification.ProductQuantityByFiltersAsync(filterDTO));
            return products.Count();
        }
        public async Task CreateAsync(CreateProductDTO createProductDTO)
        {
            var product = _mapper.Map<ProductEntity>(createProductDTO);
            await _productRepository.InsertAsync(product);
            await _productRepository.SaveAsync();

            if (createProductDTO.ImagesFile != null)
            {
                foreach (var imageFile in createProductDTO.ImagesFile)
                {
                    var imageResult = await _imageRepository.GetByIDAsync(imageFile.Id);
                    var image = _mapper.Map<ImageDTO>(imageResult);
                    image.ProductId = product.Id;
                    image.ImagePath = imageFile.ImagePath;
                    await _imageService.EditAsync(image);
                };
            }
            var res = _productRepository.GetItemBySpec(new ProductSpecification.GetProductById(product.Id)).Result;
            foreach (var size in createProductDTO?.Sizes)
            {
                StorageEntity storage = new StorageEntity()
                {
                    ProductId = res.Id,
                    Size = size.Name_en,
                    ProductQuantity = 0,
                    SortOrder = size.SortOrder,
                };
                await _storageRepository.InsertAsync(storage);
                await _storageRepository.SaveAsync();
            }
        }
        public async Task EditAsync(EditProductDTO editProductDTO)
        {
            var product = _mapper.Map<ProductEntity>(editProductDTO);
            await _productRepository.UpdateAsync(product);
            await _productRepository.SaveAsync();

            if (editProductDTO.ImagesFile != null)
            {
                foreach (var imageFile in editProductDTO.ImagesFile)
                {
                    var imageResult = await _imageRepository.GetByIDAsync(imageFile.Id);
                    var image = _mapper.Map<ImageDTO>(imageResult);
                    image.ProductId = product.Id;
                    image.ImagePath = imageFile.ImagePath;
                    await _imageService.EditAsync(image);
                };
            }

            // Отримання існуючих розмірів продукту з бази даних
            var existingStorages = await _storageRepository.GetListBySpec(new StorageSpecification.GetStorageByProductId(product.Id));
            var existingSizes = existingStorages.ToDictionary(s => s.Size);

            // Додавання нових розмірів та оновлення існуючих
            var sizesToKeep = new HashSet<string>(editProductDTO.Sizes?.Select(s => s.Name_en) ?? new List<string>());
            var sizesToRemove = new List<StorageEntity>();

            foreach (var size in editProductDTO?.Sizes ?? new List<ProductSizeDTO>())
            {
                if (existingSizes.TryGetValue(size.Name_en, out var existingStorage))
                {
                    // Оновлюємо існуючий розмір
                    existingStorage.SortOrder = size.SortOrder;
                    await _storageRepository.UpdateAsync(existingStorage);
                    existingSizes.Remove(size.Name_en); // Видаляємо зі списку існуючих розмірів
                }
                else
                {
                    // Додаємо новий розмір
                    var newStorage = new StorageEntity()
                    {
                        ProductId = product.Id,
                        Size = size.Name_en,
                        ProductQuantity = 0,
                        SortOrder = size.SortOrder,
                    };
                    await _storageRepository.InsertAsync(newStorage);
                }
            }

            // Визначаємо розміри, які треба видалити
            sizesToRemove.AddRange(existingSizes.Values);

            // Видалення старих розмірів
            foreach (var storage in sizesToRemove)
            {
                await _storageRepository.DeleteAsync(storage);
            }

            await _storageRepository.SaveAsync();

        }
        public async Task DeleteProductByIDAsync(int id)
        {
            var imageClass = await _imageService.GetImageByIDProductAsync(id)!;
            if (imageClass != null)
            {
                foreach (var image in imageClass)
                {
                    await _filesService.DeleteProductImage(image.ImagePath!);
                }
                foreach (var image in imageClass)
                {
                    await _imageRepository.DeleteAsync(image.Id);
                    await _imageRepository.SaveAsync();
                }
            }
            var storages = await _storageRepository.GetListBySpec(new StorageSpecification.GetStorageByProductId(id));
            var res = _mapper.Map<List<StorageDTO>>(storages);
            if (res != null)
            {
                foreach (var storage in res)
                {
                    await _storageRepository.DeleteAsync(storage.Id);
                    await _storageRepository.SaveAsync();
                }
            }
            var productToDelete = await _productRepository.GetByIDAsync(id);
            if (productToDelete != null)
            {
                await _productRepository.DeleteAsync(productToDelete);
                await _productRepository.SaveAsync();
            }
        }
    }
}