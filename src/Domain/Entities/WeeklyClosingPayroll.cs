
public class WeeklyClosingPayroll : BaseEntity
{
    public int WeeklyClosingId { get; set;}
    public int UserId { get; set;}
    public string EmployeeName { get; set;} = string.Empty;
    public decimal WeeklySalaryPaid { get; set;}
    public TimeSpan WorkedHours { get; set;}

    public WeeklyClosing WeeklyClosing { get; set;} = null!;
    public User User { get; set;} = null!;
}