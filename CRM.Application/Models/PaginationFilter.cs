namespace CRM.Application.Models;

public class PaginationFilter : BaseFilter
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}