namespace RestASPNet.Data.DTO.V1
{
    public class EmailRequestDTO
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string body { get; set; } = string.Empty;
    }
}