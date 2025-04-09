using System.Collections;

namespace ProductManagement.hasona23.Models;

public class PaginatedList<T>(List<T> items,int pageIndex,int pageSize=3)
{
    public int CurrentPage { get; set; } = pageIndex;
    public int PageSize { get; set; } = pageSize;
    public int TotalPages => (int)Math.Ceiling(items.Count / (double)pageSize);

    public List<T> GetItems()
    {
        return items.GetRange((CurrentPage-1) * PageSize, ValidRange()?pageSize:items.Count()-(CurrentPage-1) * PageSize);
    }

    private bool ValidRange()
    {
        return items.Count - ((CurrentPage-1) * PageSize) > PageSize;
    }
}