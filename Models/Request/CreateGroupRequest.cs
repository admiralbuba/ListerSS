namespace Lister.WebApi.Models.Request
{
    public class CreateGroupRequest
    {
        public string Name { get; set; }
        public ICollection<Guid> Users { get; set; }
    }
}
