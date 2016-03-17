<%@ page language="java" pageEncoding="UTF-8"%>
<%@ include file="/common/taglib.jsp" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
  <head>
    
    <title>高级查询</title>
    
  </head>
  
  <body>
    <center>
    <h2>请输查询条件</h2>
    <html:form action="/emp/list" method="post">
    <html:hidden property="query" value="true"/>
	    <table>
	    	<tr>
	    		<td>姓名：</td>
	    		<td><html:text property="name"/></td>
	    	</tr>
	    	<tr>
	    		<td>住址：</td>
	    		<td><html:text property="address"/></td>
	    	</tr>
	    	<tr>
	    		<td>电话：</td>
	    		<td><html:text property="phone"/></td>
	    	</tr>
	    </table>
	<html:button value="返 回" property="back" onclick="javascript:history.go(-1)"/>&nbsp;&nbsp;&nbsp;
   	<html:submit value="查 询" />
    </html:form>
    </center>
  </body>
</html>
