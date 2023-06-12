namespace Lister.WebApi.Models.Response
{
    public class CreateGroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Guid> Users { get; set; }
    }
}
