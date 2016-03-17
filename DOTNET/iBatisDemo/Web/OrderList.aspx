<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderList.aspx.cs" Inherits="OrderList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
    table{background-color:#666;}
    table tr th{font-size:14px; padding:5px; font-family:Arial;}
    table tr td{ background-color:#e5e5e5; padding:5px; font-size:12px; font-family:Arial;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div><h2>订单列表</h2></div>
    
    <table border="0" cellspacing="1">
        <tr>
            <th>OrderID</th>
            <th>CreateTime</th>
            <th>CustomID</th>
            <th>Status</th>
            <th>Manage</th>
        </tr>
    <asp:Repeater id="rpt" runat="server" onitemcommand="rpt_ItemCommand">
    <ItemTemplate>
        <tr>
            <td><%# Eval("ID")%></td>
            <td><%# Eval("CreateTime").ToString()%></td>
            <td><%# Eval("CustomID")%></td>
            <td><%# Eval("Status")%></td>
            <td>
            <asp:LinkButton ID="lnkbtnDel" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="delete" Text="Delete" OnClientClick="return confirm('确定要删除吗？');"></asp:LinkButton>
            <a href='OrderInfo.aspx?id=<%# Eval("ID") %>'>Detail</a>
            </td>
        </tr>
    </ItemTemplate>
    </asp:Repeater>
    </table>
    </form>
</body>
</html>
