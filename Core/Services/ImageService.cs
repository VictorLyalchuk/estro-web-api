using AutoMapper;
using Core.DTOs.Image;
using Core.Entities.Product;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Http;

namespace Core.Services
{
    public class ImageService : IImageService
    {
        private readonly IRepository<ImageEntity> _imageRepository;
        private readonly IFilesService _filesService;
        private readonly IMapper _mapper;
        public ImageService(IRepository<ImageEntity> imageRepository, IFilesService filesService, IMapper mapper)
        {
            _imageRepository = imageRepository;
            _filesService = filesService;
            _mapper = mapper;
        }
        public async Task<ImageEntity> CreateImageAsync(IFormFile ImageFile)
        {
            ImageEntity image = new ImageEntity()
            {
                ImagePath = await _filesService.SaveProductImage(ImageFile)
            };
            await _imageRepository.InsertAsync(image);
            await _imageRepository.SaveAsync();
            return image;
        }
        public async Task<List<ImageDTO>>? GetImageByIDProductAsync(int id)
        {
            var image = await _imageRepository.GetListBySpec(new ImageSpecification.ImageByProductID(id));
            return _mapper.Map<List<ImageDTO>>(image);
        }
        public async Task<ImageDTO> GetByIdAsync(int id)
        {
            var image = await _imageRepository.GetByIDAsync(id);
            return _mapper.Map<ImageDTO>(image);
        }
        public async Task EditAsync(ImageDTO imageDTO)
        {
            var image = _mapper.Map<ImageEntity>(imageDTO);
            await _imageRepository.UpdateAsync(image);
            await _imageRepository.SaveAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var image = await _imageRepository.GetByIDAsync(id);
            if (image != null)
            {
                await _imageRepository.DeleteAsync(image);
                await _imageRepository.SaveAsync();
            }
        }
        public async Task CreateAsync(ImageDTO imageDTO)
        {
            var image = _mapper.Map<ImageEntity>(imageDTO);
            await _imageRepository.InsertAsync(image);
            await _imageRepository.SaveAsync();
        }
        public async Task DeleteImageAsync(ImageDTO imageDTO)
        {
            await _filesService.DeleteProductImage(imageDTO.ImagePath!);
            await DeleteAsync(imageDTO.Id);
        }

        public async Task<string> CreateUserImageAsync(IFormFile ImageFile)
        {
            return await _filesService.SaveUserImageAsync(ImageFile);
        }
        public async Task DeleteUserImageAsync(string image)
        {
            await _filesService.DeleteUserImageAsync(image);

        }
        public async Task<string> SaveImageFromUrlAsync(string url)
        {
            return await _filesService.SaveImageFromUrlAsync(url);
        }

        public async Task<string> CreateCategoryImageAsync(IFormFile ImageFile)
        {
            return await _filesService.SaveUserImageAsync(ImageFile);
        }

        public async Task DeleteCategoryImageAsync(string ImagePath)
        {
            await _filesService.DeleteUserImageAsync(ImagePath!);
        }
    }
}
