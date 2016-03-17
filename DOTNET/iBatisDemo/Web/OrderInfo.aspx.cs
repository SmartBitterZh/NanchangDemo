using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;
using IBLL;
using SpringIoc;

public partial class OrderInfo : System.Web.UI.Page
{
    protected Order _order;

    protected override void OnInit(EventArgs e)
    {
        IOrderBLL orderBLL = (IOrderBLL)SpringContext.Context.CreateSecurityProxyInstance("OrderBLL");
        _order = orderBLL.Select(new Guid(Request.QueryString["id"].ToString()));
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
}
