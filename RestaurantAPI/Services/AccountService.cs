using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDTO DTO);
        string GenerateJWT(LoginDTO dTO);
    }

    public class AccountService : IAccountService
    {
        private readonly RestaurantDBContext _context;
        private readonly IPasswordHasher<User> _hasher;
        private readonly AuthenticationSettings _authSettings;

        public AccountService(RestaurantDBContext context, IPasswordHasher<User> hasher, AuthenticationSettings authSettings)
        {
            _context = context;
            _hasher = hasher;
            _authSettings = authSettings;
        }

        public string GenerateJWT(LoginDTO dTO)
        {
            var user = _context.Users.Include(user => user.Role).FirstOrDefault(u => u.Email == dTO.Email);
            if(user is null)
            {
                throw new BadRequestException("invalid username or password");
            }

            var res = _hasher.VerifyHashedPassword(user, user.PasswordHash, dTO.Password);
            if(res == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("invalid username or password");
            }
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
                new Claim("BirthDate", user.BirthDate.Value.ToString("yyyy-MM-dd")),

            };

            if (!string.IsNullOrEmpty(user.Nationality))
            {
                claims.Add(
                    new Claim("Nationality", user.Nationality)
                    );
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authSettings.JwtIssuer,
                _authSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public void RegisterUser(RegisterUserDTO DTO)
        {
            var newUser = new User()
            {
                Email = DTO.Email,
                Nationality = DTO.Nationality,
                BirthDate = DTO.BirthDate,
                RoleId = DTO.RoleId
            };
            var hashedPassword = _hasher.HashPassword(newUser, DTO.Password);
            newUser.PasswordHash = hashedPassword;

            _context.Users.Add(newUser);
            _context.SaveChanges();

        }
    }
}
