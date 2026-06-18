namespace Survey.ApiGateway.Feature.User.DTO
{
    public class ResetPasswordDTO
    {
        public string Email { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
