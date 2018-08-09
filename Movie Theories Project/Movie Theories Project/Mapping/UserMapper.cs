using DAL.Models;
using Movie_Theories_Project.Models;

namespace Movie_Theories_Project.Mapping
{
    public class UserMapper
    {
        public UserPO MapDoToPo(UserDO from)
        {
            UserPO to = new UserPO();
            to.UserId = from.UserId;
            to.Username = from.Username;
            to.Password = from.Password;
            to.FirstName = from.FirstName;
            to.Email = from.Email;
            to.Role = from.Role;
            to.RoleName = from.RoleName;

            return to;
        }

        public UserDO MapPoToDo(UserPO from)
        {
            UserDO to = new UserDO();
            to.UserId = from.UserId;
            to.Username = from.Username;
            to.Password = from.Password;
            to.FirstName = from.FirstName;
            to.Email = from.Email;
            to.Role = from.Role;
            to.RoleName = from.RoleName;

            return to;
        }
    }
}