using AutoMapper;
using Core.DTOs.Store;
using Core.Entities.Store;
using Core.Interfaces;
using Core.Specification;

namespace Core.Services
{
    public class StoreService : IStoreService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<StoreEntity> _storeRepository;
        public StoreService(IMapper mapper, IRepository<StoreEntity> storeRepository)
        {
            _mapper = mapper;
            _storeRepository = storeRepository;
        }
        public async Task<List<StoreDTO>?> GetAllAsync()
        {
            var stores = await _storeRepository.GetListBySpec(new StoreSpecification.StoreAll());
            return _mapper.Map<List<StoreDTO>>(stores);
        }

        public async Task<List<StoreDTO>> StoreByPageAsync(int page)
        {
            var stores = await _storeRepository.GetListBySpec(new StoreSpecification.StoreByPage(page));
            return _mapper.Map<List<StoreDTO>>(stores);
        }
        public async Task<StoreDTO>? GetStoreByIDAsync(int id)
        {
            var store = await _storeRepository.GetByIDAsync(id);
            return _mapper.Map<StoreDTO>(store);
        }
        public async Task<int> StoreQuantityAsync()
        {
            var store = await _storeRepository.GetAsync();
            return store.Count();
        }
        public async Task CreateAsync(CreateStoreDTO createStoreDTO)
        {
            var store = _mapper.Map<StoreEntity>(createStoreDTO);
            await _storeRepository.InsertAsync(store);
            await _storeRepository.SaveAsync();
        }
        public async Task EditAsync(EditStoreDTO editStoreDTO)
        {
            var store = _mapper.Map<StoreEntity>(editStoreDTO);
            await _storeRepository.UpdateAsync(store);
            await _storeRepository.SaveAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var store = await _storeRepository.GetByIDAsync(id);
            if (store != null)
            {
                await _storeRepository.DeleteAsync(store);
                await _storeRepository.SaveAsync();
            }
        }
    }
}
