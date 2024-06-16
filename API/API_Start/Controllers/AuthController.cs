using System.Data;
using API_Start.Data;
using API_Start.Dtos;
using Dapper;
using API_Start.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using API_Start.Models;

namespace API_Start.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        private readonly AuthHelper _authHelper;
        private readonly ReusableSql _reusableSql;
        private readonly IMapper _mapper;
        public AuthController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
            _authHelper = new AuthHelper(config);
            _reusableSql = new ReusableSql(config);
            _mapper = new Mapper(new MapperConfiguration(cfg => 
            {
                cfg.CreateMap<UserForRegistrationDto, UserComplete>();
            }));
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration.Password == userForRegistration.PasswordConfirm)
            {
                string sqlCheckUserExists = "SELECT Email FROM TutorialAppSchema.Auth WHERE Email = '" +
                    userForRegistration.Email + "'";

                IEnumerable<string> existingUsers = _dapper.LoadData<string>(sqlCheckUserExists);
                if (existingUsers.Count() == 0)
                {
                    UserForLoginDto userForSetPassword = new UserForLoginDto() {
                        Email = userForRegistration.Email,
                        Password = userForRegistration.Password
                    };
                    if (_authHelper.SetPassword(userForSetPassword))
                    {
                        // string sqlAddUser = @"EXEC TutorialAppSchema.spUser_Upsert
                        //     @FirstName = '" + userForRegistration.FirstName + 
                        //     "', @LastName = '" + userForRegistration.LastName +
                        //     "', @Email = '" + userForRegistration.Email + 
                        //     "', @Gender = '" + userForRegistration.Gender + 
                        //     "', @Active = 1" + 
                        //     ", @JobTitle = '" + userForRegistration.JobTitle + 
                        //     "', @Department = '" + userForRegistration.Department + 
                        //     "', @Salary = '" + userForRegistration.Salary + "'";

                        UserComplete userComplete = _mapper.Map<UserComplete>(userForRegistration);
                        userComplete.Active = true;
                        
                        if (_reusableSql.UpsertUser(userComplete))
                        {
                            return Ok();
                        }
                        throw new Exception("Failed to add user.");
                    }
                    throw new Exception("Failed to register user.");
                }
                throw new Exception("User with this email already exists!");
            }
            throw new Exception("Passwords do not match!");
        }

        [HttpPut("ResetPassword")]
        public IActionResult ResetPassword(UserForLoginDto userForSetPassword)
        {
            if (_authHelper.SetPassword(userForSetPassword))
            {
                return Ok();
            }
            throw new Exception("Failed to update password!");
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLogin)
        {
            string sqlForHashAndSalt = @"EXEC TutorialAppSchema.spLoginConfirmation_Get 
                @Email = @EmailParam";

            DynamicParameters sqlParameters = new DynamicParameters();

            // SqlParameter emailParameter = new SqlParameter("@EmailParam", SqlDbType.VarChar);
            // emailParameter.Value = userForLogin.Email;
            // sqlParameters.Add(emailParameter);

            sqlParameters.Add("@EmailParam", userForLogin.Email, DbType.String);

            UserForLoginConfirmationDto userForConfirmation = _dapper
                .LoadDataSingleWithParameters<UserForLoginConfirmationDto>(sqlForHashAndSalt, sqlParameters);

            byte[] passwordHash = _authHelper.GetPasswordHash(userForLogin.Password, userForConfirmation.PasswordSalt);

            // if (passwordHash == userForConfirmation.PasswordHash) // Won't work

            for (int index = 0; index < passwordHash.Length; index++)
            {
                if (passwordHash[index] != userForConfirmation.PasswordHash[index]){
                    return StatusCode(401, "Incorrect password!");
                }
            }

            string userIdSql = @"
                SELECT UserId FROM TutorialAppSchema.Users WHERE Email = '" +
                userForLogin.Email + "'";

            int userId = _dapper.LoadDataSingle<int>(userIdSql);

            return Ok(new Dictionary<string, string> {
                {"token", _authHelper.CreateToken(userId)}
            });
        }

        // [AllowAnonymous]
        // [HttpPost("Register")]
        // public IActionResult Register(UserForRegistrationDto userForRegistration)
        // {
        //     if (userForRegistration.Password == userForRegistration.PasswordConfirm)
        //     {
        //         string sqlCheckuserExists = @"
        //             SELECT Email FROM TutorialAppSchema.Auth
        //                 WHERE EMAIL = '" + userForRegistration.Email + "'";
        //         IEnumerable<string> existingUser = _dapper.LoadData<string>(sqlCheckuserExists);

        //         if(existingUser.Count() == 0)
        //         {
        //             byte[] passwordSalt = new byte[128 / 8];
        //             using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        //             {
        //                 rng.GetNonZeroBytes(passwordSalt);
        //             }

        //             byte[] passwordHash = _authHelper.GetPasswordHash(userForRegistration.Password, passwordSalt);

        //             string sqlAddAuth = @"
        //             INSERT INTO TutorialAppSchema.Auth (
        //                 [Email],
        //                 [PasswordHash],
        //                 [PasswordSalt]
        //             ) VALUES ('" + userForRegistration.Email +  
        //             "', @PasswordHash, @PasswordSalt) ";

        //             List<SqlParameter> sqlParameters = new List<SqlParameter>();
        //             SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSalt",SqlDbType.VarBinary);
        //             passwordSaltParameter.Value = passwordSalt;

        //             SqlParameter passwordHashParameter = new SqlParameter("@PasswordHash",SqlDbType.VarBinary);
        //             passwordHashParameter.Value = passwordHash;

        //             sqlParameters.Add(passwordSaltParameter);
        //             sqlParameters.Add(passwordHashParameter);

        //             if(_dapper.ExecuteSqlWithParameters(sqlAddAuth,sqlParameters))
        //             {
        //                 string sqlAddUser = @"
        //                     INSERT INTO TutorialAppSchema.Users
        //                     (
        //                     [FirstName],
        //                     [LastName],
        //                     [Email],
        //                     [Gender],
        //                     [Active]    
        //                     ) VALUES (
        //                         '" + userForRegistration.FirstName +
        //                         "','" + userForRegistration.LastName +
        //                         "','" + userForRegistration.Email +
        //                         "','" + userForRegistration.Gender +
        //                         "',1)";

        //                 if(_dapper.ExecuteSql(sqlAddUser))
        //                 {
        //                     return Ok();
        //                 }
        //                 throw new Exception("Failed to add user!");
        //             }
        //             throw new Exception("Failed to register user!");
        //         }  
        //         throw new Exception("User with this email already exist!");
        //     }
        //     throw new Exception("Password do not match!");
        // }

        // [AllowAnonymous]
        // [HttpPost("Login")]
        // public IActionResult Login(UserForLoginDto userForLogin)
        // {
        //     string sqlForHashAndSalt = @"SELECT 
        //         [PasswordHash],
        //         [PasswordSalt] FROM TutorialAppSchema.Auth WHERE Email = '" +
        //         userForLogin.Email + "'";

        //     UserForLoginConfirmationDto userForConfirmation = _dapper
        //         .LoadDataSingle<UserForLoginConfirmationDto>(sqlForHashAndSalt);

        //     byte[] passwordHash = _authHelper.GetPasswordHash(userForLogin.Password, userForConfirmation.PasswordSalt);

        //     // if (passwordHash == userForConfirmation.PasswordHash) // Won't work

        //     for (int index = 0; index < passwordHash.Length; index++)
        //     {
        //         if (passwordHash[index] != userForConfirmation.PasswordHash[index]){
        //             return StatusCode(401, "Incorrect password!");
        //         }
        //     }

        //      string userIdSql = @"
        //         SELECT UserId FROM TutorialAppSchema.Users WHERE Email = '" +
        //         userForLogin.Email + "'";

        //     int userId = _dapper.LoadDataSingle<int>(userIdSql);

        //     return Ok(new Dictionary<string, string> {
        //         {"token", _authHelper.CreateToken(userId)}
        //     });
        // }

        [HttpGet("RefreshToken")]
        public string RefreshToken()
        {
            string userIdSql = @"
                SELECT UserId FROM TutorialAppSchema.Users WHERE UserId = '" +
                User.FindFirst("userId")?.Value + "'";

            int userId = _dapper.LoadDataSingle<int>(userIdSql);

            return _authHelper.CreateToken(userId);
        }
    }
}