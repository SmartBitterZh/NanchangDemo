<%@ page language="java" pageEncoding="UTF-8"%>
<%@ include file="/common/taglib.jsp" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
  <head>
    
    <title>add</title>
	<script type='text/javascript' src='${contextPath }/dwr/interface/TestEmployee.js'></script>
	<script type='text/javascript' src='${contextPath }/dwr/engine.js'></script>
	<script type='text/javascript' src='${contextPath }/dwr/util.js'></script>
	<script type="text/javascript">
		function testName(empName){
			TestEmployee.testName(empName,
				function(data){
					DWRUtil.setValue("nameMsg",data);
				}
			);
		}
	</script>
	
  </head>
  
  <body>
    <center>
    <h2>请输入雇员信息</h2>
    <html:form action="/emp/manage" method="post">
    <html:hidden property="method" value="add"/>
    	<table>
	    	<tr>
    			<td>雇员名称:</td>
    			<td><html:text property="name" onblur="testName(this.value)"/>
    				<span id="nameMsg"></span>
    			</td>
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
   		<html:submit value="添 加" />
    </html:form>
    </center>
  </body>
</html>
