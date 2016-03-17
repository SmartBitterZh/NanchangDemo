using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;
using IBLL;
using SpringIoc;

public partial class OrderList : System.Web.UI.Page
{
    private IOrderBLL _orderBLL;

    protected void Page_Load(object sender, EventArgs e)
    {
        _orderBLL = (IOrderBLL)SpringContext.Context.CreateSecurityProxyInstance("OrderBLL");

        if (!IsPostBack)
        {
            BindRepeater();
        }
    }

    private void BindRepeater()
    {
        PagerItem pagerItem = new PagerItem();
        pagerItem.CurrentPage = 1;
        pagerItem.PageSize = 15;
        pagerItem.Keywords = "";
        rpt.DataSource = _orderBLL.Select(pagerItem);
        rpt.DataBind();
    }

    protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string orderId = e.CommandArgument.ToString();
        switch (e.CommandName.ToLower())
        {
            case "delete":
                _orderBLL.Delete(new Guid(orderId));
                Response.Redirect("OrderList.aspx", true);
                break;
            default:
                break;
        }
    }


}
