
using MediatR;

public class SearchProductsQuery : IRequest<List<ProductDto>>
{
    public string SearchTerm { get; set; } = string.Empty;
    public SearchProductsQuery(string searchTerm) => SearchTerm = searchTerm;
}