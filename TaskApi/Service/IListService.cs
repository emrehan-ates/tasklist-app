using TaskApi.Models;

namespace TaskApi.Services
{
    public interface IListService
    {
        public Task<IEnumerable<List>> GetAll(int id, bool asc);
        public Task<IEnumerable<List>> GetByName(string name, int id);
        public Task<bool> AddList(ListDTO newlist);
        public Task<bool> DeleteList(int id);
    }
}