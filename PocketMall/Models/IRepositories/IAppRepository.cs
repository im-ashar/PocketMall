using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace PocketMall.Models.IRepositories
{
    public interface IAppRepository<T> where T : class
    {
        Task<IdentityResult> SignUpUser(SignUp model);
        Task<SignInResult> LogInUser(LogIn model);
        Task LogOutUser();

        //Generic Repository Functions
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<int?> AddAsync(T entity);
        Task<int?> UpdateAsync(T entity);
        Task<int?> DeleteAsync(T entity);
        Task<T> GetIfExistsAsync(T entity);
    }
}
