using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;
using IBLL;
using SpringIoc;

public partial class ShopCar : System.Web.UI.Page
{
    private IShopCarBLL _shopCarBLL;

    protected void Page_Load(object sender, EventArgs e)
    {
        _shopCarBLL = (IShopCarBLL)SpringContext.Context.CreateSecurityProxyInstance("ShopCarBLL");

        if (!IsPostBack)
        {
            BindRepeater();
        }
    }

    private void BindRepeater()
    {
        lblTotal.Text = string.Format(@"合计：${0}", _shopCarBLL.Total.ToString("#"));
        rpt.DataSource = _shopCarBLL.GetList();
        rpt.DataBind();
    }

    protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        int productId = int.Parse(e.CommandArgument.ToString());
        switch (e.CommandName.ToLower())
        {
            case "delete":
                _shopCarBLL.Remove(productId);
                Response.Redirect("ShopCar.aspx", true);
                break;
            case "edit":
                Label lblNum = (Label)e.Item.FindControl("lblNum");
                TextBox txtNum = (TextBox)e.Item.FindControl("txtNum");
                LinkButton lnkbtnEdit = (LinkButton)e.Item.FindControl("lnkbtnEdit");
                LinkButton lnkbtnSave = (LinkButton)e.Item.FindControl("lnkbtnSave");
                LinkButton lnkbtnCancel = (LinkButton)e.Item.FindControl("lnkbtnCancel");

                lblNum.Visible = false;
                txtNum.Visible = true;
                lnkbtnEdit.Visible = false;
                lnkbtnSave.Visible = true;
                lnkbtnCancel.Visible = true;
                txtNum.Text = lblNum.Text;
                break;
            case "cancel":
                Response.Redirect("ShopCar.aspx", true);
                break;
            case "save":
                TextBox txtNumModified = (TextBox)e.Item.FindControl("txtNum");
                int num = 1;
                int.TryParse(txtNumModified.Text.Trim(), out num);
                _shopCarBLL.SetNum(productId, num);
                Response.Redirect("ShopCar.aspx", true);
                break;
            default:
                break;
        }
    }

    protected void btnOrder_Click(object sender, EventArgs e)
    {
        IUserBLL userBll = (IUserBLL)SpringContext.Context.CreateSecurityProxyInstance("UserBLL");

        Order order = new Order();
        order.ID = Guid.NewGuid();
        order.CreateTime = DateTime.Now;
        order.Status = 0;
        order.CustomID = userBll.UserID;

        order.OrderProducts = _shopCarBLL.GetList();
        for (int i = 0; i < order.OrderProducts.Count; i++)
        {
            order.OrderProducts[i].OrderID = order.ID;
        }

        IOrderBLL orderBll = (IOrderBLL)SpringContext.Context.CreateSecurityProxyInstance("OrderBLL");
        orderBll.Insert(order);

        Response.Redirect("OrderList.aspx", true);
    }
}
