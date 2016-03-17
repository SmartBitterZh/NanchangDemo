<%@ page language="java" pageEncoding="UTF-8"%>
<%@ include file="/common/taglib.jsp" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
  <head>
    
    <title>edit</title>
    
  </head>
  
  <body>
    <center>
    <h2>请输入雇员信息</h2>
    <html:form action="/emp/manage" method="post">
	    <html:hidden property="method" value="edit"/>
	    <html:hidden property="id"/>
	    <table>
	    	<tr>
    			<td>雇员名称:</td>
    			<td><html:text property="name" /></td>
			</tr>
			<tr>
    			<td>雇员住址:</td>
    			<td><html:text property="address" /></td>
    		</tr>
    		<tr>
    			<td>雇员电话:</td>
    			<td><html:text property="phone" /></td>
			</tr>
    	</table>
    	<html:button value="返 回" property="back" onclick="javascript:history.go(-1)"/>&nbsp;&nbsp;&nbsp;
   		<html:submit value="修 改" />
    </html:form>
    </center>
  </body>
</html>
