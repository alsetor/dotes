namespace Templates.Web.Models
{
    public class LoginModel
    {
        public LoginModel() { }

        public LoginModel(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public string Login { get; set; }
        public string Password { get; set; }
    }
}