using System.Collections.Generic;
using System.Threading.Tasks;

namespace NNNN.DataAccess.Interfaces
{
    public interface IXXXXRepository
    {
        Task<IEnumerable<XXXX>> GetAllXXXXs();

        Task<XXXX> Get(int id);

        Task<bool> Exists(int id);

        Task<bool> Add(XXXX item);

        Task<bool> Remove(int id);

        Task<bool> RemoveAll();

        Task<bool> Update(int id, XXXX item);
    }
}
