using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using backend.Models;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMongoCollection<User> _usersCollection;
    private readonly IConfiguration _configuration;

    public UserController(IMongoClient mongoClient, IConfiguration configuration)
    {
        var database = mongoClient.GetDatabase("mydashboarddb");
        _usersCollection = database.GetCollection<User>("users");
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IEnumerable<User>> Get()
    {
        return await _usersCollection.Find(_ => true).ToListAsync();
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<User>> Get(string id)
    {
        var user = await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
        if (user == null)
        {
            return NotFound();
        }
        return user;
    }

    [HttpPost("signup")]
    public async Task<ActionResult<User>> Signup(User user)
    {
        var existingUser = await _usersCollection.Find(u => u.Email == user.Email).FirstOrDefaultAsync();
        if (existingUser != null)
        {
            return BadRequest("User with this email already exists.");
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        await _usersCollection.InsertOneAsync(user);
        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginRequest loginRequest)
    {
        var user = await _usersCollection.Find(u => u.Email == loginRequest.Email).FirstOrDefaultAsync();
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
        {
            return Unauthorized("Invalid email or password.");
        }

        var token = GenerateJwtToken(user);
        return Ok(token);
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, User updatedUser)
    {
        var result = await _usersCollection.ReplaceOneAsync(u => u.Id == id, updatedUser);
        if (result.MatchedCount == 0)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _usersCollection.DeleteOneAsync(u => u.Id == id);
        if (result.DeletedCount == 0)
        {
            return NotFound();
        }
        return NoContent();
    }
}

public class LoginRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
