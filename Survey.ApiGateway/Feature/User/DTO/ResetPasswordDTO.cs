using System.Text.Json.Serialization;

namespace Survey.ApiGateway.Feature.User.DTO
{
    public class ResetPasswordDTO
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string NewPassword { get; set; } = string.Empty;
    }
}
