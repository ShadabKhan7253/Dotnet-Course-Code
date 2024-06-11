
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

    [HttpGet("TestConnection")]

    public DateTime TestConnection()
    {   
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    [HttpGet("GetUsers/{testValue}")] // it will define the http get method for the route
    public string[] GetUsers(string testValue)
    {
        string[] responseArray = new string[] {
            "test1",
            "test2",
            testValue
        };
        return responseArray;
    }
}
