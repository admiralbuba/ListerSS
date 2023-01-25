namespace Lister.WebApi.Models.Request
{
    public class AddUsersRequest
    {
        public Guid Id { get; set; }
        public ICollection<Guid>? Users { get; set; } = null!;
    }
}
