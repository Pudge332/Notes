using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Application.Notes.Queries.GetNoteList;
using Notes.Users;


namespace Notes.Application.Users.Queries.GetUserList
{
    public class UserLookupDto : IMapWith<User>
    {
        public Guid Id { get; set; }
        public string Login { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserLookupDto>()
                .ForMember(noteDto => noteDto.Id,
                    opt => opt.MapFrom(note => note.Id))
                .ForMember(noteDto => noteDto.Login,
                    opt => opt.MapFrom(note => note.Login));
        }

    }
}
