using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Application.Users.Commands;
using Notes.Application.Users.Queries.GetUser;
using Notes.Application.Users.Queries.GetUserByLogin;
using Notes.Application.Users.Queries.GetUserList;
using Notes.Users;
using Notes.WebApi.Models;
using Notes.WebApi.Services;
using System.Security.Claims;

namespace Notes.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly JwtProvider _jwtProvider;
        private CurrentUserService _currentUserService;
        public UserController(IMapper mapper, JwtProvider jwtProvider, CurrentUserService currentUserService)
        {
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<ActionResult<UserListVm>> GetAllUsers() 
        {
            var query = new GetUserListQuery
            {
                Id = Guid.NewGuid()
            };

            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpPost("login")]
        public async Task<ActionResult<CreateUserDto>> LoginUser([FromBody] UserVm userVm)
        {
            Console.WriteLine(userVm.Login);
            var query = new GetUserQuery
            {
                Login = userVm.Login,
                Password = userVm.Password
            };

            var vm = await Mediator.Send(query);
            if(vm == default)
            {
                return NotFound();
            }
            var identity = new ClaimsIdentity(new List<Claim>
                {
                new Claim($"{vm.Id}" , ClaimValueTypes.Integer32)
                }, "Custom");
            
            HttpContext.User = new ClaimsPrincipal(identity);
            User user = new User
            {
                Id = vm.Id,
                Login = vm.Login,
                Password = vm.Password
            };
            var token = _jwtProvider.GenerateToken(user);
            HttpContext.Response.Cookies.Append("one-small-detail", token);
            _currentUserService.UserId = vm.Id;
            Console.WriteLine(vm.Id);
            Console.WriteLine(User.Identity.IsAuthenticated);
            Console.WriteLine(token);
            return Ok(vm.Id);
        }

        [HttpPost]
        public async Task<ActionResult<CreateUserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            var getUser = new GetUserByLoginQuery
            {
                Login = createUserDto.Login
            };
            var result = await Mediator.Send(getUser);

            if(result != default)
            {
                Console.WriteLine("User User");
                return StatusCode(409);
            }
            var command = _mapper.Map<CreateUserCommand>(createUserDto);
            var userId = await Mediator.Send(command);

            return Ok(userId);
        }
    }
}
