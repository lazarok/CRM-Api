using CRM.Domain.Common;

namespace CRM.Domain.Entities;

public class Product : BaseEntity
{ 
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int ProductStock { get; set; }
    public decimal Price { get; set; }

}