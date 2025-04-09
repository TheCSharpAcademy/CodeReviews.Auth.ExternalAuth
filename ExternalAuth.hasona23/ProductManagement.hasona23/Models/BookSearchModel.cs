using System.Collections;

namespace ProductManagement.hasona23.Models;

public class BookSearchModel
{
    public IEnumerable<BookModel> Books { get; set; }
    public string? BookName { get; set; }
    public int? MaxPrice { get; set; }
    public int? MinPrice { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? DateAdded { get; set; }
    public int? CurrentPage { get; set; }
    public int? TotalPages { get; set; }
    public bool HasNextPage => CurrentPage < TotalPages;
    public bool HasPreviousPage => CurrentPage > 1;

    public IEnumerable<BookModel> SearchBooks()
    {
        var books = Books;
        if(MaxPrice.HasValue)
            books = books.Where(x => x.Price <= MaxPrice.Value);
        if(MinPrice.HasValue)
            books = books.Where(x => x.Price >= MinPrice.Value);
        if(BookName != null)
            books = books.Where(x => x.Name.ToUpper().Contains(BookName.ToUpper()));
        if(IsActive.HasValue)
            books = books.Where(x => x.IsActive == IsActive.Value);
        if(DateAdded.HasValue)
            books = books.Where(x => x.DateAdded >= DateAdded.Value);
        return books;
    }
}