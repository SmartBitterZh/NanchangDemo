using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using IBLL;
using SpringIoc;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        IUserBLL userBll = (IUserBLL)SpringContext.Context.CreateInstance("UserBLL");
        if (userBll.Logined)
        {
            Response.Redirect("ProductList.aspx", true);
        }
        else
        {
            Response.Redirect("Enter.aspx", true);
        }
    }
}
