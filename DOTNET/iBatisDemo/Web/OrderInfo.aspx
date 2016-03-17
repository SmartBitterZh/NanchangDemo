<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderInfo.aspx.cs" Inherits="OrderInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
    table{background-color:#666;}
    table tr th{font-size:14px; padding:5px; font-family:Arial;}
    table tr td{ background-color:#e5e5e5; padding:5px; font-size:12px; font-family:Arial;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>OrderID:<%=_order.ID.ToString() %></div>
    <div>CreateTime:<%=_order.CreateTime %></div>
    <div>CustomID:<%=_order.CustomID %></div>
    <div>Stauts:<%=_order.Status %></div>
    <div>产品信息</div>
    <table border="0" cellspacing="1">
        <tr>
            <th>ProductID</th>
            <th>ProductName</th>
            <th>Price</th>
            <th>Num</th>
        </tr>
    <% decimal total=0;
       if (_order.OrderProducts != null)
       {
           foreach (Model.OrderProduct orderProduct in _order.OrderProducts)
           { %>
        <tr>
            <td><%=orderProduct.ProductID%></td>
            <td><%=orderProduct.ProductName%></td>
            <td>$<%=orderProduct.Price.ToString("#")%></td>
            <td><%=orderProduct.Num%></td>
        </tr>
    <%      total += orderProduct.Num * orderProduct.Price;
           }
       }%>
        <tr>
            <td colspan="4">合计：<%=total.ToString("#") %></td>
        </tr>
    </table>
    </form>
</body>
</html>
