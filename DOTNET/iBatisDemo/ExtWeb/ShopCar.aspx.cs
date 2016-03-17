using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IBLL;
using SpringIoc;
using Model;
using System.Text;

public partial class ShopCar : System.Web.UI.Page
{
    IShopCarBLL shopCarBLL;

    protected void Page_Load(object sender, EventArgs e)
    {
        shopCarBLL = (IShopCarBLL)SpringContext.Context.CreateInstance("ShopCarBLL");

        string cmd = Request.QueryString["cmd"];
        switch (cmd)
        {
            case "list":
                GetList();
                break;
            case "add":
                Add();
                break;
            case "edit":
                Edit();
                break;
            case "del":
                Del();
                break;
            case "order":
                Order();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 获取购物车产品列表
    /// </summary>
    private void GetList()
    {
        IList<OrderProduct> list = shopCarBLL.GetList();
        int count = list.Count;

        StringBuilder result = new StringBuilder();
        result.Append("{result:[");
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                OrderProduct orderProduct = list[i];
                result.Append("{NO:").Append((i + 1).ToString()).Append(",");
                result.Append("ProductID:\"").Append(orderProduct.ProductID).Append("\",");
                result.Append("ProductName:\"").Append(orderProduct.ProductName).Append("\",");
                result.Append("Price:").Append(orderProduct.Price).Append(",");
                result.Append("Num:").Append(orderProduct.Num).Append("}");
                if (i < count - 1)
                {
                    result.Append(",");
                }
            }
        }

        result.Append("]}");

        Response.Write(result.ToString());
        Response.End();
    }

    /// <summary>
    /// 向购物车添加产品
    /// </summary>
    private void Add()
    {
        string id = Request.QueryString["productId"];
        if (!string.IsNullOrEmpty(id))
        {
            int productId = 0;
            int.TryParse(id, out productId);

            IProductBLL productBLL = (IProductBLL)SpringContext.Context.CreateInstance("ProductBLL");
            Model.Product product = productBLL.Select(productId);

            OrderProduct orderProduct = new OrderProduct();
            orderProduct.Num = 1;
            orderProduct.ProductID = productId;
            orderProduct.ProductName = product.ProductName;

            IUserBLL userBLL = (IUserBLL)SpringContext.Context.CreateInstance("UserBLL");
            ICustomBLL customBLL = (ICustomBLL)SpringContext.Context.CreateInstance("CustomBLL");
            if (customBLL.Select(userBLL.UserID).IsMember)
            {
                orderProduct.Price = product.MemberPrice;
            }
            else
            {
                orderProduct.Price = product.NormalPrice;
            }

            shopCarBLL.Add(orderProduct);

            Response.Write("{success:true}");
        }
        else
        {
            Response.Write("{success:false}");
        }
        Response.End();
    }

    /// <summary>
    /// 编辑产品数量
    /// </summary>
    private void Edit()
    {
        string id = Request.QueryString["productId"];
        if (!string.IsNullOrEmpty(id))
        {
            int productId = 0;
            int.TryParse(id, out productId);

            int num = 1;
            int.TryParse(Request.Form["Num"], out num);
            shopCarBLL.SetNum(productId, num);

            Response.Write("{success:true}");
        }
        else
        {
            Response.Write("{success:false}");
        }
        Response.End();
    }

    /// <summary>
    /// 从购物车中移除产品
    /// </summary>
    private void Del()
    {
        string id = Request.QueryString["productId"];
        if (!string.IsNullOrEmpty(id))
        {
            int productId = 0;
            int.TryParse(id, out productId);

            shopCarBLL.Remove(productId);

            Response.Write("{success:true}");
        }
        else
        {
            Response.Write("{success:false}");
        }
        Response.End();
    }

    private void Order()
    {
        IUserBLL userBLL = (IUserBLL)SpringContext.Context.CreateInstance("UserBLL");

        Model.Order order = new Model.Order();
        order.ID = Guid.NewGuid();
        order.CreateTime = DateTime.Now;
        order.Status = 0;
        order.CustomID = userBLL.UserID;

        order.OrderProducts = shopCarBLL.GetList();
        for (int i = 0; i < order.OrderProducts.Count; i++)
        {
            order.OrderProducts[i].OrderID = order.ID;
        }

        IOrderBLL orderBll = (IOrderBLL)SpringContext.Context.CreateInstance("OrderBLL");
        orderBll.Insert(order);

        Response.Write("{success:true}");
        Response.End();
    }
}
