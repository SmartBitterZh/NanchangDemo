 package com.tarena.util;

public class WebTool {
	
/**
 * 根据请求数据获得开始页和结束页的索引，如：
 * 			显示页码总数为10，当前为第20页，总页数为30，则获得开始页和结束页的索引为16，25
 * @param viewPageCount 显示页码总数
 * @param currentPage	当前页索引
 * @param totalPage		总页数
 * @return 开始页和结束页的索引
 */
public static PageIndex getPageIndex(long viewPageCount, int currentPage, long totalPage){
		long startPage = currentPage-(viewPageCount%2==0? viewPageCount/2-1 : viewPageCount/2);
		long endPage = currentPage+viewPageCount/2;
		if(startPage<1){
			startPage = 1;
			if(totalPage>=viewPageCount) endPage = viewPageCount;
			else endPage = totalPage;
		}
		if(endPage>totalPage){
			endPage = totalPage;
			if((endPage-viewPageCount)>0) startPage = endPage-viewPageCount+1;
			else startPage = 1;
		}
		return new PageIndex(startPage, endPage);		
  }
}
