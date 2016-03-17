<%@ page language="java" pageEncoding="UTF-8"%>
总记录数/总页数&nbsp;:&nbsp;${pageView.totalRecord }/${pageView.totalPage }&nbsp;&nbsp;&nbsp;&nbsp;
   <c:if test="${pageView.currentPage>1 }">
   		<a href="javascript:toPage('1')">首页</a>&nbsp;&nbsp;
   		<a href="javascript:toPage('${pageView.currentPage-1}')">上一页</a>&nbsp;&nbsp;
   </c:if>
   <c:forEach begin="${pageView.pageIndex.startIndex }" end="${pageView.pageIndex.endIndex }" var="i">
   		<c:choose>
   			<c:when test="${pageView.currentPage!=i}">
   				<a href="javascript:toPage('${i}')">${i}</a>&nbsp;&nbsp;
   			</c:when>
   			<c:otherwise>
		   		<font color="red"><b>${i}</b></font>&nbsp;&nbsp;
   			</c:otherwise>
   		</c:choose>
   	</c:forEach>
   	<c:if test="${pageView.currentPage<pageView.totalPage }">
   		<a href="javascript:toPage('${pageView.currentPage+1}')">下一页</a>&nbsp;&nbsp;
   		<a href="javascript:toPage('${pageView.totalPage}')">尾页</a>&nbsp;&nbsp;
   </c:if>
	&nbsp;&nbsp;第<html:text property="page" size="2" onkeypress="inputIntNumberCheck()"/>页&nbsp;&nbsp;每页<html:text property="maxResult" size="2" onkeypress="inputIntNumberCheck()"/>条
	<html:submit value="Go.." />