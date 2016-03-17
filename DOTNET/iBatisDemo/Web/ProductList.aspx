<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProductList.aspx.cs" Inherits="ProductList" %>

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
    <div><h2>欢迎 <asp:Literal ID="ltlCustomName" runat="server" /> 光临选购！</h2></div>
    
    <table border="0" cellspacing="1">
        <tr>
            <th>ID</th>
            <th>ProductName</th>
            <th>NormalPrice</th>
            <th>MemberPrice</th>
            <th>Manage</th>
        </tr>
    <asp:Repeater id="rpt" runat="server" onitemcommand="rpt_ItemCommand">
    <ItemTemplate>
        <tr>
            <td><%# Eval("ID") %></td>
            <td><%# Eval("ProductName") %></td>
            <td>$<%# decimal.Parse(Eval("NormalPrice").ToString()).ToString("#") %></td>
            <td>$<%# decimal.Parse(Eval("MemberPrice").ToString()).ToString("#")%></td>
            <td><a href='ProductEdit.aspx?id=<%# Eval("ID") %>'>Edit</a>
            <asp:LinkButton ID="lnkbtnDel" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="delete" Text="Delete" OnClientClick="return confirm('确定要删除吗？');"></asp:LinkButton>
            <asp:LinkButton ID="lnkbtnPutCar" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="putCar" Text="PutCar"></asp:LinkButton>
            
            </td>
        </tr>
    </ItemTemplate>
    </asp:Repeater>
        <tr>
            <td colspan="5"><a href="ProductEdit.aspx">Add New</a> |
            <a href="ShopCar.aspx">[我的购物车]</a></td>
        </tr>
    </table>
    </form>
</body>
</html>
