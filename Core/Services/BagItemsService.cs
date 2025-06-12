using AutoMapper;
using Core.DTOs.UserInfo;
using Core.Entities.UserInfo;
using Core.Interfaces;
using Core.Specification;

namespace Core.Services
{
    public class BagItemsService : IBagItemsService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<BagItems> _bagItemsRepository;
        public BagItemsService(IMapper mapper, IRepository<BagItems> bagItemsRepository)
        {
            _mapper = mapper;
            _bagItemsRepository = bagItemsRepository;
        }
        public async Task CreateAsync(BagItemsDTO bagDTO)
        {
            var bagItem = _mapper.Map<BagItems>(bagDTO);
            await _bagItemsRepository.InsertAsync(bagItem);
            await _bagItemsRepository.SaveAsync();
        }
        public async Task DeleteBagItemsByIdBagAsync(BagItems bagItems)
        {
            var bagItem = await _bagItemsRepository.GetItemBySpec(new BagItemsSpecification.GetBagItemByPropsItems(bagItems));
            if (bagItem != null)
            {
                await _bagItemsRepository.DeleteAsync(bagItem);
                await _bagItemsRepository.SaveAsync();
            }
        }
        public async Task DeleteItemByID(int id)
        {
            var bagItem = await _bagItemsRepository.GetByIDAsync(id);
            if (bagItem != null)
            {
                await _bagItemsRepository.DeleteAsync(bagItem);
                await _bagItemsRepository.SaveAsync();
            }
        }
        public async Task EditAsync(BagItems bagItems)
        {
            var bagItem = await _bagItemsRepository.GetItemBySpec(new BagItemsSpecification.GetBagItemByPropsItems(bagItems));
            if (bagItem != null)
            {
                bagItem.Quantity = bagItems.Quantity;
                await _bagItemsRepository.InsertAsync(bagItem);
                await _bagItemsRepository.SaveAsync();
            }
        }
        public async Task<List<BagItemsDTO>>? GetBagItemsByBagIdAsync(int BagId)
        {
            var bagItems = await _bagItemsRepository.GetListBySpec(new BagItemsSpecification.GetBagItemsByBagID(BagId));
            return _mapper.Map<List<BagItemsDTO>>(bagItems);
        }
        public async Task<List<BagItemsDTO>>? GetBagItemsByBagEmailAsync(string email)
        {
            var bagItems = await _bagItemsRepository.GetListBySpec(new BagItemsSpecification.GetBagItemsByBagEmail(email));
            return _mapper.Map<List<BagItemsDTO>>(bagItems);
        }
    }
}