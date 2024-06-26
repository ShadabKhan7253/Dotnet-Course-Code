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

    IUserRepository _userRepository;
    IMapper _mapper;
    public UserEFController(IConfiguration config,IUserRepository userRepository) 
    {
        _entityFramework = new DataContextEF(config);

        _userRepository = userRepository;

        _mapper = new Mapper(new MapperConfiguration(cfg =>{
            cfg.CreateMap<UserToAddDtos, User>();
            cfg.CreateMap<UserSalary, UserSalary>();
            cfg.CreateMap<UserJobInfo, UserJobInfo>();
        }));

    }

    [HttpGet("GetUsers")]

    public IEnumerable<User> GetUsers()
    {
        // IEnumerable<User> users = _entityFramework.Users.ToList<User>();
        IEnumerable<User> users = _userRepository.GetUsers();
        return users;
    }

    [HttpGet("GetSingleUser/{userId}")] 
    public User GetSingleUsers(int userId)
    {
        // Here FirstOrDefault() return a single user if exist otherwise it return a null value therefore we have to make User as nullable
        // User? user = _entityFramework.Users
        //     .Where(u => u.UserId == userId)
        //     .FirstOrDefault();

        User? user = _userRepository.GetSingleUsers(userId);

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
        // User? userDb = _entityFramework.Users
        //     .Where(u => u.UserId == user.UserId)
        //     .FirstOrDefault();

        User? userDb = _userRepository.GetSingleUsers(user.UserId);

        if(userDb != null)
        {
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;
            userDb.Active = user.Active;
            if(_userRepository.SaveChanges())
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

        _userRepository.AddEntity<User>(userDb);
        if(_userRepository.SaveChanges())
        {
            return Ok();
        }
        throw new Exception("Failed to add user");
        
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        // User? userDb = _entityFramework.Users
        //     .Where(u => u.UserId == userId)
        //     .FirstOrDefault();

        User userDb = _userRepository.GetSingleUsers(userId);

        if(userDb != null)
        {
            _userRepository.RemoveEntity<User>(userDb);
            if(_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Failed to delete user");
        }
        throw new Exception("Failed to get user");

    }

    [HttpGet("UserSalary/{userId}")]
    public UserSalary GetUserSalaryEF(int userId)
    {
        // return _entityFramework.UserSalary
        //     .Where(u => u.UserId == userId)
        //     .ToList();

        return _userRepository.GetSingleUserSalary(userId);
    }

    [HttpPost("UserSalary")]
    public IActionResult PostUserSalaryEf(UserSalary userForInsert)
    {
        _userRepository.AddEntity<UserSalary>(userForInsert);
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }
        throw new Exception("Adding UserSalary failed on save");
    }


    [HttpPut("UserSalary")]
    public IActionResult PutUserSalaryEf(UserSalary userForUpdate)
    {
        // UserSalary? userToUpdate = _entityFramework.UserSalary
        //     .Where(u => u.UserId == userForUpdate.UserId)
        //     .FirstOrDefault();

        UserSalary userToUpdate = _userRepository.GetSingleUserSalary(userForUpdate.UserId);

        if (userToUpdate != null)
        {
            _mapper.Map(userForUpdate, userToUpdate);
            if (_userRepository.SaveChanges())
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
        // UserSalary? userToDelete = _entityFramework.UserSalary
        //     .Where(u => u.UserId == userId)
        //     .FirstOrDefault();

        UserSalary? userToDelete = _userRepository.GetSingleUserSalary(userId);


        if (userToDelete != null)
        {
            _userRepository.RemoveEntity<UserSalary>(userToDelete);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Deleting UserSalary failed on save");
        }
        throw new Exception("Failed to find UserSalary to delete");
    }


    [HttpGet("UserJobInfo/{userId}")]
    public UserJobInfo GetUserJobInfoEF(int userId)
    {
        // return _entityFramework.UserJobInfo
        //     .Where(u => u.UserId == userId)
        //     .ToList();

        return _userRepository.GetSingleUserJobInfo(userId);
    }

    [HttpPost("UserJobInfo")]
    public IActionResult PostUserJobInfoEf(UserJobInfo userForInsert)
    {
        _userRepository.AddEntity<UserJobInfo>(userForInsert);
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }
        throw new Exception("Adding UserJobInfo failed on save");
    }


    [HttpPut("UserJobInfo")]
    public IActionResult PutUserJobInfoEf(UserJobInfo userForUpdate)
    {
        // UserJobInfo? userToUpdate = _entityFramework.UserJobInfo
        //     .Where(u => u.UserId == userForUpdate.UserId)
        //     .FirstOrDefault();

        UserJobInfo? userToUpdate = _userRepository.GetSingleUserJobInfo(userForUpdate.UserId);

        if (userToUpdate != null)
        {
            _mapper.Map(userForUpdate, userToUpdate);
            if (_userRepository.SaveChanges())
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
        // UserJobInfo? userToDelete = _entityFramework.UserJobInfo
        //     .Where(u => u.UserId == userId)
        //     .FirstOrDefault();

        UserJobInfo? userToDelete = _userRepository.GetSingleUserJobInfo(userId);

        if (userToDelete != null)
        {
            _userRepository.RemoveEntity<UserJobInfo>(userToDelete);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Deleting UserJobInfo failed on save");
        }
        throw new Exception("Failed to find UserJobInfo to delete");
    }
}
