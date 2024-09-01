using ChefizaApi.ApiModels;
using ChefizaApi.Dtos;
using ChefizaApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChefizaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;
        public UserController(UserService userService) : base()
        {
            _service = userService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> Add(CreateUserDto newUser)
        {
            var result = await _service.findByEmail(newUser.Email);
            if (result != null)
            {
                return Conflict(new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    Success = false,
                    Message = "Email already exists"
                });
            }

            await _service.Add(newUser);
            return Ok(new ApiResponse<string>
            {
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<ApiResponse<List<UserListDto>>>> GetUserByEmail(string email)
        {
            var result = await _service.findByEmail(email);
            if (result == null)
            {
                return Conflict(new ApiResponse<UserListDto>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Success = false,
                    Message = "User does not exist"
                });
            }

            return Ok(new ApiResponse<UserListDto>
            {
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Data = (UserListDto)result
            });
        }

        [HttpPatch]
        public async Task<ActionResult<ApiResponse<string>>> Update(UpdateUserDto updatedUser) {
            var result = await _service.findByEmail(updatedUser.Email);
            if (result == null) {
                return Conflict(new ApiResponse<string>{
                    StatusCode = StatusCodes.Status404NotFound,
                    Success = false,
                    Message = "User does not exist"
                });
            }

            var success = await _service.Update(updatedUser);

            return Ok(new ApiResponse<UserListDto>{
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Data = (UserListDto)success
            });
        }

        [HttpGet("delete/{email}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(string email)
        {
            var user = await _service.findByEmail(email);
            if (user == null)
            {
                return Conflict(new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Success = false,
                    Message = "User not found"
                });
            }
            await _service.DeleteById(email);
            return Ok(new ApiResponse<string>
            {
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserListDto>>> GetAll()
        {
            IEnumerable<UserListDto> users = await _service.GetAll();
            return Ok(new ApiResponse<IEnumerable<UserListDto>>
            {
                StatusCode = StatusCodes.Status200OK,
                Data = users
            });
        }
    }
}