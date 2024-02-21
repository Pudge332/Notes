using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
namespace Notes.Application.Users.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserVm>
    {
        private readonly IUsersDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IUsersDbContext dbContext, IMapper mapper) => (_dbContext, _mapper) = (dbContext, mapper);
        
        public async Task<UserVm> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Users.FirstOrDefaultAsync(user => user.Login == request.Login && user.Password == request.Password, cancellationToken);

            return _mapper.Map<UserVm>(entity);
        }
    }
}
