using AutoMapper;
using CafeManagement.Data.DataModels.DTOs;
using CafeManagement.Data.DataModels.Entities;
using CafeManagement.Data.DataModels.Models;
using CafeManagement.Data.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CafeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly int maxPageSize = 20;

        public UsersController(IUserService userService, IMapper mapper)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers
            (Role? role, string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if(pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }
            var(userEntities, paginationMetaData) = await userService.GetAllUsersAsync(role, searchQuery, pageNumber, pageSize);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetaData));
            return Ok(mapper.Map<IEnumerable<User>>(userEntities));
        }

        [HttpGet("{id}", Name ="GetUser")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            UserEntity? userEntity = await userService.GetUserByIdAsync(id);
            if(userEntity == null)
            {
                return NotFound("User with this id was not found");
            }
            return Ok(mapper.Map<User>(userEntity));
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddUser(UserDto user)
        {
            if(user == null)
            {
                return BadRequest("Please provide user to add");
            }
            UserEntity userEntity = mapper.Map<UserEntity>(user);
            await userService.AddUserAsync(userEntity);
            await userService.SaveChangesAsync();

            User userToShip = mapper.Map<User>(userEntity);

            return CreatedAtRoute("GetUser",
                new
                {
                    Id = userEntity.Id,
                }, userToShip);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteUser(int id)
        {
            UserEntity? userEntity = await userService.GetUserByIdAsync(id);
            if(userEntity == null)
            {
                return BadRequest("User with this id was not found");
            }
            await userService.DeleteUserAsync(id);
            return Ok(await userService.SaveChangesAsync());
        }
    }
}
