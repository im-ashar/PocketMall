using System.Threading.Tasks;

namespace PocketMall.Models.IRepositories
{
    public interface IAccountsRepository
    {
        Task<bool> StoreUserToDb(UserModel model);
        Task<bool> AuthorizeUserFromDb(UserModel model);
    }
}
