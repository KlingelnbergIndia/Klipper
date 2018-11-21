using Models.Core.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KlipperApi.Controllers.Auth
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();

        Task<User> Get(int id);

        Task<bool> Exists(int id);

        Task<bool> Add(User item);

        Task<bool> Remove(int id);

        Task<bool> RemoveAll();

        Task<bool> Update(int id, User item);

        bool ValidateCredentials(string userName, string passwordHash);

        Task<User> GetByUserName(string userName);

    }
}
