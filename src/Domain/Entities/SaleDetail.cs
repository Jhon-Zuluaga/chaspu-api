
public class SaleDetail : BaseEntity
{
    public int SaleId { get; set;}
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    // Navigation properties
    public Sale Sale { get; set; } = null!;
    public Product Product { get; set; } = null!;

    public decimal Subtotal => Quantity * UnitPrice;
}