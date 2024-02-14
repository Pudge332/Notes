using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Users.Commands
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
