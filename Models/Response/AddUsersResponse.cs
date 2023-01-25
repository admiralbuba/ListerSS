namespace Lister.WebApi.Models.Response
{
    public class AddUsersResponse
    {
        public List<Guid>? Users { get; set; } = null!;
        public DateTime ModifiedAt { get; set; }
    }
}
