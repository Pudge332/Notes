using MediatR;
using Notes.Application.Users.Queries.GetUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Users.Queries.GetUserByLogin
{
    public class GetUserByLoginQuery : IRequest<UserVm>
    {
        public string Login { get; set; }
    }
}
