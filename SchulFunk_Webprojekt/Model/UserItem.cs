using SchulFunk_Webprojekt.Model;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

public class UserModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("lastname")]
    public string Lastname { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("group")]
    public Groups Group { get; set; }

    [JsonPropertyName("class")]
    public ClassModel? Class { get; set; }
}