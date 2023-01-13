namespace ListerSS.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        //public string ConnectionId { get; set; }
        //public DateTime ConnectedAt { get; set; }
        //public DateTime DisconnectedAt { get; set;}
        public ICollection<Group> Groups { get; set; }
    }
}
