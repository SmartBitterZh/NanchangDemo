package com.tarena.filter;

import java.io.IOException;

import javax.servlet.Filter;
import javax.servlet.FilterChain;
import javax.servlet.FilterConfig;
import javax.servlet.ServletException;
import javax.servlet.ServletRequest;
import javax.servlet.ServletResponse;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

public class CharaterFilter implements Filter {
	
	private String encoding = "UTF-8";

	public void destroy() {
		
	}

	public void doFilter(ServletRequest request, ServletResponse response,
			FilterChain chain) throws IOException, ServletException {
		HttpServletRequest req = (HttpServletRequest) request;
		HttpServletResponse res = (HttpServletResponse)response;
		req.setCharacterEncoding(encoding);
		res.setCharacterEncoding(encoding);
		req.setAttribute("contextPath", req.getContextPath());
		chain.doFilter(request, response);
	}

	public void init(FilterConfig filterConfig) throws ServletException {
		try {
			this.encoding = filterConfig.getInitParameter("encoding");
		} catch (Exception e) {
		}
	}	
}
