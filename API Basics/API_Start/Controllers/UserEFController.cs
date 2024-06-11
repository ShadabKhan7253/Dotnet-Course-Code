using API_Start.Data;
using API_Start.Dtos;
using API_Start.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API_Start.Controllers;

[ApiController] // it will help to send the json data and receive json data  
[Route("[controller]")]
public class UserEFController : ControllerBase 
{
    DataContextEF _entityFramework;
    IMapper _mapper;
    public UserEFController(IConfiguration config) 
    {
        _entityFramework = new DataContextEF(config);
        _mapper = new Mapper(new MapperConfiguration(cfg =>{
            cfg.CreateMap<UserToAddDtos, User>();
        }));

    }

    [HttpGet("GetUsers")]

    public IEnumerable<User> GetUsers()
    {
        IEnumerable<User> users = _entityFramework.Users.ToList<User>();
        return users;
    }

    [HttpGet("GetSingleUser/{userId}")] 
    public User GetSingleUsers(int userId)
    {
        // Here FirstOrDefault() return a single user if exist otherwise it return a null value therefore we have to make User as nullable
        User? user = _entityFramework.Users
            .Where(u => u.UserId == userId)
            .FirstOrDefault();

        if(user != null)
        {
            return user; 
        }
        throw new Exception("Failed to get user");
    }

    [HttpPut("EditUser")]

    // IActionResult return a response that tell you what happen by return chank of data here we return that request is successful or failed
    public IActionResult EditUser(User user)
    {
        User? userDb = _entityFramework.Users
            .Where(u => u.UserId == user.UserId)
            .FirstOrDefault();

        if(userDb != null)
        {
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;
            userDb.Active = user.Active;
            if(_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Failed to update user");
        }
        throw new Exception("Failed to get user");
        
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDtos user)
    {
        // User? userDb = new User();
        User userDb = _mapper.Map<User>(user);

        // userDb.FirstName = user.FirstName;
        // userDb.LastName = user.LastName;
        // userDb.Email = user.Email;
        // userDb.Gender = user.Gender;
        // userDb.Active = user.Active;
        
        _entityFramework.Add(userDb);
        if(_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }
        throw new Exception("Failed to add user");
        
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _entityFramework.Users
            .Where(u => u.UserId == userId)
            .FirstOrDefault();

        if(userDb != null)
        {
            _entityFramework.Users.Remove(userDb);
            if(_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Failed to delete user");
        }
        throw new Exception("Failed to get user");

    }
}
