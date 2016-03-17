using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace IBLL
{
    public interface IUserBLL
    {
        void Login(int customId);
        bool Logined { get; }
        void Logout();
        int UserID { get; }
    }
}
