using TechStudents.Utils.Entities;

namespace TechStudents.Web.Models
{
    public class UsersModel
    {
        public List<User> GetAll()
        {
            return User.QuerryAll();
        }
    }
}
