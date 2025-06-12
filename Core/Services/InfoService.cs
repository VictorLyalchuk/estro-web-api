using AutoMapper;
using Core.DTOs.Product;
using Core.Entities.Product;
using Core.Interfaces;
using Core.Specification;

namespace Core.Services
{
    public class InfoService : IInfoService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Info> _infoRepository;
        private readonly IRepository<ProductSeason> _productSeasonRepository;
        private readonly IRepository<ProductColors> _productColorsRepository;
        private readonly IRepository<ProductMaterial> _productMaterialRepository;
        private readonly IRepository<ProductSize> _productSizesRepository;
        public InfoService(IMapper mapper, IRepository<Info> infoRepository, IRepository<ProductSeason> productSeasonRepository, IRepository<ProductColors> productColorsRepository, IRepository<ProductMaterial> productMaterialsRepository, IRepository<ProductSize> productSizesRepository)
        {
            _mapper = mapper;
            _infoRepository = infoRepository;
            _productSeasonRepository = productSeasonRepository;
            _productColorsRepository = productColorsRepository;
            _productMaterialRepository = productMaterialsRepository;
            _productSizesRepository = productSizesRepository;
        }
        public async Task<List<InfoDTO>>? GetInfoAsync()
        {
            var infos = await _infoRepository.GetListBySpec(new InfoSpecification.GetAll());
            return _mapper.Map<List<InfoDTO>>(infos);
        }

        public async Task<List<ProductSeasonDTO>>? GetSeason()
        {
            var seasons = await _productSeasonRepository.GetListBySpec(new InfoSpecification.GetSeason());
            return _mapper.Map<List<ProductSeasonDTO>>(seasons);
        }

        public async Task<List<ProductColorsDTO>>? GetColors()
        {
            var colors = await _productColorsRepository.GetListBySpec(new InfoSpecification.GetColors());
            return _mapper.Map<List<ProductColorsDTO>>(colors);
        }
        public async Task<List<ProductMaterialDTO>>? GetMaterials()
        {
            var materials = await _productMaterialRepository.GetListBySpec(new InfoSpecification.GetMaterials());
            return _mapper.Map<List<ProductMaterialDTO>>(materials);
        }

        public async Task<List<ProductSizeDTO>>? GetSizes()
        {
            var sizes = await _productSizesRepository.GetListBySpec(new InfoSpecification.GetSizes());
            return _mapper.Map<List<ProductSizeDTO>>(sizes);
        }
    }
}
