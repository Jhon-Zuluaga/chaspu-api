
public class EmployeePayrollDto
{
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string WorkedHours { get; set; } = "0h 0m";
    public decimal WeeklySalary { get; set; }
}