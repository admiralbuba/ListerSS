namespace ListerSS.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ConnectionId { get; set; }
        public DateTime LogoutAt { get; set; }
        public DateTime CreatedAt { get; set;}
    }
}
