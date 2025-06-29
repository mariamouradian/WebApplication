using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Seminar4Application.Abstraction;
using Seminar4Application.DataStore.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Seminar4Application.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserService(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Guid UserAdd(string name, string password, UserRole roleId)
        {
            var users = new List<UserEntity>();
            using(_context)
            {
                var userExist = _context.Users.Where(x => x.UserName.ToLower().Equals(name.ToLower()));
                UserEntity entity = null;
                if(userExist != null)
                {
                    return default;
                }
                entity = new UserEntity
                {
                    Id = Guid.NewGuid(),
                    UserName = name,
                    Password = password,
                    RoleType = roleId
                };
                return entity.Id;
            }
        }

        public string CheckUserRole(string name, string password)
        {
            using (_context)
            {
                var entity = _context.Users
                    .FirstOrDefault(
                    x => x.UserName.ToLower().Equals(name.ToLower()) &&
                    x.Password.Equals(password));

                if(entity == null)
                {
                    return "";
                }

                var user =  new UserModel
                {
                    UserName = entity.UserName,
                    Password = entity.Password,
                    Role = entity.RoleType

                };

                return GenerateToken(user);
            }
        }

        private string GenerateToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,user.UserName.ToString()),
                new Claim(ClaimTypes.Role,user.Role.ToString())
            };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
