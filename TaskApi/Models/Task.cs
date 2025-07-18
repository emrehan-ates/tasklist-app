using Microsoft.AspNetCore.Mvc;

namespace TaskApi.Models
{
    public class TaskType
    {
        public int task_id { get; set; }
        public int? list_id { get; set; }
        public string? task_name { get; set; }
        public string? task_description { get; set; }
        public DateTime? task_created { get; set; }
        public bool? task_done { get; set; } = false;
        public DateTime? deadline { get; set; }

        public TaskType() { }

        public TaskType(int task_id, int? list_id, string? task_name, string? task_description, bool? task_done, DateTime? task_created, DateTime? deadline)
        {
            this.task_id = task_id;
            this.list_id = list_id;
            this.task_name = task_name;
            this.task_description = task_description;
            this.task_created = task_created;
            this.task_done = task_done;
            
            this.deadline = deadline;
        }
    }

    public class TaskDTO
    {
        public int? list_id { get; set; }
        public string? task_name { get; set; }
        public string? task_description { get; set; }
        public bool? task_done { get; set; } = false;
        public DateTime? deadline { get; set; }

        public TaskDTO(int? list_id, string? task_name, string? task_description, bool? task_done, DateTime? deadline)
        {
            this.task_description = task_description;
            this.task_done = task_done;
            this.task_name = task_name;
            this.deadline = deadline;
            this.list_id = list_id;
        }
    }
}