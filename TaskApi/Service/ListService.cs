using TaskApi.Models;
using TaskApi.Helpers;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace TaskApi.Services
{
    public class ListService : IListService
    {

        private readonly AppDbContext _context;
        private readonly IMapper _map;

        public ListService(AppDbContext context, IMapper map)
        {
            _context = context;
            _map = map;
        }

        public async Task<IEnumerable<List>> GetAll(int id, bool asc = true)
        {
            var lists1 = _context.Lists.AsQueryable();



            var userLists = lists1.Where(l => l.user_id == id);

            if (asc)
            {
                userLists.Order();
            }
            else
            {
                userLists.OrderDescending();
            }

            return await userLists.Select(l => new List(
                l.list_id,
                l.user_id,
                l.list_name,
                l.list_description,
                l.list_created
            )).ToListAsync();

        }

        public async Task<IEnumerable<List>> GetByName(string name, int id)
        {
            var lists1 = _context.Lists.AsQueryable();
            var filtered = lists1.Where(l => l.user_id == id && l.list_name.Contains(name, StringComparison.OrdinalIgnoreCase));

            return await filtered.Select(l => new List(
                l.list_id,
                l.user_id,
                l.list_name,
                l.list_description,
                l.list_created
            )).ToListAsync();
        }

        //TODO:check top 2 later 

        public async Task<bool> AddList(ListDTO newlist)
        {
            var temp = _map.Map<List>(newlist);

            _context.Lists.Add(temp);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteList(int id)
        {
            var temp = _context.Lists.Find(id);
            if (temp == null)
            {
                throw new Exception("Silinecek liste yokki");
            }
            _context.Lists.Remove(temp);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}