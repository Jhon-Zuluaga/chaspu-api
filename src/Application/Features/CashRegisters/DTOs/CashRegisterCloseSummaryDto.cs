
public class CashRegisterCloseSummaryDto
{
    public int CashRegisterId { get; set; }
    public DateOnly Date { get; set; }
    public decimal InitialAmount { get; set; }
    public decimal TotalCashSales { get; set; }
    public decimal TotalTransferSales { get; set; }
    public decimal ExpectedCashAmount { get; set; }
    public decimal ActualFinalAmount { get; set; }
    public decimal Difference { get; set; }
    public bool IsBalanced { get; set; }
}