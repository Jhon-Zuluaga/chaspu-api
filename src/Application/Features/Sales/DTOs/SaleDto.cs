
public class SaleDto
{
    public int Id { get; set;}
    public DateTime Date { get; set;}
    public decimal TotalAmount { get; set;}
    public string PaymentMethod { get; set;} = string.Empty;
    public string UserName { get; set;} = string.Empty;
    public decimal? AmountReceived { get; set;}
    public decimal? ChangeGiven { get; set;}
    public List<SaleDetailDto> Items { get; set;} = new();
}