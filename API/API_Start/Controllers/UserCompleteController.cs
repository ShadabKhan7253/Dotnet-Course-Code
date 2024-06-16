using System.Data;
using API_Start.Data;
using API_Start.Helpers;
using API_Start.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserCompleteController : ControllerBase
{
    private readonly DataContextDapper _dapper;
    private readonly ReusableSql _reusableSql;
    public UserCompleteController(IConfiguration config)
    {
         _dapper = new DataContextDapper(config);
        _reusableSql = new ReusableSql(config);;
    }

    [HttpGet("GetUsers/{userId}/{isActive}")]
    // public IEnumerable<User> GetUsers()
    public IEnumerable<UserComplete> GetUsers(int userId, bool isActive)
    {
        string sql = @"EXEC TutorialAppSchema.spUsers_Get";
        // string parameters = "";
        string stringParameters = "";
        DynamicParameters sqlParameters = new DynamicParameters();

        
        if (userId != 0)
        {
            // parameters += ", @UserId=" + userId.ToString();
            stringParameters += ", @UserId=@UserIdParameter";
            sqlParameters.Add("@UserIdParameter", userId, DbType.Int32 );
        }
        if (isActive)
        {
            // parameters += ", @Active=" + isActive.ToString();
            stringParameters += ", @Active=@ActiveParameter";
            sqlParameters.Add("@ActiveParameter", isActive, DbType.Boolean );
        }

        if (stringParameters.Length > 0)
        {
            // sql += parameters.Substring(1);//, parameters.Length);
            sql += stringParameters.Substring(1);//, parameters.Length);
        }

        // IEnumerable<UserComplete> users = _dapper.LoadData<UserComplete>(sql);
        IEnumerable<UserComplete> users = _dapper.LoadDataWithParameters<UserComplete>(sql,sqlParameters);
        return users;
    }
    
    [HttpPut("UpsertUser")]
    public IActionResult UpsertUser(UserComplete user)
    {
        // // string sql = @"EXEC TutorialAppSchema.spUser_Upsert
        // //     @FirstName = '" + user.FirstName + 
        // //     "', @LastName = '" + user.LastName +
        // //     "', @Email = '" + user.Email + 
        // //     "', @Gender = '" + user.Gender + 
        // //     "', @Active = '" + user.Active + 
        // //     "', @JobTitle = '" + user.JobTitle + 
        // //     "', @Department = '" + user.Department + 
        // //     "', @Salary = '" + user.Salary + 
        // //     "', @UserId = " + user.UserId;


        // // if (_dapper.ExecuteSql(sql))
        // // {
        // //     return Ok();
        // // } 

        // // throw new Exception("Failed to Update User");

        //  string sql = @"EXEC TutorialAppSchema.spUser_Upsert
        //     @FirstName = @FirstNameParameter, 
        //     @LastName = @LastNameParameter, 
        //     @Email = @EmailParameter, 
        //     @Gender = @GenderParameter, 
        //     @Active = @ActiveParameter, 
        //     @JobTitle = @JobTitleParameter, 
        //     @Department = @DepartmentParameter, 
        //     @Salary = @SalaryParameter, 
        //     @UserId = @UserIdParameter";

        // DynamicParameters sqlParameters = new DynamicParameters();

        // sqlParameters.Add("@FirstNameParameter", user.FirstName, DbType.String);
        // sqlParameters.Add("@LastNameParameter", user.LastName, DbType.String);
        // sqlParameters.Add("@EmailParameter", user.Email, DbType.String);
        // sqlParameters.Add("@GenderParameter", user.Gender, DbType.String);
        // sqlParameters.Add("@ActiveParameter", user.Active, DbType.Boolean);
        // sqlParameters.Add("@JobTitleParameter", user.JobTitle, DbType.String);
        // sqlParameters.Add("@DepartmentParameter", user.Department, DbType.String);
        // sqlParameters.Add("@SalaryParameter", user.Salary, DbType.Decimal);
        // sqlParameters.Add("@UserIdParameter", user.UserId, DbType.Int32);

        if (_reusableSql.UpsertUser(user))
        {
            return Ok();
        } 

        throw new Exception("Failed to Update User");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        // string sql = @"TutorialAppSchema.spUser_Delete
        //     @UserId = " + userId.ToString();

        // if (_dapper.ExecuteSql(sql))
        // {
        //     return Ok();
        // } 

        // throw new Exception("Failed to Delete User");

        string sql = @"TutorialAppSchema.spUser_Delete
            @UserId = @UserIdParameter";

        DynamicParameters sqlParameters = new DynamicParameters();
        sqlParameters.Add("@UserIdParameter", userId, DbType.Int32);

        if (_dapper.ExecuteSqlWithParameters(sql, sqlParameters))
        {
            return Ok();
        } 

        throw new Exception("Failed to Delete User");
    }
}