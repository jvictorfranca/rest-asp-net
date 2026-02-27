using System.Text.Json.Serialization;

namespace RestASPNet.Data.DTO.V2
{
    public class PersonDTO
    {
        public long Id { get; set; }

        [JsonPropertyOrder(3)]
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        public string Adress { get; set; }

        public string Gender { get; set; }

        [JsonConverter(typeof(Utils.JsonSerializers.DateSerializer))]
        public DateTime? BirthDay { get; set; }

    }
}
