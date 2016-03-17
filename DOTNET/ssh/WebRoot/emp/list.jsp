<%@ page language="java" pageEncoding="UTF-8"%>
<%@ include file="/common/taglib.jsp" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
  <head>
    
    <title>list</title>
    <link rel="stylesheet" type="text/css" href="${contextPath }/css/common.css">
	<script type="text/javascript" src="${contextPath }/js/jquery.js"></script>
    <script type="text/javascript" src="${contextPath }/js/jquery.tablesorter.js"></script>
	<script type="text/javascript" src="${contextPath }/js/common.js"></script>
	<script type="text/javascript">

		$(document).ready(
			function(){
				$("#mytable").tablesorter();
			}
		);
			
	</script>
	
  </head>
  
  <body>
  	 <center><h2>雇员信息</h2></center>
  	 <html:form action="/emp/list" method="post" onsubmit="confirm(${pageView.totalPage})">
  	 <html:hidden property="query"/>
  	 <html:hidden property="name"/>
  	 <html:hidden property="address"/>
  	 <html:hidden property="phone"/>
  	 
     <table id="mytable" cellpadding="1" cellspacing="1" border="0" align="center" class="tableBorder">
    	<thead>
    	<tr>
	    	<th style="cursor:pointer" width="10%">编 号</th>
	    	<th style="cursor:pointer" width="20%">姓 名</th>
	    	<th style="cursor:pointer" width="40%">住 址</th>
	    	<th style="cursor:pointer" width="10%">电 话</th>
	    	<th style="width="20%">操 作</th>
    	</tr>
    	</thead>
    	<c:forEach items="${pageView.records}" var="entity" varStatus="status">
    	<tr>
    		<td class="tableBody1"><input type="checkbox" name="ids" value="${entity.id }"/>&nbsp;&nbsp;${status.count }</td>
	    	<td class="tableBody2">${entity.name }</td>
	    	<td class="tableBody1">${entity.address }</td>
	    	<td class="tableBody2">${entity.phone }</td>
	    	<td class="tableBody1"><a href="<html:rewrite action='/emp/manage'/>?method=editUI&id=${entity.id }">修改</a>&nbsp;&nbsp;&nbsp;&nbsp;<a href="<html:rewrite action='/emp/manage'/>?method=delete&id=${entity.id }">删除</a></td>
   		</tr>
    	</c:forEach>
    	
    </table>
    <table cellpadding="1" cellspacing="1" border="0" align="center" class="tableBorder">
    	<tr>
    		<td  width="10%" class="tableBody1"><input type="checkbox" name="all"
				<c:if test="${fn:length(pageView.records)<1}">disabled="disabled"</c:if>
				onclick="selectAll(this,this.form.ids)"/>全选
			</td>
    		<td class="tableBody2" colspan="4">
    			<a href="#" onClick="javascript:deleteSelected('<html:rewrite action="/emp/manage"/>?method=delete')">删除选中</a>&nbsp;&nbsp;&nbsp;&nbsp;
    			<a href="<html:rewrite action='/emp/manage'/>?method=addUI"/>添加雇员</a>&nbsp;&nbsp;&nbsp;&nbsp;
    			<a href="<html:rewrite action='/emp/query'/>">高级查询</a>
    		</td>
    	</tr>
    </table>
	<table width="80%" border="0" align="center">
		<tr>
		    <td>
			    <%@ include file="/common/fenye.jsp" %>
		    </td>
		</tr>
	</table>
    </html:form>
  </body>
</html>
