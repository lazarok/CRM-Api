namespace CRM.Application.Features.Products.DTOs;

public record ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int ProductStock { get; set; }
    public decimal Price { get; set; }
}