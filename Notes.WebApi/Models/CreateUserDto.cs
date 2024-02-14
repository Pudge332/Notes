using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Application.Users.Commands;

namespace Notes.WebApi.Models
{
    public class CreateUserDto : IMapWith<CreateUserCommand>
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateUserDto, CreateUserCommand>()
                .ForMember(userCommand => userCommand.Login,
                    opt => opt.MapFrom(noteDto => noteDto.Login))
                .ForMember(userCommand => userCommand.Password,
                    opt => opt.MapFrom(noteDto => noteDto.Password));
        }
    }
}
