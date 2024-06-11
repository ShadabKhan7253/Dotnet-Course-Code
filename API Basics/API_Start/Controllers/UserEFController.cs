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

    [HttpGet("UserSalary/{userId}")]
    public IEnumerable<UserSalary> GetUserSalaryEF(int userId)
    {
        return _entityFramework.UserSalary
            .Where(u => u.UserId == userId)
            .ToList();
    }

    [HttpPost("UserSalary")]
    public IActionResult PostUserSalaryEf(UserSalary userForInsert)
    {
        _entityFramework.UserSalary.Add(userForInsert);
        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }
        throw new Exception("Adding UserSalary failed on save");
    }


    [HttpPut("UserSalary")]
    public IActionResult PutUserSalaryEf(UserSalary userForUpdate)
    {
        UserSalary? userToUpdate = _entityFramework.UserSalary
            .Where(u => u.UserId == userForUpdate.UserId)
            .FirstOrDefault();

        if (userToUpdate != null)
        {
            _mapper.Map(userForUpdate, userToUpdate);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Updating UserSalary failed on save");
        }
        throw new Exception("Failed to find UserSalary to Update");
    }


    [HttpDelete("UserSalary/{userId}")]
    public IActionResult DeleteUserSalaryEf(int userId)
    {
        UserSalary? userToDelete = _entityFramework.UserSalary
            .Where(u => u.UserId == userId)
            .FirstOrDefault();

        if (userToDelete != null)
        {
            _entityFramework.UserSalary.Remove(userToDelete);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Deleting UserSalary failed on save");
        }
        throw new Exception("Failed to find UserSalary to delete");
    }


    [HttpGet("UserJobInfo/{userId}")]
    public IEnumerable<UserJobInfo> GetUserJobInfoEF(int userId)
    {
        return _entityFramework.UserJobInfo
            .Where(u => u.UserId == userId)
            .ToList();
    }

    [HttpPost("UserJobInfo")]
    public IActionResult PostUserJobInfoEf(UserJobInfo userForInsert)
    {
        _entityFramework.UserJobInfo.Add(userForInsert);
        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }
        throw new Exception("Adding UserJobInfo failed on save");
    }


    [HttpPut("UserJobInfo")]
    public IActionResult PutUserJobInfoEf(UserJobInfo userForUpdate)
    {
        UserJobInfo? userToUpdate = _entityFramework.UserJobInfo
            .Where(u => u.UserId == userForUpdate.UserId)
            .FirstOrDefault();

        if (userToUpdate != null)
        {
            _mapper.Map(userForUpdate, userToUpdate);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Updating UserJobInfo failed on save");
        }
        throw new Exception("Failed to find UserJobInfo to Update");
    }


    [HttpDelete("UserJobInfo/{userId}")]
    public IActionResult DeleteUserJobInfoEf(int userId)
    {
        UserJobInfo? userToDelete = _entityFramework.UserJobInfo
            .Where(u => u.UserId == userId)
            .FirstOrDefault();

        if (userToDelete != null)
        {
            _entityFramework.UserJobInfo.Remove(userToDelete);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Deleting UserJobInfo failed on save");
        }
        throw new Exception("Failed to find UserJobInfo to delete");
    }
}
