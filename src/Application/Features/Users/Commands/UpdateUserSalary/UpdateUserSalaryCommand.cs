
using MediatR;

public class UpdateUserSalaryCommand : IRequest<UserDto>
{
    public int UserId { get; set; }
    public decimal WeeklySalary { get; set; }
}