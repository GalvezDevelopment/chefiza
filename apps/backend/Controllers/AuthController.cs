using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using ChefizaApi.ApiModels;
using ChefizaApi.Dtos;
using ChefizaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace ChefizaApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _service;
        private readonly IConfiguration _configuration;
        private IMemoryCache _cache { get; set; }

        public AuthController(UserService userService, IConfiguration configuration, IMemoryCache cache)
        {
            _service = userService;
            _configuration = configuration;
            _cache = cache;
        }

        [HttpPost("register")]
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

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<string>>> Login(LoginDto login)
        {
            var result = await _service.Login(login);
            if (result == null)
            {
                return BadRequest(new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Success = false,
                    Data = "Invalid username or password"
                });
            }

            var token = GetToken((UserListDto)result);

            return Ok(new ApiResponse<string>
            {
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Data = token
            });
        }

        [Authorize]
        [HttpGet("logout")]
        public ActionResult<ApiResponse<string>> Logout()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == null)
            {
                return Conflict(new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Success = false,
                    Message = "Token not found"
                });
            }

            var token_expiration = short.Parse(_configuration["JWT:Expiration"]);
            _cache.Set(token.Replace("Bearer ", ""), true, TimeSpan.FromMinutes(token_expiration));

            return Ok(new ApiResponse<string>
            {
                StatusCode = StatusCodes.Status200OK,
                Success = true
            });
        }

        private string GetToken(UserListDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token_expiration = short.Parse(_configuration["JWT:Expiration"]);
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"]),
                    new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"])
                }),
                Expires = DateTime.UtcNow.AddMinutes(token_expiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}