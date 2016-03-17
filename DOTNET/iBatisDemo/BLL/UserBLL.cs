using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Model;
using IBLL;
using System.Web.Security;

namespace BLL
{
    public class UserBLL : IUserBLL
    {
        public void Login(int customId)
        {
            FormsAuthentication.SetAuthCookie(customId.ToString(), false);
        }
        public bool Logined 
        {
            get
            {
                if (HttpContext.Current.Request.IsAuthenticated)
                {
                    return true;
                }
                return false;
            }
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
            HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            HttpContext.Current.Session.RemoveAll();
        }
        
        public int UserID {
            get
            {
                if (Logined)
                {
                    return int.Parse(HttpContext.Current.User.Identity.Name);
                }
                return 0;
            }
        }
    }
}
