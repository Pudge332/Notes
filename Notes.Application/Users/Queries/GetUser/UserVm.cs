using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Application.Notes.Queries.GetNoteDetails;
using Notes.Domain;
using Notes.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
