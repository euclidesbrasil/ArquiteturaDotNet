namespace ArquiteturaDesafio.Core.Domain.Common;

public class PaginatedResult<T>
{
    public List<T> Data { get; set; }
    public int TotalItems { get; set; } // Total de itens sem considerar a paginação
    public int TotalPages => TotalItems == 0 || Data.Count == 0 ? 0 : (int)Math.Ceiling(TotalItems / (double)Data.Count);
    public int CurrentPage { get; set; }
}