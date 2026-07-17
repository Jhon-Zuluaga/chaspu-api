
public class Attendance : BaseEntity
{
    public int UserId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly? CheckIn { get; set; }
    public TimeOnly ? CheckOut { get; set; }

    // Navigation property
    public User User { get; set; } = null!;

    // Helper (Not mapped) - calculates worked hours
    public TimeSpan? WorkedHours =>
        CheckIn.HasValue && CheckOut.HasValue
            ? CheckOut.Value.ToTimeSpan() - CheckIn.Value.ToTimeSpan()
            : null;
}