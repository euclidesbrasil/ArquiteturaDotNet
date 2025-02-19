using ArquiteturaDesafio.Core.Domain.Common;
using System.Linq.Expressions;

namespace ArquiteturaDesafio.Core.Domain.Interfaces;

public interface IPaginatedResult<T>
{
    List<T> Data { get; set; }
    int TotalItems { get; set; } // Total de itens sem considerar a paginação
    int TotalPages { get; } // Total de itens sem considerar a paginação
    int CurrentPage { get; set; }
}
