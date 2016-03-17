package com.tarena.util;

import java.util.List;

/** 页面视图 */
@SuppressWarnings("unchecked")
public class PageView<T> {
	/** 分页数据 */
	private List<T> records;
	/** 页码开始索引和结束索引 */
	private PageIndex pageIndex;
	/** 总页数 */
	private int totalPage;
	/** 每页显示记录数 */
	private int maxResult;
	/** 当前页 */
	private int currentPage;
	/** 总记录数 */
	private int totalRecord;
	/** 页码数量 */
	private int viewPageCount = Integer.parseInt(ReadResourceMapping.get("viewPageCount"));
	
	public PageView(int currentPage){
		this.currentPage = currentPage;
	}
	
	public PageView(int currentPage,int maxResult){
		this.maxResult = maxResult;
		this.currentPage = currentPage;
	}
	
	public void setQueryResult(QueryResult qr){
		setTotalRecord(qr.getTotal());
		setRecords(qr.getResultList());
	}
	
	public List<T> getRecords() {
		return records;
	}
	public void setRecords(List<T> records) {
		this.records = records;
	}
	public PageIndex getPageIndex() {
		return pageIndex;
	}
	public int getTotalPage() {
		return totalPage;
	}
	public void setTotalPage(int totalPage) {
		this.totalPage = totalPage;
		this.pageIndex = WebTool.getPageIndex(viewPageCount, currentPage, totalPage);
	}
	public int getMaxResult() {
		return maxResult;
	}
	public int getCurrentPage() {
		return currentPage;
	}
	public int getTotalRecord() {
		return totalRecord;
	}
	public void setTotalRecord(int totalRecord) {
		this.totalRecord = totalRecord;
		setTotalPage(this.totalRecord%this.maxResult==0?this.totalRecord/this.maxResult:this.totalRecord/this.maxResult+1);
	}
	public int getViewPageCount() {
		return viewPageCount;
	}
	public void setViewPageCount(int viewPageCount) {
		this.viewPageCount = viewPageCount;
	}
}
