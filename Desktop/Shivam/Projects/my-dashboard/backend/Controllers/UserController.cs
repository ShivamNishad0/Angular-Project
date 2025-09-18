using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using backend.Models;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMongoCollection<User> _usersCollection;

    public UserController(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("mydashboarddb");
        _usersCollection = database.GetCollection<User>("users");
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

    [HttpPost]
    public async Task<ActionResult<User>> Create(User user)
    {
        await _usersCollection.InsertOneAsync(user);
        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
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
