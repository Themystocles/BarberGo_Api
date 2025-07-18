using AutoMapper;
using Api.DTOs;
using Api.Entities;


public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AppUser, AppUser>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<AppointmentDto, Appointment>();
        CreateMap<Appointment, AppointmentDto>();


        CreateMap<WeeklySchedule, WeeklySchedule>()
    .ForMember(dest => dest.Id, opt => opt.Ignore()) 
    .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Haircut, Haircut>();
    }
}
