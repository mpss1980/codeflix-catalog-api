using Codeflix.Catalog.Domain.SeedWork;

namespace Codeflix.Catalog.Domain.Params;

public class SearchOutput<T> where T : AggregateRoot
{
    public int CurrentPage { get; set; }
    public int PerPage { get; set; }
    public int Total { get; set; }
    public IReadOnlyList<T> Items{ get; set; }

    public SearchOutput(int currentPage, int perPage, int total, IReadOnlyList<T> items)
    {
        CurrentPage = currentPage;
        PerPage = perPage;
        Total = total;
        Items = items;
    }
}