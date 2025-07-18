using TaskApi.Models;
using TaskApi.Helpers;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace TaskApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _map;

        public TaskService(AppDbContext context, IMapper map)
        {
            _context = context;
            _map = map;
        }
        public async Task<IEnumerable<TaskType>> GetAll(int id, bool asc = true)
        {

            var alldata = _context.Tasks.AsQueryable();
            var tasks1 = alldata.Where(t => t.list_id == id);

            if (asc)
            {
                tasks1 = tasks1.OrderBy(t => t.deadline);
            }
            else
            {
                tasks1 = tasks1.OrderByDescending(t => t.deadline);
            }

            return await tasks1.Select(t => new TaskType(
                t.task_id,
                t.list_id,
                t.task_name,
                t.task_description,
                t.task_done,
                t.task_created,
                t.deadline
            )).ToListAsync();

        }

        public async Task<IEnumerable<TaskType>> GetByName(string name, int id)
        {
            var tasks1 = _context.Tasks.AsQueryable();

            var foundTasks = tasks1.Where(t => t.list_id == id && t.task_name.Contains(name, StringComparison.OrdinalIgnoreCase));



            return await foundTasks.Select(t => new TaskType(
                t.task_id,
                t.list_id,
                t.task_name,
                t.task_description,
                t.task_done,
                t.task_created,
                t.deadline
            )).ToListAsync();
        }

        public async Task<IEnumerable<TaskType>> FilterByDeadline(DateTime? low, DateTime? high, int id)
        {
            var tasks1 = _context.Tasks.AsQueryable();


            if (low == null && high == null)
            {
                throw new System.Exception("Low and High deadlines cannot both be null.");
            }
            else if (low == null)
            {
                var filtered = tasks1.Where(t => t.deadline <= high && t.list_id == id);

                return await filtered.Select(t => new TaskType(
                t.task_id,
                t.list_id,
                t.task_name,
                t.task_description,
                t.task_done,
                t.task_created,
                t.deadline
            )).ToListAsync();
            }
            else if (high == null)
            {
                var filtered = tasks1.Where(t => t.deadline >= low && t.list_id == id);

                return await filtered.Select(t => new TaskType(
                t.task_id,
                t.list_id,
                t.task_name,
                t.task_description,
                t.task_done,
                t.task_created,
                t.deadline
            )).ToListAsync();
            }
            else
            {
                var filtered = tasks1.Where(t => t.deadline >= low && t.deadline <= high && t.list_id == id);

                return await filtered.Select(t => new TaskType(
                t.task_id,
                t.list_id,
                t.task_name,
                t.task_description,
                t.task_done,
                t.task_created,
                t.deadline
            )).ToListAsync();
            }
        }

        public async Task<TaskType> AddTask(TaskDTO addedTask)
        {
            if (_context.Tasks.Any(x => x.task_name == addedTask.task_name))
            {
                throw new Exception("Not the same name, C'mon loser");
            }

            var temp = _map.Map<TaskType>(addedTask);

            if (temp.deadline.HasValue)
            {
                temp.deadline = DateTime.SpecifyKind(temp.deadline.Value, DateTimeKind.Utc);
            }
            _context.Tasks.Add(temp);
            await _context.SaveChangesAsync();

            return temp;
        }

        public async Task<TaskType> UpdateTask(TaskType updated)
        {
            if (_context.Tasks.Any(x => x.task_name == updated.task_name && x.task_id != updated.task_id))
            {
                throw new Exception("Not the same name, C'mon loser");
            }

            var temp = _context.Tasks.Find(updated.task_id);
            if (temp == null)
            {
                throw new Exception("Task not found.");
            }

            // mapper sanırım çalışıyo ama tekrar denemeye yemedi
            temp.task_name = updated.task_name;
            temp.task_description = updated.task_description;
            temp.task_done = updated.task_done;
            temp.deadline = updated.deadline.HasValue
                ? DateTime.SpecifyKind(updated.deadline.Value, DateTimeKind.Utc)
                : null;
            temp.task_created = updated.task_created.HasValue
                ? DateTime.SpecifyKind(updated.task_created.Value, DateTimeKind.Utc)
                : null;

            // bunu yapmadan olmuyor
            _context.Entry(temp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update task: {ex.Message}");
            }

            return temp;
        }

        public async Task<bool> DeleteTask(int task_id)
        {
            var taskToDelete = _context.Tasks.Find(task_id);
            if (taskToDelete == null)
            {
                throw new Exception("Silecek bişi yok");
                return false;
            }

            _context.Tasks.Remove(taskToDelete);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> SetDone(int task_id)
        {
            var taskDone = _context.Tasks.Find(task_id);
            if (taskDone == null)
            {
                throw new NullReferenceException("böyle task yokki");

            }
            taskDone.task_done = true;
            // _context.Attach(taskDone);
            // _context.Entry(taskDone).Property(x => x.task_done).IsModified = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to change status {ex.Message}");
            }
            return true;
        }
}
}