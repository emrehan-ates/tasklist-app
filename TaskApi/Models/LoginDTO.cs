namespace TaskApi.Models
{
    public class LoginDTO
    {
        public string user_email { get; set; }
        public string user_password { get; set; }
        public LoginDTO(string user_email, string user_password)
        {
            this.user_email = user_email;
            this.user_password = user_password;
        }
    }
}