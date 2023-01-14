﻿namespace Lister.WebApi.Models.Response
{
    public class CreateGroupResponse
    {
        public Guid GroupId { get; set; }
        public string Name { get; set; }
        public ICollection<Guid> Users { get; set; }
    }
}