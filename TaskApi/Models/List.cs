using Microsoft.AspNetCore.Mvc;

namespace TaskApi.Models
{
    public class List
    {
        public int list_id { get; set; }
        public int? user_id { get; set; }
        public string? list_name { get; set; }
        public string? list_description { get; set; }
        public DateTime? list_created { get; set; }

        public List() { }
        public List(int list_id, int? user_id, string? list_name, string? list_description, DateTime? list_created)
        {
            this.list_id = list_id;
            this.user_id = user_id;
            this.list_name = list_name;
            this.list_description = list_description;
            this.list_created = list_created;
        }
    }

    public class ListDTO
    {
        public int? user_id { get; set; }
        public string? list_name { get; set; }
        public string? list_description { get; set; }
        public ListDTO(int? user_id, string? list_name, string? list_description)
        {
            this.user_id = user_id;
            this.list_name = list_name;
            this.list_description = list_description;
        }
    }
}