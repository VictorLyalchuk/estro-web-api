using AutoMapper;
using Core.Entities.Product;
using Core.Interfaces;

namespace Core.Services
{
    public class ImageForHomeService : IImageForHomeService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<ImageForHome> _imageForHomeRepository;
        public ImageForHomeService(IMapper mapper, IRepository<ImageForHome> imageForHomeRepository)
        {
            _imageForHomeRepository = imageForHomeRepository;
            _mapper = mapper;
        }
        public async Task<List<ImageForHome>>? GetAllImageAsync()
        {
            var images = await _imageForHomeRepository.GetAsync();
            return _mapper.Map<List<ImageForHome>>(images);
        }
    }
}
