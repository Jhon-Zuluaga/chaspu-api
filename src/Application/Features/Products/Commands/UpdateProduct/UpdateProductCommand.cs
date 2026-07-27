
using MediatR;

public class UpdateProductCommand : IRequest<ProductDto>
{
    public int Id { get; set;}
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal CostPrice { get; set; } 
    public decimal SalePrice { get; set; }
    public int MinStock { get; set; }
}