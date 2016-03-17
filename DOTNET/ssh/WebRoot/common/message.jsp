<%@ page language="java" pageEncoding="UTF-8"%>
<%@ include file="/common/taglib.jsp" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
  <head>
    
    <title>Message</title>

  </head>
  
  <body>
  <center>
  	<br />
    ${message }
    <br /><br />
    <a href="<html:rewrite action="${url }"/>">确定</a>
  	</center>
  </body>
</html>
