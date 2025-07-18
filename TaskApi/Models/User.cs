
using Microsoft.AspNetCore.Mvc;

namespace TaskApi.Models
{
    public class User
    {
        public int? user_id { get; set; }
        public string? user_name { get; set; }
        public string? user_surname { get; set; }
        public string? user_email { get; set; }
        public DateOnly? user_birthdate { get; set; }
        public string user_password { get; set; }

        public DateTime? user_created { get; set; }

        public User(int user_id, string user_name, string user_surname, string user_email, DateOnly user_birthdate, string user_password, DateTime user_creation)
        {
            this.user_id = user_id;
            this.user_name = user_name;
            this.user_surname = user_surname;
            this.user_email = user_email;
            this.user_birthdate = user_birthdate;
            this.user_password = user_password;
            this.user_created = user_creation;
        }

        public User() { }

    }

    public class UserDTO
    {
        public string? user_name { get; set; }
        public string? user_surname { get; set; }
        public string? user_email { get; set; }
        public DateOnly? user_birthdate { get; set; }
        public string? user_password { get; set; }

        public UserDTO(string? user_name, string? user_surname, string? user_email, DateOnly? user_birthdate, string? user_password)
        {
            this.user_name = user_name;
            this.user_surname = user_surname;
            this.user_email = user_email;
            this.user_birthdate = user_birthdate;
            this.user_password = user_password;
        }
    }

    public class CacheUserDTO
    {
        public int? user_id { get; set; }
        public string? user_name { get; set; }
        public string? user_surname { get; set; }
        public string? user_email { get; set; }
        public DateOnly? user_birthdate { get; set; }
        public Boolean isAdmin { get; set; }

        public CacheUserDTO(int? user_id, string? user_name, string? user_surname, string? user_email, DateOnly? user_birthdate, Boolean isAdmin = false)
        {
            this.user_name = user_name;
            this.user_surname = user_surname;
            this.user_email = user_email;
            this.user_birthdate = user_birthdate;
            this.isAdmin = isAdmin;
            this.user_id = user_id;
        }

    }
}