using AutoMapper;
using CafeManagement.Data.DataModels.DTOs;
using CafeManagement.Data.DataModels.Entities;
using CafeManagement.Data.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace CafeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AccountController(IUserService userService, ITokenService tokenService, IMapper mapper)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginUserDto loginDto)
        {
            UserEntity? user = await userService.GetUserByUsernameAsync(loginDto.Username);
            if (user == null)
            {
                return Unauthorized("Invalid Password");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized();
                }
            }
            UserDto userDto = new UserDto
            {
                Username = user.Username,
                Token = tokenService.CreateToken(user)
            };
            return Ok(userDto);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterUserDto registerDto)
        {
            if (await userService.IsUserExists(registerDto.Username))
            {
                return BadRequest("A user with the provided username is already in the system");
            }

            //salting the password so that it generates a different string in jwt every time
            using HMACSHA512 hmac = new HMACSHA512();

            UserEntity userEntity = mapper.Map<UserEntity>(registerDto);
            userEntity.Username = registerDto.Username.ToLower();
            userEntity.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            userEntity.PasswordSalt = hmac.Key;

            //UserEntity userEntity = new UserEntity
            //{
            //    Username = registerDto.Username.ToLower(),
            //    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            //    PasswordSalt = hmac.Key
            //};
            await userService.AddUserAsync(userEntity);
            await userService.SaveChangesAsync();
            UserDto userDto = new UserDto
            {
                Username = userEntity.Username,
                Token = tokenService.CreateToken(userEntity)
            };
            return Ok(userDto);
        }
    }
}
