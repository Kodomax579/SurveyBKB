using System.Text.Json.Serialization;

namespace SchulFunk_Webprojekt.Feature.UserHandling.Model
{
    public class ChangePasswordDTO
    {
        [JsonPropertyName("currentPassword")]
        public string CurrentPassword { get; set; } = string.Empty;

        [JsonPropertyName("newPassword")]
        public string NewPassword { get; set; } = string.Empty;
    }
}
