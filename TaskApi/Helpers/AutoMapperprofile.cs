using AutoMapper;
using TaskApi.Models;

namespace TaskApi.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDTO, User>()
            .ForAllMembers(x =>
            {
                x.Condition((src, dst, prop) =>
                {
                    //if (prop == null) return false;
                    //if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;
                    return true;
                });
            });

            CreateMap<ListDTO, List>()
            .ForAllMembers(x =>
            {
                x.Condition((src, dst, prop) =>
                {
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;
                    return true;
                });
            });

            CreateMap<TaskDTO, TaskType>()
            .ForAllMembers(x =>
            {
                x.Condition((src, dst, prop) =>
                {
                   // if (prop == null) return false;
                   // if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;
                    return true;
                });
            });
        }
    }
}