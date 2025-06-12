using AutoMapper;
using Core.DTOs.UserInfo;
using Core.Entities.UserEntity;
using Core.Entities.UserInfo;
using Core.Helpers;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Core.Services
{
    public class BagService : IBagService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Bag> _bagRepository;
        private readonly IRepository<BagItems> _bagItemsRepository;
        private readonly UserManager<User> _userManager;

        public BagService(IMapper mapper, IRepository<Bag> bagRepository, IRepository<BagItems> bagItemsRepository, UserManager<User> userManager)
        {
            _mapper = mapper;
            _bagRepository = bagRepository;
            _bagItemsRepository = bagItemsRepository;
            _userManager = userManager;
        }
        public async Task CreateAsync(BagDTO bagDTO)
        {
            try
            {
                var bag = await _bagRepository.GetItemBySpec(new BagSpecification.GetBagByUserEmail(bagDTO.UserEmail));
                if (bag == null)
                {
                    var user = await _userManager.FindByEmailAsync(bagDTO.UserEmail);
                    if (user != null)
                    {
                        Bag newBag = new Bag()
                        {
                            OrderDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                            UserEmail = bagDTO.UserEmail,
                            UserId = user.Id,
                        };
                        await _bagRepository.InsertAsync(newBag);
                        await _bagRepository.SaveAsync();

                        await _bagItemsRepository.InsertAsync(new BagItems
                        {
                            ProductId = bagDTO.ProductId,
                            BagId = newBag.Id,
                            Quantity = 1,
                            Size = bagDTO.Size,
                        });
                        user.BagId = newBag.Id;
                        var result = await _userManager.UpdateAsync(user);
                    }
                }
                else
                {
                    await _bagRepository.Update(bag);

                    bagDTO.Id = bag.Id;

                    var existBagItem = await _bagItemsRepository.GetItemBySpec(new BagItemsSpecification.GetBagItemByProps(bagDTO));
                    if (existBagItem == null)
                    {
                        await _bagItemsRepository.InsertAsync(new BagItems
                        {
                            ProductId = bagDTO.ProductId,
                            BagId = bagDTO.Id,
                            Quantity = 1,
                            Size = bagDTO.Size,
                        });
                    }
                    else
                    {
                        existBagItem.Quantity += 1;
                        await _bagItemsRepository.Update(existBagItem);
                    }
                };
                await _bagRepository.SaveAsync();
                await _bagItemsRepository.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new CustomHttpException($"Error create bag: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }
        public async Task DeleteBagByIdAsync(string email)
        {
            var bag = await _bagRepository.GetItemBySpec(new BagSpecification.GetBagByUserEmail(email));
            if (bag != null)
            {
                await _bagRepository.DeleteAsync(bag);
                await _bagRepository.SaveAsync();
            }
        }
        public async Task<BagUserDTO> GetBagByUserEmailAsync(string email)
        {
            var bag = await _bagRepository.GetItemBySpec(new BagSpecification.GetBagByUserEmail(email));
            return _mapper.Map<BagUserDTO>(bag);
        }
        public async Task<int> GetCountBagByEmailAsync(string email)
        {
            try
            {
                var bag = await _bagRepository.GetItemBySpec(new BagSpecification.GetBagByUserEmail(email));
                if (bag != null)
                {
                    return (int)(bag.BagItems?.Sum(bi => bi.Quantity))!;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw new CustomHttpException($"Error get count bag: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        // Bag Items
        public async Task DeleteItemByID(int id)
        {
            try
            {
                var bagItem = await _bagItemsRepository.GetByIDAsync(id);
                if (bagItem != null)
                {
                    var bag = await _bagRepository.GetItemBySpec(new BagSpecification.GetBagByItemsID(bagItem.BagId));

                    if (bag != null)
                    {
                        if (bag.BagItems?.Sum(bi => bi.Quantity) > bagItem.Quantity)
                        {
                            await _bagItemsRepository.DeleteAsync(bagItem);
                        }

                        else
                        {
                            await _bagRepository.DeleteAsync(bag);
                        }
                        var user = await _userManager.FindByEmailAsync(bag.UserEmail);
                        if (user != null)
                        {
                            user.BagId = null;
                            await _userManager.UpdateAsync(user);
                        }
                    }
                    await _bagItemsRepository.SaveAsync();
                    await _bagRepository.SaveAsync();
                }
            }
            catch (Exception ex)
            {
                throw new CustomHttpException($"Error delete bag: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }
        public async Task IncreaseAsync(int id)
        {
            var bagItem = await _bagItemsRepository.GetByIDAsync(id);
            if (bagItem != null)
            {
                if (bagItem.Quantity < 10)
                {
                    bagItem.Quantity += 1;
                    await _bagItemsRepository.Update(bagItem);
                    await _bagItemsRepository.SaveAsync();
                }
            }
        }
        public async Task DecreaseAsync(int id)
        {
            var bagItem = await _bagItemsRepository.GetByIDAsync(id);
            if (bagItem != null)
            {
                if (bagItem.Quantity > 1)
                {
                    bagItem.Quantity -= 1;
                    await _bagItemsRepository.Update(bagItem);
                    await _bagItemsRepository.SaveAsync();
                }
            }
        }
    }
}