namespace SchulFunk_Webprojekt.Feature.UserHandling.Model
{
    public class LoginModel
    {
        public UserModel User { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
