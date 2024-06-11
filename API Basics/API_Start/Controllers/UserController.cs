
using API_Start.Data;
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

    }
