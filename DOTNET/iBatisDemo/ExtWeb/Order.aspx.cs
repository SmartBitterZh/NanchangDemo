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
using System.Collections;

public partial class Order : System.Web.UI.Page
{
    private IOrderBLL _orderBLL;

    protected void Page_Load(object sender, EventArgs e)
    {
        _orderBLL = (IOrderBLL)SpringContext.Context.CreateInstance("OrderBLL");

        string cmd = Request.QueryString["cmd"];
        switch (cmd)
        {
            case "list":
                List();
                break;
            case "del":
                Del();
                break;
            default:
                break;
        }
    }

    private void List()
    {
        PagerItem pagerItem = new PagerItem();
        pagerItem.CurrentPage = 1;
        pagerItem.PageSize = 15;
        pagerItem.Keywords = "";

        IList list = _orderBLL.Select(pagerItem);
        int count = list.Count;

        StringBuilder result = new StringBuilder();
        result.Append("{result:[");
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                Model.Order order = (Model.Order)list[i];
                result.Append("{OrderID:\"").Append(order.ID).Append("\",");
                result.Append("CustomID:\"").Append(order.CustomID).Append("\",");
                result.Append("Status:\"").Append(order.Status).Append("\",");
                result.Append("CreateTime:\"").Append(order.CreateTime).Append("\"}");
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
    /// 删除订单
    /// </summary>
    private void Del()
    {
        string id = Request.QueryString["orderId"];
        if (!string.IsNullOrEmpty(id))
        {
            Guid orderId = new Guid(id);
            _orderBLL.Delete(orderId);
            Response.Write("{success:true}");
        }
        else
        {
            Response.Write("{success:false}");
        }
        Response.End();
    }
}
