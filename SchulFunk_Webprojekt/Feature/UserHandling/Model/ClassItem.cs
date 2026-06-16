using System.Text.Json.Serialization;

namespace SchulFunk_Webprojekt.Feature.UserHandling.Model
{
    public class ClassModel
    {
        [JsonPropertyName("classname")]
        public string Classname { get; set; } = string.Empty;
    }
}