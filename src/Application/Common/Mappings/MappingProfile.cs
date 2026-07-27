
using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

        CreateMap<Attendance, AttendanceDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.WorkedHours, opt => opt.MapFrom(src =>
                src.WorkedHours.HasValue
                    ? $"{(int)src.WorkedHours.Value.TotalHours}h {src.WorkedHours.Value.Minutes}m"
                    : null));

        CreateMap<Product, ProductDto>();
    }


}