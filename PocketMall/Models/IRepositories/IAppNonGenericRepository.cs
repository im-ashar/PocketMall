using Microsoft.AspNetCore.Identity;

namespace PocketMall.Models.IRepositories
{
    public interface IAppNonGenericRepository
    {
        Task<IdentityResult> SignUpUser(SignUp model);
        Task<SignInResult> LogInUser(LogIn model);
        Task LogOutUser();
    }
}
