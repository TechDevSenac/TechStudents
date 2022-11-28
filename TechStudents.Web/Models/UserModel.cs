using TechStudents.Utils.Entities;

namespace TechStudents.Web.Models
{
    public class UserModel
    {
        public int ID { get; set; }
        public int PersonID { get; set; }
        public int LoginCount { get; set; }

        public bool Blocked { get; set; }

        public string Name { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }

        public LoginType LoginType { get; set; }

        public UserModel() { }
        public UserModel(User user)
        {
            ID = user.ID;
            Name = user.Name;
            NickName = user.NickName;

            LoginType = user.LoginType;
            LoginCount = user.LoginCount;
            Blocked = user.Blocked;
            PersonID = user.PersonID;
        }

        public User GenerateUser()
        {
            var result = new User()
            {
                ID = ID,
                Name = Name,
                NickName = NickName,
                Password = Password,
                LoginType = LoginType,
                LoginCount = LoginCount,
                Blocked = Blocked,
                PersonID = PersonID,
            };

            return result;
        }
    }
}
