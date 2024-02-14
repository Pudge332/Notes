using MediatR;
using Notes.Application.Interfaces;
using Notes.Users;

namespace Notes.Application.Users.Commands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUsersDbContext _dbContext;

        public CreateUserCommandHandler(IUsersDbContext dbContext) => _dbContext = dbContext;
        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Login = request.Login,
                Password = request.Password
            };

            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}
