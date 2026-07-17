
public class AttendanceDto
{
    public int Id { get; set;}
    public int UserId { get; set;}
    public  string UserName { get; set;} = string.Empty;
    public DateOnly Date { get; set; } 
    public TimeOnly? CheckIn { get; set; }
    public TimeOnly? CheckOut { get; set;}
    public string? WorkedHours { get; set;}
}