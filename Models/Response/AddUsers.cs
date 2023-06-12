namespace Lister.WebApi.Models.Response
{
    public class AddUsers
    {
        public List<Guid>? Users { get; set; } = null!;
        public DateTime ModifiedAt { get; set; }
    }
}
