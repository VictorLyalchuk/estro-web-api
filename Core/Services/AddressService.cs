using AutoMapper;
using Core.DTOs.Address;
using Core.Entities.Address;
using Core.Interfaces;
using Core.Specification;

namespace Core.Services
{
    public class AddressService : IAddressService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<AddressEntity> _addressRepository;
        private readonly IRepository<CityEntity> _cityRepository;
        private readonly IRepository<CountryEntity> _countryRepository;
        public AddressService(IMapper mapper, IRepository<AddressEntity> addressRepository, IRepository<CityEntity> cityRepository, IRepository<CountryEntity> countryRepository)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
        }
        public async Task CreateAsync(AddressDTO addressDTO)
        {
            var address = _mapper.Map<AddressEntity>(addressDTO);
            await _addressRepository.InsertAsync(address);
            await _addressRepository.SaveAsync();
        }

        public async Task<List<AddressDTO>> GetAllAddressAsync()
        {
            var address = await _addressRepository.GetAsync();
            return _mapper.Map<List<AddressDTO>>(address);
        }
        public async Task<List<CountryDTO>> GetCountry()
        {
            var country = await _countryRepository.GetListBySpec(new AddressSpecification.GettAllCountry());
            return _mapper.Map<List<CountryDTO>>(country);

        }
        public async Task<List<CityDTO>> GetCity()
        {
            var city = await _cityRepository.GetListBySpec(new AddressSpecification.GettAllCity());
            return _mapper.Map<List<CityDTO>>(city);
        }

    }
}
