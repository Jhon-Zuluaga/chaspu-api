
public class WeeklyClosingSummaryDto
{
    public DateOnly StartDate { get; set;}
    public DateOnly EndDate { get; set;}
    
    public decimal GrossSale { get; set;}
    public decimal TotalCostOfGoodsSol { get; set;}
    public decimal TotalPayroll { get; set;}
    public decimal NetProfit { get; set;}

    public List<EmployeePayrollDto> Employees { get; set; } = new ();

    public int LowStockProductsCount { get; set;}
}