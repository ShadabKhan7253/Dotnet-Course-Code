using API_Start.Data;
using API_Start.Dtos;
using API_Start.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_Start.Controllers;

[ApiController] // it will help to send the json data and receive json data  
[Route("[controller]")]
public class UserController : ControllerBase 
{
    DataContextDapper _dapper;
    public UserController(IConfiguration config) 
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("GetUsers")]

    public IEnumerable<User> GetUsers()
    {
        string sql = @"
            SELECT  [UserId]
                , [FirstName]
                , [LastName]
                , [Email]
                , [Gender]
                , [Active]
            FROM  TutorialAppSchema.Users;";

        IEnumerable<User> users = _dapper.LoadData<User>(sql);
        return users;
    }

    [HttpGet("GetSingleUser/{userId}")] // it will define the http get method for the route
    public User GetSingleUsers(int userId)
    {
        
        string sql = @"
            SELECT  [UserId]
                , [FirstName]
                , [LastName]
                , [Email]
                , [Gender]
                , [Active]  
            FROM  TutorialAppSchema.Users
                WHERE UserId = " + userId.ToString();

        User user = _dapper.LoadDataSingle<User>(sql);
        return user; 
    }

    [HttpPut("EditUser")]

    // IActionResult return a response that tell you what happen by return chank of data here we return that request is successful or failed
    public IActionResult EditUser(User user)
    {
        string sql = @"
            Update TutorialAppSchema.Users
                SET [FirstName] = '" + user.FirstName +
                "', [LastName] = '" + user.LastName +
                "', [Email] = '" + user.Email +
                "', [Gender] = '" + user.Gender +
                "', [Active] = '" + user.Active +
            "' WHERE UserId = " + user.UserId;

        if(_dapper.ExecuteSql(sql))
        {
            // OK() is a build in method which come from ControllerBase class which we have inherited
            return Ok();
        }
        throw new Exception("Failed to update user");
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDtos user)
    {
        string sql = @"
            INSERT INTO TutorialAppSchema.Users
            (
               [FirstName],
               [LastName],
               [Email],
               [Gender],
               [Active]    
            ) VALUES (
                '" + user.FirstName +
                "','" + user.LastName +
                "','" + user.Email +
                "','" + user.Gender +
                "','" + user.Active + 
                "')";
            Console.WriteLine(sql);

        if(_dapper.ExecuteSql(sql))
        {
            // OK() is a build in method which come from ControllerBase class which we have inherited
            return Ok();
        }
        throw new Exception("Failed to add user");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string sql = @"
            DELETE 
                FROM TutorialAppSchema.Users 
            WHERE UserId = " + userId.ToString();
        Console.WriteLine(sql);

        if(_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to delete user");

    }
}
