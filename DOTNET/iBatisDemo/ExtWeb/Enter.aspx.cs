using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IBLL;
using Model;
using SpringIoc;

public partial class Enter : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string cmd = Request.QueryString["cmd"];
        switch (cmd)
        {
            case "login":
                Login();
                break;
            default:
                break;
        }
    }

    private void Login()
    {
        int customId;
        int.TryParse(Request.Form["CustomID"], out customId);

        ICustomBLL customBLL = (ICustomBLL)SpringContext.Context.CreateInstance("CustomBLL");
        Custom custom = customBLL.Select(customId);
        if (custom != null)
        {
            IUserBLL userBLL = (IUserBLL)SpringContext.Context.CreateInstance("UserBLL");
            userBLL.Login(customId);
            Response.Write("{success: true}");
        }
        else
        {
            Response.Write("{success: false,msg: 'CustomID不存在！'}");
        }
        Response.End();
    }
}
