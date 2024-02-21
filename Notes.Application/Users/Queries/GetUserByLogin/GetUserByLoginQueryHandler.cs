using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Interfaces;
using Notes.Application.Users.Queries.GetUser;

namespace Notes.Application.Users.Queries.GetUserByLogin
{
    public class GetUserByLoginQueryHandler : IRequestHandler<GetUserByLoginQuery, UserVm>
    {
        private readonly IMapper _mapper;
        private readonly IUsersDbContext _dbContext;

        public GetUserByLoginQueryHandler(IMapper mapper, IUsersDbContext dbContext) => (_mapper, _dbContext) = (mapper, dbContext);
        public async Task<UserVm> Handle(GetUserByLoginQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Users.FirstOrDefaultAsync(user => user.Login == request.Login, cancellationToken);

            return _mapper.Map<UserVm>(entity); 
        }
    }
}
