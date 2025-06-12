using Core.DTOs.Address;

namespace Core.Interfaces
{
    public interface IAddressService
    {
        Task CreateAsync(AddressDTO addressDTO);
        Task<List<AddressDTO>> GetAllAddressAsync();
        Task<List<CountryDTO>> GetCountry();
        Task<List<CityDTO>> GetCity();
    }
}
