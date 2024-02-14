using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Interfaces;

namespace Notes.Application.Users.Queries.GetUserList
{
    public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, UserListVm>
    {
        private readonly IUsersDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetUserListQueryHandler(IUsersDbContext dbContext, IMapper mapper) => (_dbContext, _mapper) = (dbContext, mapper);
        public async Task<UserListVm> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var notesQuery = await _dbContext.Users.Where(note => note.Id != Guid.Empty).
                ProjectTo<UserLookupDto>(_mapper.ConfigurationProvider).
                ToListAsync(cancellationToken);

            return new UserListVm { Users = notesQuery };
        }
    }
}
