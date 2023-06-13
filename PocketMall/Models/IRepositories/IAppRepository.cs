using System.Threading.Tasks;

namespace PocketMall.Models.IRepositories
{
    public interface IAppRepository<T> where T : class
    {
        Task<bool> AuthorizeUserFromDb(User model);

        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<int?> AddAsync(T entity);
        Task<int?> UpdateAsync(T entity);
        Task<int?> DeleteAsync(T entity);
        Task<T> GetIfExistsAsync(T entity);
    }
}
