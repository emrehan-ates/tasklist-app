using TaskApi.Models;

namespace TaskApi.Services
{
    public interface ITaskService
    {
        public Task<IEnumerable<TaskType>> GetAll(int id, bool asc);
        public Task<IEnumerable<TaskType>> GetByName(string name, int id);
        public Task<IEnumerable<TaskType>> FilterByDeadline(DateTime? low, DateTime? high, int id);
        public Task<TaskType> AddTask(TaskDTO added);
        public Task<TaskType> UpdateTask(TaskType updated);
        public Task<bool> DeleteTask(int task_id);
        public Task<bool> SetDone(int task_id);
    }
}