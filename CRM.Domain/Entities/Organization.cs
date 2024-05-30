using CRM.Domain.Common;

namespace CRM.Domain.Entities;

public class Organization : BaseEntity
{
    public required string Name { get; set; }
    public required string SlugTenant { get; set; }

    public IList<User> Users { get; set; } = new List<User>();
}