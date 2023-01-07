namespace ListerSS.Models
{
    public class HubMessage
    {
        public Guid? Id { get; set; }
        public string? FromName { get; set; }
        public string? ToName { get; set; }
        public bool? IsGroup { get; set; }
        public string? Text { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
