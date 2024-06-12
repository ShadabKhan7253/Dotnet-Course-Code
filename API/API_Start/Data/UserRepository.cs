
using API_Start.Models;

namespace API_Start.Data
{
    public class UserRepository : IUserRepository
    {
        DataContextEF _entityFramework;
        public UserRepository(IConfiguration config) 
        {
            _entityFramework = new DataContextEF(config);   
        }

        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }

        public void AddEntity<T>(T entityToAdd)
        {
            if(entityToAdd != null)
            {
                _entityFramework.Add(entityToAdd);
            }
        } 

        public void RemoveEntity<T>(T entityToRemove)
        {
            if(entityToRemove != null)
            {
                _entityFramework.Remove(entityToRemove);
            }
        }

        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = _entityFramework.Users.ToList<User>();
            return users;
        } 

        public User GetSingleUsers(int userId)
        {
            // Here FirstOrDefault() return a single user if exist otherwise it return a null value therefore we have to make User as nullable
            User? user = _entityFramework.Users
                .Where(u => u.UserId == userId)
                .FirstOrDefault<User>();

            if(user != null)
            {
                return user; 
            }
            throw new Exception("Failed to get user");
        }

        public UserSalary GetSingleUserSalary(int userId)
        {
            // Here FirstOrDefault() return a single user if exist otherwise it return a null value therefore we have to make User as nullable
            UserSalary? userSalary = _entityFramework.UserSalary
                .Where(u => u.UserId == userId)
                .FirstOrDefault<UserSalary>();

            if(userSalary != null)
            {
                return userSalary; 
            }
            throw new Exception("Failed to get user salary");
        }

        public UserJobInfo GetSingleUserJobInfo(int userId)
        {
            // Here FirstOrDefault() return a single user if exist otherwise it return a null value therefore we have to make User as nullable
            UserJobInfo? userJobInfo = _entityFramework.UserJobInfo
                .Where(u => u.UserId == userId)
                .FirstOrDefault<UserJobInfo>();

            if(userJobInfo != null)
            {
                return userJobInfo; 
            }
            throw new Exception("Failed to get user salary");
        }
    }
}