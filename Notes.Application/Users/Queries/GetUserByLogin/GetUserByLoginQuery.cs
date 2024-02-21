using MediatR;
using Notes.Application.Users.Queries.GetUser;

namespace Notes.Application.Users.Queries.GetUserByLogin
{
    public class GetUserByLoginQuery : IRequest<UserVm>
    {
        public string Login { get; set; }
    }
}
