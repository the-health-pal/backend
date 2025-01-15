using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using health_pal_backend.DTOs;
using health_pal_backend.Models;
using health_pal_backend.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace health_pal_backend.Services;

public interface IUserService{

        Task<string> RegisterAsync(UserRegDTO dto);
        Task<string> LoginAsync(UserLoginDTO dto);
        Task<IEnumerable<UserModel>> GetUsersAsync();
        Task<UserModel> GetUserByIdAsync(int id);
        Task UpdateUserAsync(int id, UserUpdateDTO dto);
        Task DeleteUserAsync(int id);

}
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    public UserService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<string> RegisterAsync(UserRegDTO dto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);

        if(existingUser != null)
        {
            return "User with this email already exists";
        }

        var user = new UserModel
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return GenerateJwtToken(user);
    }

    public async Task<string> LoginAsync(UserLoginDTO dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
        {
            throw new Exception("Invalid email or password.");
        }
        return GenerateJwtToken(user);
    }

     public async Task<IEnumerable<UserModel>> GetUsersAsync()
    {
        return await _userRepository.GetAllAsync();
    }

     public async Task<UserModel> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task UpdateUserAsync(int id, UserUpdateDTO dto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        user.Name = dto.Name ?? user.Name;
        user.Email = dto.Email ?? user.Email;
        if (!string.IsNullOrEmpty(dto.Password))
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        }

        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            throw new Exception("User not found.");
        }

        await _userRepository.DeleteAsync(user);
        await _userRepository.SaveChangesAsync();
    }

    private string GenerateJwtToken(UserModel user)
    {
        var claims = new []
        {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("UserId", user.Id.ToString()),
            new Claim("Email", user.Email),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: signIn
        );

        string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenValue;
    }
}
