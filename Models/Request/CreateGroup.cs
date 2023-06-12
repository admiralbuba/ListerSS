namespace Lister.WebApi.Models.Request
{
    public class CreateGroup
    {
        public string Name { get; set; }
        public ICollection<Guid> Users { get; set; }
    }
}
