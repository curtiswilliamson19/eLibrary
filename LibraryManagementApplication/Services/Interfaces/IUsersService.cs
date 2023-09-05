using LibraryManagementApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementApplication.Services
{
    interface IUsersService
    {
        void AddUser(Member user);

        void DeleteUserByID(Guid userID);

        List<Member> GetAllUsers();

        void EditUser(Member user);
    }
}
