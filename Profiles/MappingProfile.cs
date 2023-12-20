using AutoMapper;
using StudentManagement.API.Dtos;
using StudentManagement.API.Entities;

namespace StudentManagement.API.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<EnrollementDTO, Enrollment>();
    }
}
