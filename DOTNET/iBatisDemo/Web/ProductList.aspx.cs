using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;
using IBLL;
using SpringIoc;

public partial class ProductList : System.Web.UI.Page
{
    protected int _currentPage;

    protected void Page_Load(object sender, EventArgs e)
    {
        GetCustomInfo();
        int.TryParse(Request.QueryString["page"], out _currentPage);
        if (_currentPage <= 0)
        {
            _currentPage = 1;
        }

        if (!IsPostBack)
        {
            BindRepeater();
        }
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    private void GetCustomInfo()
    {
        IUserBLL userBLL = (IUserBLL)SpringContext.Context.CreateSecurityProxyInstance("UserBLL");
        ICustomBLL customBLL = (ICustomBLL)SpringContext.Context.CreateSecurityProxyInstance("CustomBLL");
        if (userBLL.Logined)
        {
            ltlCustomName.Text = customBLL.Select(userBLL.UserID).CustomName;
        }
    }

    private void BindRepeater()
    {
        PagerItem pagerItem = new PagerItem();
        pagerItem.CurrentPage = _currentPage;
        pagerItem.PageSize = 15;
        pagerItem.Keywords = "";

        IProductBLL productBLL = (IProductBLL)SpringContext.Context.CreateSecurityProxyInstance("ProductBLL");
        rpt.DataSource = productBLL.Select(pagerItem);
        rpt.DataBind();
    }

    protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        int productId = int.Parse(e.CommandArgument.ToString());
        IProductBLL productBLL = (IProductBLL)SpringContext.Context.CreateSecurityProxyInstance("ProductBLL");

        switch (e.CommandName.ToLower())
        {
            case "delete":
                productBLL.Delete(productId);
                Response.Redirect("ProductList.aspx", true);
                break;
            case "putcar":;
                Product product = productBLL.Select(productId);

                OrderProduct orderProduct = new OrderProduct();
                orderProduct.Num = 1;
                orderProduct.ProductID = productId;
                orderProduct.ProductName = product.ProductName;

                IUserBLL userBLL = (IUserBLL)SpringContext.Context.CreateSecurityProxyInstance("UserBLL");
                ICustomBLL customBLL = (ICustomBLL)SpringContext.Context.CreateSecurityProxyInstance("CustomBLL");
                if (customBLL.Select(userBLL.UserID).IsMember)
                {
                    orderProduct.Price = product.MemberPrice;
                }
                else
                {
                    orderProduct.Price = product.NormalPrice;
                }

                IShopCarBLL shopCarBLL = (IShopCarBLL)SpringContext.Context.CreateSecurityProxyInstance("ShopCarBLL");
                shopCarBLL.Add(orderProduct);
                Response.Redirect("ShopCar.aspx", true);
                break;
            default:
                break;
        }
    }
}
