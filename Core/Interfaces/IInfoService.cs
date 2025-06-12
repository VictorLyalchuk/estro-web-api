using Core.DTOs.Product;

namespace Core.Interfaces
{
    public interface IInfoService
    {
        Task<List<InfoDTO>>? GetInfoAsync();
        Task<List<ProductSeasonDTO>>? GetSeason();
        Task<List<ProductColorsDTO>>? GetColors();
        Task<List<ProductMaterialDTO>>? GetMaterials();
        Task<List<ProductSizeDTO>>? GetSizes();
    }
}