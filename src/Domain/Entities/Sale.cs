
public class Sale : BaseEntity
{
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public int UserId { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
}