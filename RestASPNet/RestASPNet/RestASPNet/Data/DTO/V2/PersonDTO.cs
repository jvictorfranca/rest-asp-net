using System.Net.Cache;
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
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LastName { get; set; }

        public string Adress { get; set; }

        [JsonConverter(typeof(Utils.JsonSerializers.GenderSerializer))]
        public string Gender { get; set; }

        [JsonConverter(typeof(Utils.JsonSerializers.DateSerializer))]
        [JsonIgnore]
        public DateTime? BirthDay { get; set; }

        [JsonIgnore(Condition =JsonIgnoreCondition.WhenWritingDefault)]
        public int Age { get; set; }

        [JsonIgnore]
        public bool IsAdult => Age >= 18;

    }
}
