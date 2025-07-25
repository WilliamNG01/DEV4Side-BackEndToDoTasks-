using AutoMapper;
using WebAPITodoList.Models;

namespace WebAPITodoList.Mappings;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src =>
                src.UserRoles.Select(ur => ur.Role.RoleName).ToList()
            ));
        CreateMap<ToDoList, ToDoDto>();
    }
}
