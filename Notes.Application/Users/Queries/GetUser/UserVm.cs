using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Users;


namespace Notes.Application.Users.Queries.GetUser
{
    public class UserVm : IMapWith<User>
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserVm>()
                .ForMember(userVm => userVm.Login,
                    opt => opt.MapFrom(user => user.Login))
                .ForMember(userVm => userVm.Password,
                    opt => opt.MapFrom(user => user.Password))
                .ForMember(userVm => userVm.Id,
                    opt => opt.MapFrom(user => user.Id));
        }
    }
}
