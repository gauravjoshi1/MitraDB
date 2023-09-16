using Microsoft.AspNetCore.Mvc;
using MitraBackend.Models;

namespace MitraBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private static readonly List<User> Users = new()
    {
        new User {UserId = Guid.NewGuid(), UserName = "Jerry"},
        new User{UserId = Guid.NewGuid(), UserName = "George"}
    };
    
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<User> Get()
    {
        return Users;
    }

    [HttpPost]
    public async Task<ActionResult<User>> Post([FromForm] User user, IFormFile? image)
    {
        user.UserId = Guid.NewGuid();
        
        // if there already exists a guid in the db
        while (Users.Any(u => u.UserId == user.UserId))
        {
            user.UserId = Guid.NewGuid();
        }

        if (image != null)
        {
            string filePath = Path.Combine("Images", $"{user.UserId}.jpeg");
            
            await using (var stream = System.IO.File.Create(filePath))
            {
                await image.CopyToAsync(stream);
            }

            user.UserProfilePicturePath = filePath; 
        }
        
        Users.Add(user);

        return CreatedAtAction(nameof(Get), new {id = user.UserId}, user);
    }
}