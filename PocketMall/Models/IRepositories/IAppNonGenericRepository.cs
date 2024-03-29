﻿using Microsoft.AspNetCore.Identity;

namespace PocketMall.Models.IRepositories
{
    public interface IAppNonGenericRepository
    {
        Task<IdentityResult> SignUpUser(SignUp model);
        Task<SignInResult> LogInUser(LogIn model);
        Task LogOutUser();
        Task<List<Product>> GetSimilarProductsAsync(string category, Guid productId);
        Task<List<Order>> GetAllOrdersAsync();
        Task<List<Order>> SortByOrderDate();
    }
}
