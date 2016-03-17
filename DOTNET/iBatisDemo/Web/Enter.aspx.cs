using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;
using IBLL;
using SpringIoc;

public partial class Enter : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        int customId;
        int.TryParse(txtCustomId.Text.Trim(),out customId);

        ICustomBLL customBLL = (ICustomBLL)SpringContext.Context.CreateInstance("CustomBLL");
        Custom custom = customBLL.Select(customId);
        if (custom != null)
        {
            IUserBLL userBLL = (IUserBLL)SpringContext.Context.CreateInstance("UserBLL");
            userBLL.Login(customId);
            Response.Redirect("ProductList.aspx", true);
        }

        lblError.Text = "ID不存在！";
    }
}
