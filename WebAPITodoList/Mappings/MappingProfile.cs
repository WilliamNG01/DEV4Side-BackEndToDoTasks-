using AutoMapper;
using WebAPITodoList.DTOs;
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
        // Se vuoi mappare da Request a Dto (aggiungendo Id dopo)
        CreateMap<ToDoTaskRequest, ToDoTaskDto>();

        CreateMap<ToDoList, ToDoListResponse>();
        CreateMap<ToDoTask, ToDoTaskDto>();
        CreateMap<ToDoTaskDto, ToDoTask>();
    }
}
