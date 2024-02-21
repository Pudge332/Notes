using MediatR;

namespace Notes.Application.Users.Commands
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
