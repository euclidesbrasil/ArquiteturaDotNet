namespace ArquiteturaDesafio.Core.Domain.Common;

public class PaginationQuery
{
    public int Page { get; set; } = 1; // Página inicial
    public int Size { get; set; } = 10; // Tamanho de página
    public string Order { get; set; } = ""; // Ordenação
    public Dictionary<string, string> Filter { get; set; } = new Dictionary<string, string>();
    public int Skip => (Page - 1) * Size; // Cálculo de quantos registros devem ser pulados (offset)
}