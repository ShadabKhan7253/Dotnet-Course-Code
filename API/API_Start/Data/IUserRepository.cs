using API_Start.Models;

namespace API_Start.Data 
{
    public interface IUserRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToRemove);
        public IEnumerable<User> GetUsers();
        public User GetSingleUsers(int userId);
        public UserSalary GetSingleUserSalary(int userId);
        public UserJobInfo GetSingleUserJobInfo(int userId);
    }
}