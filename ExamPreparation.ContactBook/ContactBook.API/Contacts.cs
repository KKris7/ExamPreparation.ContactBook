using System.Text.Json.Serialization;

namespace ContactBook.API
{
    internal class Contacts
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("firstName")]
        public string firstName { get; set; }

        [JsonPropertyName("lastName")]
        public string lastName { get; set; }

        [JsonPropertyName("email")]
        public string email { get; set; }

        [JsonPropertyName("phone")]
        public string phone { get; set; }
    }
}