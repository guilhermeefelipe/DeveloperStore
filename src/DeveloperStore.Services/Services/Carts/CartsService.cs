using DeveloperStore.Domain.Dto.Users;
using DeveloperStore.Repositories.Repositories.Carts;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Services.Carts;

//public interface ICartsService
//{
//    //Task<FileServerDto?> GetAsync(Guid id);
//    //Task<IReadOnlyList<MutableList>> GetListToMutableListAsync();
//    //Task<IPagedList<FileServerDto>> GetPagedListAsync(string filter, int currentPage, int pageSize);
//    //Task<IPagedList<HistoryDto>> GetPagedListHistoryAsync(Guid id, string filter, int currentPage, int pageSize);
//    //Task<FileServerFtpDto?> GetFtpAsync(Guid id);
//    //Task<FileServerUpdateDto?> GetToUpdateAsync(Guid id);
//    Task<int> CreateAsync(UserCreationDto model);
//}

//public class CartsService : ICartsService
//{
//    private readonly ICartsRepository cartsRepository;

//    public CartsService(ICartsRepository cartsRepository)
//    {
//        this.cartsRepository = cartsRepository;
//    }

//    public async Task<int> CreateAsync(UserCreationDto model)
//    {
//        if (model == null)
//            throw new ArgumentNullException(nameof(model));

//        model.Password = CryptoHelper.Encrypt(model.Password);

//        var cartId = await cartsRepository.CreateAsync(model);

//        return cartId;
//    }
//}