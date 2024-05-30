using CRM.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM.Application.Extensions;

public static class PagedExtensions
{
    public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int page) where T : class
    {
        var pagedList = new PagedList<T>();
        pagedList.Page = page;            
        pagedList.TotalCount = await source.CountAsync();
        pagedList.TotalPages = (int)Math.Ceiling(pagedList.TotalCount / (double)pagedList.PageSize);

        var items = await source.Skip((page - 1) * pagedList.PageSize).Take(pagedList.PageSize).ToListAsync();
        pagedList.List.AddRange(items);
            
        return pagedList;
    }
    
    public static PagedList<T> ToPagedList<T>(this List<T> source, int page) where T : class
    {
        var pagedList = new PagedList<T>();
        pagedList.Page = page;            
        pagedList.TotalCount = source.Count();
        pagedList.TotalPages = (int)Math.Ceiling(pagedList.TotalCount / (double)pagedList.PageSize);

        var items = source.Skip((page - 1) * pagedList.PageSize).Take(pagedList.PageSize).ToList();
        pagedList.List.AddRange(items);
            
        return pagedList;
    }
}