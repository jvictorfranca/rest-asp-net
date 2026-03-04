namespace RestASPNet.Data.DTO.V1
{
    public class PersonDTO
    {
        public long Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Adress { get; set; }

        public string Gender { get; set; }

        public bool Enabled { get; set; }
    }
}
