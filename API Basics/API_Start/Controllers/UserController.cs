
using Microsoft.AspNetCore.Mvc;

namespace API_Start.Controllers;

[ApiController] // it will help to send the json data and receive json data  
[Route("[controller]")]
public class UserController : ControllerBase 
{
    public UserController() 
    {

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
